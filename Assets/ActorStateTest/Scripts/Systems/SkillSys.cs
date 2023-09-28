using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ActorStateTest.Data;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks.Triggers;
using UniRx;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace ActorStateTest.Systems
{
    public class SkillSys : ITickable
    {
        [Inject] private SkillDataRepo repo;

        private Dictionary<SkillData, List<ActorHandler>> lunchInfoDict = new();
        private Dictionary<SkillData, CancellationToken> tokens = new();
        private Dictionary<Collider, ActorHandler> colliderDict = new();
        private List<SkillInfo> skillInfos = new();

        class SkillInfo
        {
            public ActorHandler ActorHandler;
            public SkillData SkillData;
            public CancellationToken CancelToken;
            public bool IsCooling { get; set; }
        }

        public void SetupColliderInfo(ActorHandler handler)
        {
            handler.GetColliders().ForEach((_, c) => { colliderDict.TryAdd(c, handler); });
        }

        public void AddSkillLunchInfo(ActorHandler handler, SkillData skillData)
        {
            if (!lunchInfoDict.TryGetValue(skillData, out var data))
            {
                data = new();
                lunchInfoDict[skillData] = data;
            }

            if (data.Contains(handler))
            {
                return;
            }

            data.Add(handler);
            var newSkillInfo = new SkillInfo { ActorHandler = handler, SkillData = skillData };
            skillInfos.Add(newSkillInfo);
        }

        public void Tick()
        {
            foreach (var skillInfo in skillInfos)
            {
                if (skillInfo.IsCooling) continue;
                skillInfo.CancelToken = new CancellationToken();
                SkillCooling(skillInfo).Forget();
                LaunchSkill(skillInfo);
            }
        }

        private async UniTaskVoid LaunchSkill(SkillInfo skillInfo)
        {
            var prefab = skillInfo.SkillData.prefab;
            var bullet = GameObject.Instantiate(prefab);
            bullet.transform.position = skillInfo.ActorHandler.GetLaunchPos();
            FlyBullet(bullet, Vector3.right).Forget();

            //timeout and destroy token
            var destroyToken = bullet.transform.GetCancellationTokenOnDestroy();
            var waitAutoDestroy = UniTask.Delay(TimeSpan.FromSeconds(skillInfo.SkillData.duration),
                cancellationToken: destroyToken);

            var collisionResult = new UniTaskCompletionSource<Collider>();
            bullet.transform.GetAsyncTriggerEnterTrigger().ForEachAsync(
                (collider) =>
                {
                    if (colliderDict.TryGetValue(collider, out var otherHandler) && otherHandler != skillInfo.ActorHandler)
                    {
                        collisionResult.TrySetResult(collider);
                    }
                }, cancellationToken: destroyToken);

            var result = await UniTask.WhenAny(waitAutoDestroy, collisionResult.Task);
            if (result == 1)
            {
                Debug.Log($"{collisionResult.GetResult(0)}");
            }
            Object.Destroy(bullet);
            // var timeoutToken = new CancellationTokenSource();
            // timeoutToken.CancelAfterSlim(TimeSpan.FromSeconds(skillInfo.SkillData.duration));
            // bullet.transform.GetAsyncCollisionEnterTrigger().
        }

        private async UniTaskVoid FlyBullet(GameObject bullet, Vector3 right)
        {
            while (true)
            {
                await UniTask.Yield(bullet.transform.GetCancellationTokenOnDestroy());
                bullet.transform.position += right;
            }
        }

        private async UniTaskVoid SkillCooling(SkillInfo skillInfo)
        {
            skillInfo.IsCooling = true;
            var atkSpeed = skillInfo.ActorHandler.GetRuntimeProperty("AtkSpeed");
            atkSpeed = Mathf.Clamp(atkSpeed, 0.1f, 99999f);
            await UniTask.WaitForSeconds(skillInfo.SkillData.CoolingTime / atkSpeed,
                cancellationToken: skillInfo.CancelToken);
            skillInfo.IsCooling = false;
        }

        public void AddSkillLunchInfo(ActorHandler handler, string keyName)
        {
            var skillData = repo.Datas.Where(x => x.skillObject.GetKeyName() == keyName).ToList().FirstOrDefault();
            AddSkillLunchInfo(handler, skillData);
        }
    }
}
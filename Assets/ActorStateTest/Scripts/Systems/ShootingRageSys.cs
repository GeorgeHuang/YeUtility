using System;
using System.Collections.Generic;
using ActorStateTest.Element;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace ActorStateTest.Systems
{
    public class ShootingRageSys : ITickable, IInitializable
    {
        [Inject] private InputState inputState;
        [Inject] private ShootingRageConfig config;
        [Inject] private ActorMgr actorMgr;

        private ActorHandler mainActorStateHandler;
        private List<ActorHandler> enemys = new();

        public void Initialize()
        {
            Setup();
        }

        // private async UniTaskVoid AAA()
        // {
        //     var task1 = UniTask.Delay(TimeSpan.FromSeconds(1));
        //     var source = new UniTaskCompletionSource<Collision>();
        //     var r = await UniTask.WhenAny(task1, source.Task);
        // }

        private async void Setup()
        {
            await UniTask.WaitForSeconds(0.25f);
            mainActorStateHandler = actorMgr.CreatePlayer(config.PlayerDataName);
            mainActorStateHandler.SetPos(new Vector3(-5, 0, 0));
            inputState.MovePress.Subscribe(InputMovePress);

            UniTask.Void(CheckEnemyNumber);
        }

        private async UniTaskVoid CheckEnemyNumber()
        {
            while (true)
            {
                if (enemys.Count < config.MaxEnemyNumber)
                {
                    var newEnemy = actorMgr.CreatePlayer(config.EnemyDataName);
                    var posX = Random.Range(0, 10);
                    newEnemy.SetPos(new Vector3(posX, 0 , 0));
                    enemys.Add(newEnemy);
                }
                await UniTask.Yield();
            }
        }

        private void InputMovePress(Vector2 dir)
        {
            mainActorStateHandler.Move(dir);
        }


        public void Tick()
        {
        }
    }
}
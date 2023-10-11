using System.Collections.Generic;
using ActorStateTest.Data;
using ActorStateTest.Element;
using UnityEngine;
using YeActorState;
using YeActorState.RuntimeCore;
using Zenject;

namespace ActorStateTest.Systems
{
    public class ActorHandler : IPropertyProvider, IInitializable
    {
        [Inject] private readonly GameObject gameObject;
        [Inject] private readonly ActorStateHandler yeActorHandler;
        [Inject] private Player player;
        [Inject] private SkillSys skillSys;

        public void Initialize()
        {
            skillSys.SetupColliderInfo(this);
            player.Setup(this);
        }

        public float GetRuntimeProperty(string propertyName)
        {
            return yeActorHandler.GetRuntimeProperty(propertyName);
        }

        public void Move(Vector3 moveDir)
        {
            player.Move(moveDir);
        }

        public bool Compare(ActorStateHandler actorStateHandler)
        {
            return yeActorHandler == actorStateHandler;
        }

        public void AddSkill(SkillObject skillObject)
        {
            skillSys.AddSkillLunchInfo(this, skillObject.GetKeyName());
        }

        public void SetPos(Vector3 pos)
        {
            player.SetPos(pos);
        }

        public Vector3 GetLaunchPos()
        {
            return player.GetPos();
        }

        public IEnumerable<Collider> GetColliders()
        {
            return player.GetColliders();
        }

        public void Attack(ActorHandler otherHandler, SkillData skillData)
        {
            yeActorHandler.Attack(otherHandler.yeActorHandler, skillData.skillObject);
        }
    }
}
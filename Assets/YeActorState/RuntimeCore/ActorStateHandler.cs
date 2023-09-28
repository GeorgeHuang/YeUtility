using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YeActorState.RuntimeCore;
using Zenject;

namespace YeActorState
{
    public class ActorStateHandler
    {
        [field: Inject] internal YeActorBaseData ActorBaseData { get; }
        [field: Inject] internal YeActorRuntimeData RuntimeData { get; set; }
        [Inject] private YeActorStateSys yeActorStateSys;
        
        public Hashtable AllRuntimeProperties => RuntimeData.AllProperties;
        public bool IsDirty { get; set; }

        public float GetRuntimeProperty(string propertyName)
        {
            return RuntimeData.GetProperty(propertyName);
        }

        public string GetBaseDataName()
        {
            return ActorBaseData.name;
        }

        public void SetProperty(string propertyName, float value)
        {
            RuntimeData.SetProperty(propertyName, value);
        }

        public void ApplyEffect(PropertyEffectData propertyEffectData)
        {
            yeActorStateSys.ApplyEffect(propertyEffectData, this);
        }

        public List<PropertyEffectData> GetCurrentEffectList()
        {
            return yeActorStateSys.GetCurrentEffectList(this);
        }

        public void DeleteEffect(PropertyEffectData propertyEffectData)
        {
            yeActorStateSys.DeleteEffect(propertyEffectData, this);
        }

        public void AddSkill(SkillObject skillObject)
        {
            yeActorStateSys.AddSkill(skillObject, this);
        }

        public List<RuntimeSkill> GetSkillList()
        {
            return yeActorStateSys.GetRuntimeList(this);
        }

        public void Attack(ActorStateHandler otherHandler, SkillObject skillObject)
        {
            yeActorStateSys.Attack(this, otherHandler, skillObject);
        }

        public void DealDamage(float damage)
        {
            var curHp = GetRuntimeProperty("CurHp");
            curHp = Mathf.Max(curHp - damage, 0);
            RuntimeData.SetProperty("CurHp", curHp);
        }

        public void SetActorDirty()
        {
            yeActorStateSys.SetActorDirty(this);
        }

        public void SetSkillDirty()
        {
            yeActorStateSys.SetSkillDirty(this);
        }
    }
}
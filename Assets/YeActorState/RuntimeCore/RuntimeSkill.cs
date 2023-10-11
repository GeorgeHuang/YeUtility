using UnityEngine;
using YeUtility;

namespace YeActorState.RuntimeCore
{
    public class RuntimeSkill : INamedObject
    {
        private readonly ActorStateHandler actorStateHandler;
        private int maxLv;
        private readonly SkillObject skillObject;

        public RuntimeSkill(SkillObject skillObject, ActorStateHandler actorStateHandler)
        {
            this.skillObject = skillObject;
            this.actorStateHandler = actorStateHandler;
            maxLv = skillObject.baseDamage.values.Count;
        }

        public float Damage { get; private set; }
        public bool IsDirty { get; set; }

        public int Lv { get; private set; } = 1;

        public string GetDisplayName()
        {
            return skillObject.GetDisplayName();
        }

        public string GetKeyName()
        {
            return skillObject.GetKeyName();
        }

        public void AddLv(int addValue)
        {
            Lv = Mathf.Clamp(Lv + addValue, 1, skillObject.baseDamage.values.Count);
        }

        public void Calculate()
        {
            var baseDamage = actorStateHandler.GetRuntimeProperty(skillObject.baseDamage.propertyName) *
                             skillObject.baseDamage.values[Lv - 1] * 0.01f;

            foreach (var tagEffect in skillObject.tagEffectList)
            {
                var propertyValue = actorStateHandler.GetRuntimeProperty(tagEffect.tagName);
                baseDamage *= 1 + propertyValue * 0.01f * tagEffect.value;
            }

            foreach (var tagEffect in skillObject.customEffects)
            {
                var propertyValue = actorStateHandler.GetRuntimeProperty(tagEffect.propertyName);
                baseDamage *= 1 + propertyValue * 0.01f * tagEffect.value;
            }

            Damage = baseDamage;
        }

        public bool Compare(SkillObject o)
        {
            return o.GetKeyName() == skillObject.GetKeyName();
        }
    }
}
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using YeActorState.RuntimeCore;
using YeUtility;

namespace ActorStateTest.Data
{
    public class SkillData : NamedScriptableObject
    {
        public Sprite icon;
        public GameObject prefab;
        public float duration;
        public float Speed;
        public float CoolingTime;
        [ValueDropdown("@YeActorStateEditorHelper.Skills")]
        public SkillObject skillObject;
    }
}
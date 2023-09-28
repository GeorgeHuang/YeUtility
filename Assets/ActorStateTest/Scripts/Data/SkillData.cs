using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using YeActorState.RuntimeCore;
using YeUtility.Scripts;

namespace ActorStateTest.Data
{
    public class SkillData : NamedScriptableObject
    {
        public Sprite icon;
        public GameObject prefab;

        public float duration;
        
        [ValueDropdown("@YeActorStateEditorHelper.Skills")]
        public SkillObject skillObject;

        public float CoolingTime;
    }
}
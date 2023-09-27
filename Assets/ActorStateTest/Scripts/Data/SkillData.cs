using Sirenix.OdinInspector;
using UnityEngine;
using YeActorState.RuntimeCore;
using YeUtility.Scripts;

namespace ActorStateTest.Data
{
    public class SkillData : NamedScriptableObject
    {
        public Sprite icon;
        public GameObject prefab;
        
        [ValueDropdown("@YeActorStateEditorHelper.Skills")]
        public SkillObject skillObject;

        public float CoolingTime;
    }
}
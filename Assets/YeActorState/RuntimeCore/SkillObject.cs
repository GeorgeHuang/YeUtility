using System;
using System.Collections.Generic;
using CommonUnit;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace YeActorState.RuntimeCore
{
    [Serializable]
    public class SkillObject : NamedObject
    {
        public List<TagEffect> tagEffectList;
        public List<CustomEffect> customEffects;
        
        [Serializable]
        public class TagEffect
        {
            [ValueDropdown("@YeActorStateEditorHelper.Tags")]
            public string tagName;
            public float value = 1;
        }
        
        [Serializable]
        public class CustomEffect
        {
            public string propertyName;
            public float value = 1;
        }
    }
}
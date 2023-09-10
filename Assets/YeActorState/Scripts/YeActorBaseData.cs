using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace YeActorState
{
    public class YeActorBaseData : SerializedScriptableObject 
    {
        [SerializeField] private List<PropertyData> properties = new ();
        
        [Serializable]
        public class PropertyData
        {
            public string name;
            public float value;


            public string Name
            {
                get => name;
                private set => name = value;
            }

            public float Value
            {
                get => value;
                set => this.value = value;
            }
        }

        public class PropertyNames : ScriptableObject
        {
            [SerializeField] private List<string> _names = new();
        }
    }
}
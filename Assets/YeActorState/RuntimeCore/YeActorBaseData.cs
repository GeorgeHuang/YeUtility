using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace YeActorState
{
    public class YeActorBaseData : SerializedScriptableObject 
    {
        [SerializeField] public List<PropertyData> properties = new ();

        public float GetProperty(string _name)
        {
            return properties.FirstOrDefault(x => x.Name == _name)!.value;
        }

        public void AddPropertyData(string _name, float value = 0)
        {
            if (properties.Any(x => x.Name == _name))
                return;
            properties.Add(new PropertyData{name = _name, value = value});
        }

        [Serializable]
        public class PropertyData
        {
            [ValueDropdown("@YeActorStateEditorHelper.PropertyNames")]
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

    }
}
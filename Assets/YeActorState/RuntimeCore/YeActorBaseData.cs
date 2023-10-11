using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

namespace YeActorState
{
    public class YeActorBaseData : SerializedScriptableObject
    {
        [SerializeField] public List<PropertyData> properties = new();

        public float GetProperty(string _name)
        {
            foreach (var propertyData in properties)
                if (propertyData.Name == _name)
                    return propertyData.value;

            Assert.IsFalse(true, $"找不到 Base Property {_name}");
            return 0;
        }

        public void AddPropertyData(string _name, float value = 0)
        {
            if (properties.Any(x => x.Name == _name))
                return;
            properties.Add(new PropertyData { name = _name, value = value });
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
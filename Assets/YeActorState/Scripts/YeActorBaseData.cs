using System;
using UnityEngine;

namespace YeActorState
{
    [Serializable]
    public class YeActorBaseData : ScriptableObject
    {
        [Serializable]
        public class PropertyData
        {
            [SerializeField] private string name;
            [SerializeField] private float value;


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
using System.Collections.Generic;
using UnityEngine;

namespace YeActorState
{
    public class PropertyNames : ScriptableObject
    {
        [SerializeField] private List<Data> datas = new();
        public class Data
        {
            [SerializeField] private string displayName;
            [SerializeField] private string name;
            [SerializeField] private string terms;
        }
    }
}
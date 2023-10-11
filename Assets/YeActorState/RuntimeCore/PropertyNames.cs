using System;
using System.Linq;
using UnityEngine;
using YeUtility;

namespace YeActorState
{
    [CreateAssetMenu(menuName = "Tools/Create PropertyNames", fileName = "PropertyNames", order = 0)]
    public class PropertyNames : YeObjectRepo<PropertyNames, PropertyNames.Data>
    {
        public string GetDisplayName(string propertyName)
        {
            foreach (var data in datas.Where(data => data.GetKeyName() == propertyName)) return data.displayName;

            return propertyName;
        }

        [Serializable]
        public class Data : INamedObject
        {
            public string displayName;
            public string name;
            public string terms;

            public string GetDisplayName()
            {
                return displayName;
            }

            public string GetKeyName()
            {
                return name;
            }
        }
    }
}
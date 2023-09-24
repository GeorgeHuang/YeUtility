using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using YeUtility;

namespace YeActorState
{
    [CreateAssetMenu(menuName = "Tools/Create PropertyNames", fileName = "PropertyNames", order = 0)]
    public class PropertyNames : ObjectRepo<PropertyNames, PropertyNames.Data>
    {
        
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

        public string GetDisplayName(string propertyName)
        {
            foreach (var data in datas.Where(data => data.GetKeyName() == propertyName))
            {
                return data.displayName;
            }

            return propertyName;
        }
    }
}
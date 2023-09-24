using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace YeActorState
{
    public class YeActorRuntimeData
    {
        private Hashtable properties = new();

        public void Setup(YeActorBaseData baseData)
        {
            foreach (var data in baseData.properties)
            {
                properties.Add(data.Name, data.value);
            }
        }

        public float GetProperty(string key)
        {
            if (properties.ContainsKey(key) == false)
                properties.Add(key, 0);
            return Convert.ToSingle(properties[key]);
        }
    }
}
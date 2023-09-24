using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace YeActorState.RuntimeCore
{
    public class YeActorRuntimeData
    {
        private Hashtable properties = new();
        public Hashtable AllProperties => properties;

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

        public void SetProperty(string key, float value)
        {
            if (properties.ContainsKey(key) == false)
                properties.Add(key, 0);
            properties[key] = value;
        }
    }
}
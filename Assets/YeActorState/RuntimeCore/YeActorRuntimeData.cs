using System;
using System.Collections;

namespace YeActorState.RuntimeCore
{
    public class YeActorRuntimeData
    {
        public Hashtable AllProperties { get; } = new();

        public void Setup(YeActorBaseData baseData)
        {
            foreach (var data in baseData.properties) AllProperties.Add(data.Name, data.value);
        }

        public float GetProperty(string key)
        {
            if (AllProperties.ContainsKey(key) == false)
                AllProperties.Add(key, 0);
            return Convert.ToSingle(AllProperties[key]);
        }

        public void SetProperty(string key, float value)
        {
            if (AllProperties.ContainsKey(key) == false)
                AllProperties.Add(key, 0);
            AllProperties[key] = value;
        }
    }
}
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using YeUtility;

namespace YeActorState.RuntimeCore
{
    [Serializable]
    public class PropertyEffectData : IBasePropertyProcessor, INamedObject
    {
        [SerializeField] private string name;
        [SerializeField] private string key;
        [SerializeField] private List<Data> datas = new();

        public enum EffectType
        {
            BaseToRuntime,
            RuntimeToRuntime
        }

        [Serializable]
        public class Data
        {
            [ValueDropdown("@YeActorStateEditorHelper.PropertyNames")]
            public string sourcePropertyName;

            [ValueDropdown("@YeActorStateEditorHelper.PropertyNames")]
            public string targetPropertyName;

            public EffectType effectType;
            public float value;
        }

        public IEnumerable<Data> Datas => datas;

        public void Processor(YeActorBaseData baseData, YeActorRuntimeData runtimeData)
        {
            foreach (var data in datas)
            {
                ProcessorData(data, baseData, runtimeData);
            }
        }

        private static void ProcessorData(Data data, YeActorBaseData baseData, YeActorRuntimeData runtimeData)
        {
            float runTimeValue;
            switch (data.effectType)
            {
                case EffectType.BaseToRuntime:
                    var baseValue = baseData.GetProperty(data.sourcePropertyName);
                    runTimeValue = runtimeData.GetProperty(data.targetPropertyName);
                    runtimeData.SetProperty(data.targetPropertyName, baseValue + runTimeValue + data.value);
                    break;
                case EffectType.RuntimeToRuntime:
                    runTimeValue = runtimeData.GetProperty(data.sourcePropertyName);
                    runtimeData.SetProperty(data.targetPropertyName, runTimeValue + data.value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public string GetDisplayName()
        {
            return name;
        }

        public string GetKeyName()
        {
            return key;
        }
    }
}
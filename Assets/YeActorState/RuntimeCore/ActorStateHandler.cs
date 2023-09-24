using System.Collections;
using YeActorState.RuntimeCore;
using Zenject;

namespace YeActorState
{
    public class ActorStateHandler
    {
        [field: Inject] internal YeActorBaseData ActorBaseData { get; }
        [field: Inject] internal YeActorRuntimeData RuntimeData { get; }
        [Inject] private YeActorStateSys yeActorStateSys;
        
        public Hashtable AllRuntimeProperties => RuntimeData.AllProperties;

        public float GetRuntimeProperty(string propertyName)
        {
            return RuntimeData.GetProperty(propertyName);
        }

        public string GetBaseDataName()
        {
            return ActorBaseData.name;
        }

        public void SetProperty(string propertyName, float value)
        {
            RuntimeData.SetProperty(propertyName, value);
        }

        public void ApplyEffect(PropertyEffectData propertyEffectData)
        {
            yeActorStateSys.ApplyEffect(propertyEffectData, this);
        }
    }
}
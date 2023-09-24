using System.Collections;
using YeActorState.RuntimeCore;
using Zenject;

namespace YeActorState
{
    public class ActorStateHandler
    {
        [Inject] private YeActorBaseData actorBaseData;
        [Inject] private YeActorRuntimeData runtimeData;
        public Hashtable AllRuntimeProperties => runtimeData.AllProperties;

        public float GetRuntimeProperty(string propertyName)
        {
            return runtimeData.GetProperty(propertyName);
        }

        public string GetBaseDataName()
        {
            return actorBaseData.name;
        }

        public void SetProperty(string propertyName, float value)
        {
            runtimeData.SetProperty(propertyName, value);
        }
    
    }
}
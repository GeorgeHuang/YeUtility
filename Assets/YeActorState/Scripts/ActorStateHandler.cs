using Zenject;

namespace YeActorState
{
    public class ActorStateHandler
    {
        [Inject] private YeActorBaseData actorBaseData;
        [Inject] private YeActorRuntimeData runtimeData;

        public float GetProperty(string propertyName)
        {
            return runtimeData.GetProperty(propertyName);
        }
    }
}
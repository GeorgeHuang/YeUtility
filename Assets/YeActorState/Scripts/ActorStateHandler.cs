using Zenject;

namespace YeActorState
{
    public class ActorStateHandler
    {
        private YeActorBaseData actorBaseData;

        public ActorStateHandler(YeActorBaseData actorBaseData)
        {
            this.actorBaseData = actorBaseData;
        }
    }
}
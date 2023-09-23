using UnityEngine;
using YeActorState;

namespace ActorStateTest.Systems
{
    public class ActorHandler
    {
        private readonly ActorStateHandler yeActorHandler;
        private readonly GameObject gameObject;

        public ActorHandler(ActorStateHandler yeActorHandler, GameObject gameObject)
        {
            this.yeActorHandler = yeActorHandler;
            this.gameObject = gameObject;
        }
    }
}
using UnityEngine;
using YeActorState;
using Zenject;
using Input = UnityEngine.Input;

namespace ActorStateTest.Systems
{
    public class ShootingRageSys : ITickable, IInitializable
    {
        [Inject] private ShootingRageConfig _config;
        [Inject] private ActorMgr actorMgr;

        private ActorHandler mainActorStateHandler;

        public void Initialize()
        {
            mainActorStateHandler = actorMgr.CreatePlayer(_config.PlayerDataName);
        }

        public void Tick()
        {
            var key = Input.GetKeyDown(KeyCode.A);
            if (key)
            {
                mainActorStateHandler.Move(Vector3.left);
            }
        }
    }
}
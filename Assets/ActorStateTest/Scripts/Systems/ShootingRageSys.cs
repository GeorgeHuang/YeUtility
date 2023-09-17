using UnityEngine;
using YeActorState;
using Zenject;
using Input = UnityEngine.Input;

namespace ActorStateTest.Systems
{
    public class ShootingRageSys : ITickable, IInitializable
    {
        [Inject] private YeActorBaseDataRepo _actorBaseDataRepo;
        [Inject] private YeActorStateSys _actorStateSys;
        [Inject] private ShootingRageConfig _config;
        
        public void Initialize()
        {
            
        }
        
        public void Tick()
        {
            var key = Input.GetKeyUp(KeyCode.A);
            if (key)
            {
                var rv = _actorStateSys.AddActor(_actorBaseDataRepo.GetData(_config.PlayerDataName));
                Debug.Log(rv);
            }
        }

    }
}
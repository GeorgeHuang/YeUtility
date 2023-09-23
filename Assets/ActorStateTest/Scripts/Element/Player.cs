using ActorStateTest.Systems;
using UnityEngine;
using YeActorState;
using Zenject;

namespace ActorStateTest.Element
{
    public class Player : MonoBehaviour, IInitializable
    {
        [Inject]
        private ActorStateHandler actorStateHandler;

        [Inject] private TimeSys timeSys;
        
        public Transform Trans { get; private set; }

        public void Move(Vector3 dir)
        {
            Trans.Translate(dir * actorStateHandler.GetProperty("MoveSpeed") * timeSys.DeltaTime);
        }

        public void Initialize()
        {
            Trans = transform;
        }

    }
}
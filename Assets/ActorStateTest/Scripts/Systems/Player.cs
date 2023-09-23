using UnityEngine;
using UnityEngine.PlayerLoop;
using YeActorState;
using Zenject;

namespace ActorStateTest.Systems
{
    public class Player : MonoBehaviour, IInitializable
    {
        [Inject]
        private ActorStateHandler actorStateHandler;
        
        public Transform Trans { get; private set; }

        public void Move(Vector3 dir)
        {
            Trans.Translate(dir * actorStateHandler.GetProperty("MoveSpeed"));    
        }

        public void Initialize()
        {
            Trans = transform;
        }

    }
}
using UnityEngine;
using Zenject;

namespace ActorStateTest.Element
{
    public class MoveHandler : IInitializable, ITickable
    {
        [Inject] private Transform trans;
        
        public void Initialize()
        {
        }

        public void Tick()
        {
        }

        public void Move(Vector3 dir)
        {
            Debug.Log("move handler move");
        }
    }
}
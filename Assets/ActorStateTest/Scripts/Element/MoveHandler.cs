using UnityEngine;
using Zenject;

namespace ActorStateTest.Element
{
    public class MoveHandler : IInitializable, ITickable
    {
        [Inject] private Player player;
        
        public void Initialize()
        {
            Debug.Log($"move handler move {this.GetHashCode()} {player.GetHashCode()}");
        }

        public void Tick()
        {
        }

        public void Move(Vector3 dir)
        {
            Debug.Log($"move handler move {this.GetHashCode()} {player.GetHashCode()}");
        }
    }
}
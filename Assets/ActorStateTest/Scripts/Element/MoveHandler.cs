using ActorStateTest.Systems;
using UnityEngine;
using Zenject;

namespace ActorStateTest.Element
{
    public class MoveHandler : IInitializable, ITickable
    {
        [Inject] private Player player;

        [Inject] TimeSys timeSys;

        public void Initialize()
        {
            //Debug.Log($"move handler move {this.GetHashCode()} {player.GetHashCode()}");
        }

        public void Tick()
        {
        }

        public void Move(Vector3 dir)
        {
            var moveSpeed = player.GetProperty("MoveSpeed");
            player.SetPos(player.GetPos() + dir * moveSpeed * timeSys.DeltaTime);
        }
    }
}
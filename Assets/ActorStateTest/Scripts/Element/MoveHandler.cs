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
            var moveSpeedRatio = 1 + player.GetProperty("MoveSpeedRatio") * 0.01f;
            var realMoveSpeed = moveSpeed * moveSpeedRatio;
            player.SetPos(player.GetPos() + dir * realMoveSpeed * timeSys.DeltaTime);
        }
    }
}
using UnityEngine;
using YeActorState;
using Zenject;

namespace ActorStateTest.Systems
{
    public class ActorHandler
    {
        [Inject] private readonly ActorStateHandler yeActorHandler;
        [Inject] private readonly GameObject gameObject;
        [Inject] private Player player;

        public void Move(Vector3 moveDir)
        {
            player.Move(moveDir);
        }
    }
}
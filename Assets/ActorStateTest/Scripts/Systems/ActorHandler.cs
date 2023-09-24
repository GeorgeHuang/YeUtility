using ActorStateTest.Data;
using ActorStateTest.Element;
using UnityEngine;
using YeActorState;
using Zenject;

namespace ActorStateTest.Systems
{
    public class ActorHandler : IPropertyProvider
    {
        [Inject] private readonly ActorStateHandler yeActorHandler;
        [Inject] private readonly GameObject gameObject;
        [Inject] private Player player;

        public void Move(Vector3 moveDir)
        {
            player.Move(moveDir);
        }

        public float GetRuntimeProperty(string propertyName)
        {
            return yeActorHandler.GetRuntimeProperty(propertyName);
        }
    }
}
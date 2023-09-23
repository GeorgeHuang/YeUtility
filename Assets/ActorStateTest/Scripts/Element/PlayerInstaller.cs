using UnityEngine;
using YeActorState;
using Zenject;

namespace ActorStateTest.Element
{
    public class PlayerInstaller : MonoInstaller
    {
        [Inject] private ActorStateHandler actorStateHandler;
        
        public override void InstallBindings()
        {
            Container.BindInstance(transform);
            Container.BindInstance(actorStateHandler);
            Container.BindInterfacesAndSelfTo<MoveHandler>().AsSingle();
            var player = GetComponent<Player>();
            Container.Inject(player);
        }
    }
}
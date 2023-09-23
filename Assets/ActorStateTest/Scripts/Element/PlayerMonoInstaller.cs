using UnityEngine;
using YeActorState;
using Zenject;

namespace ActorStateTest.Element
{
    public class PlayerMonoInstaller : MonoInstaller
    {
        public ActorStateHandler ActorStateHandler { get; set; }

        [SerializeField] private Player player;


        public override void InstallBindings()
        {
            //Container.BindInterfacesAndSelfTo<Player>().FromInstance(player);
            Container.BindInstance(transform);
            Container.BindInstance(ActorStateHandler);
            Container.BindInterfacesAndSelfTo<MoveHandler>().AsSingle();
            Container.Inject(player);
        }
    }
}
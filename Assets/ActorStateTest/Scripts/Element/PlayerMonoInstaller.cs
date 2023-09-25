using UnityEngine;
using YeActorState;
using Zenject;

namespace ActorStateTest.Element
{
    public class PlayerMonoInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInstance(transform);
            Container.BindInterfacesAndSelfTo<MoveHandler>().AsSingle();
        }
    }
}
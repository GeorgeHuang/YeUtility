using UnityEngine;
using YeActorState.RuntimeCore;
using Zenject;

namespace YeActorState
{
    public class ActorStateSysBind : MonoInstaller
    {
        public override void InstallBindings()
        {
            //Container.Bind<ActorStateHandler>().FromInstance(new ActorStateHandler(null));
            Container.Bind<YeActorStateSys>().AsSingle();
        }
    }
}
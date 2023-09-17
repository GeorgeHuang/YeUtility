using UnityEngine;
using Zenject;

namespace YeActorState
{
    public class ActorStateSysBind : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<YeActorStateSys>().AsSingle();
        }
    }
}
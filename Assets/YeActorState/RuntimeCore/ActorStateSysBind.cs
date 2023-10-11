using YeActorState.RuntimeCore;
using Zenject;

namespace YeActorState
{
    public class ActorStateSysBind : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<YeActorStateSys>().AsSingle();
        }
    }
}
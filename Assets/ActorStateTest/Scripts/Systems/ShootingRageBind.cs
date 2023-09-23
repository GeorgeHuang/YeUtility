using ActorStateTest.Element;
using Zenject;

namespace ActorStateTest.Systems
{
    public class ShootingRageBind : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputState>().AsSingle();
            Container.BindInterfacesAndSelfTo<TimeSys>().AsSingle();
            Container.BindInterfacesAndSelfTo<ShootingRageSys>().AsSingle();
            Container.BindInterfacesAndSelfTo<ActorMgr>().AsSingle();
        }
    }
}
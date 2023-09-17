using Zenject;

namespace ActorStateTest.Systems
{
    public class ShootingRageBind : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ShootingRageSys>().AsSingle();
        }
    }
}
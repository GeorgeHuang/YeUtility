using YeActorState;
using Zenject;

namespace ActorStateTest.Element
{
    public class PlayerInstaller : Installer
    {
        private readonly ActorStateHandler actorStateHandler;

        public PlayerInstaller(ActorStateHandler actorStateHandler)
        {
            this.actorStateHandler = actorStateHandler;
        }

        public override void InstallBindings()
        {
            Container.BindInstance(actorStateHandler);
            Container.BindInterfacesAndSelfTo<MoveHandler>().AsSingle();
        }
    }
}
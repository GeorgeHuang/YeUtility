using System.Collections.Generic;
using ActorStateTest.Data;
using ActorStateTest.Element;
using UnityEngine.Assertions;
using YeActorState;
using Zenject;

namespace ActorStateTest.Systems
{
    public class ActorMgr
    {
        [Inject] private ActorDataRepo actorDataRepo;
        [Inject] private YeActorStateSys yeActorStateSys;
        [Inject] private DiContainer container;
        private bool bind = false;

        public ActorHandler CreatePlayer(string name)
        {
            var actorData = actorDataRepo.GetData(name);
            Assert.IsNotNull(actorData, $"沒找到角色 {name}");

            var yeActorHandler = yeActorStateSys.AddActor(actorData.yeActorBaseData);

            var player = container.InstantiatePrefabForComponent<Player>(actorData.modelPrefab);
            player.ActorStateHandler = yeActorHandler;

            ActorHandler rv = new();
            var perimeter = new List<object>
            {
                yeActorHandler,
                player.gameObject,
                player
            };
            container.Inject(rv, perimeter);

            return rv;
        }

        private void PlayerInstallerMethod(DiContainer diContainer, ActorStateHandler actorStateHandler)
        {
            //diContainer.Bind<Player>().FromComponentOnRoot().AsSingle();
            diContainer.BindInstance(actorStateHandler);
            diContainer.BindInterfacesAndSelfTo<MoveHandler>().AsSingle();
        }
    }
}
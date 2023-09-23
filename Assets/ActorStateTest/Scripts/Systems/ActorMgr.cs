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

        public ActorHandler CreatePlayer(string name)
        {
            var actorData = actorDataRepo.GetData(name);
            Assert.IsNotNull(actorData, $"沒找到角色 {name}");

            var yeActorHandler = yeActorStateSys.AddActor(actorData.yeActorBaseData);

            var perimeter = new List<object> { yeActorHandler };

            var player = container.InstantiatePrefabForComponent<Player>(actorData.modelPrefab, perimeter);
            var playerInstaller = player.GetComponent<PlayerInstaller>();
            container.Inject(playerInstaller, perimeter);
            playerInstaller.InstallBindings();

            ActorHandler rv = new();
            perimeter.Add(player.gameObject);
            perimeter.Add(player);
            container.Inject(rv, perimeter);

            return rv;
        }
    }
}
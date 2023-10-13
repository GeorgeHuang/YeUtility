using System.Collections.Generic;
using ActorStateTest.Data;
using ActorStateTest.Element;
using UnityEngine.Assertions;
using YeActorState.RuntimeCore;
using YeUtility;
using Zenject;

namespace ActorStateTest.Systems
{
    public class ActorMgr
    {
        [Inject] private ActorDataRepo actorDataRepo;

        private readonly List<ActorHandler> actorHandlers = new();
        [Inject] private IAddActorReceiver[] addActorReceivers;
        [Inject] private DiContainer container;
        [Inject] private YeActorStateSys yeActorStateSys;

        public IEnumerable<ActorHandler> ActorHandlers => actorHandlers;

        public ActorHandler CreatePlayer(string name)
        {
            yeActorStateSys.NeedDefaultProcessor();
            
            var actorData = actorDataRepo.GetData(name);
            Assert.IsNotNull(actorData, $"沒找到角色 {name}");

            var yeActorHandler = yeActorStateSys.AddActor(actorData.yeActorBaseData);

            ActorHandler rv = new();
            var player = container.InstantiatePrefabForComponent<Player>(actorData.modelPrefab);

            var perimeter = new List<object>
            {
                yeActorHandler,
                player.gameObject,
                player
            };
            container.Inject(rv, perimeter);
            rv.Initialize();
            actorHandlers.Add(rv);
            addActorReceivers.ForEach((_, h) => h.AddRuntimeData(yeActorHandler));
            return rv;
        }
    }
}
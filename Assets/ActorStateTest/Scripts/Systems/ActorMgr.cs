using ActorStateTest.Data;
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
            var gameObject = container.InstantiatePrefab(actorData.modelPrefab);
            
            ActorHandler rv = new(yeActorHandler, gameObject);
            return rv;
        }
    }
}
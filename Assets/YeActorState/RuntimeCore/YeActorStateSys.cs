using System.Collections.Generic;
using Zenject;

namespace YeActorState.RuntimeCore
{
    public class YeActorStateSys
    {
        [InjectOptional] private YeActorBaseDataRepo actorBaseDataRepo;
        [Inject] private DiContainer container;
        private List<ActorStateHandler> handlers = new();
        private DefaultPropertyProcessor defaultPropertyProcessor = new();

        public IEnumerable<ActorStateHandler> AllHandlers => handlers;
        
        public ActorStateHandler AddActor(string name)
        {
            foreach (var baseData in actorBaseDataRepo.Datas)
            {
                if (baseData.name == name)
                    return AddActor(baseData);
            }

            return null;
        }

        public ActorStateHandler AddActor(YeActorBaseData baseData)
        {
            var rv = new ActorStateHandler();
            var runtime = new YeActorRuntimeData();
            runtime.Setup(baseData);
            defaultPropertyProcessor.Processor(baseData, runtime);
            var perimeter = new List<object> { runtime, baseData };
            container.Inject(rv, perimeter);
            handlers.Add(rv);
            return rv;
        }

    }
}
using Zenject;

namespace YeActorState
{
    public class YeActorStateSys
    {
        [InjectOptional] private YeActorBaseDataRepo actorBaseDataRepo;
        [Inject] private DiContainer container;

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
            var rv = container.Instantiate<ActorStateHandler>();
            container.Inject(rv, new[] { baseData });
            return rv;
        }
    }
}
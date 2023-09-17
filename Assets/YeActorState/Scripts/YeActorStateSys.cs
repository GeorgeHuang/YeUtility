    using Zenject;

    namespace YeActorState
    {
        public class YeActorStateSys
        {
            [Inject] private YeActorBaseDataRepo actorBaseDataRepo;

            public int AddActor(string name)
            {
                foreach (var baseData in actorBaseDataRepo.Datas)
                {
                    if (baseData.name == name)
                        return AddActor(baseData);
                }

                return -1;
            }
            
            public int AddActor(YeActorBaseData baseData)
            {
                return baseData ? 1 : -1;
            }
        }
    }

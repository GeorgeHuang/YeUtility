using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace YeActorState.RuntimeCore
{
    public class YeActorStateSys : ITickable, IInitializable
    {
        [InjectOptional] private YeActorBaseDataRepo actorBaseDataRepo;
        [Inject] private DiContainer container;
        [Inject] private List<ISkillChangeReceiver> skillChangeReceivers;

        private List<ActorStateHandler> handlers = new();
        private DefaultPropertyProcessor defaultPropertyProcessor = new();
        private Dictionary<ActorStateHandler, List<PropertyEffectData>> actorEffectList = new();
        private Dictionary<ActorStateHandler, List<RuntimeSkill>> actorSkillList = new();

        private LinkedList<ActorStateHandler> dirtyActorList = new();
        private LinkedList<RuntimeSkill> dirtySkillList = new();
        private List<IBasePropertyProcessor> runtimeProcessors = new();

        public IEnumerable<ActorStateHandler> AllHandlers => handlers;

        public void Initialize()
        {
            runtimeProcessors.Add(new DefaultRuntimePropertyProcessor());
        }

        public ActorStateHandler AddActor(string name)
        {
            return (from baseData in actorBaseDataRepo.Datas where baseData.name == name select AddActor(baseData))
                .FirstOrDefault();
        }

        public ActorStateHandler AddActor(YeActorBaseData baseData)
        {
            var rv = new ActorStateHandler();
            actorEffectList.Add(rv, new List<PropertyEffectData>());
            actorSkillList.Add(rv, new List<RuntimeSkill>());
            var runtime = new YeActorRuntimeData();
            runtime.Setup(baseData);
            defaultPropertyProcessor.Processor(baseData, runtime);
            runtimeProcessors.ForEach(x => x.Processor(baseData, runtime));
            var perimeter = new List<object> { runtime, baseData, this };
            container.Inject(rv, perimeter);
            handlers.Add(rv);
            return rv;
        }

        public void Tick()
        {
            for (var data = dirtyActorList.First; data != null; data = data.Next)
            {
                Calculate(data.Value);
                data.Value.IsDirty = false;
            }

            dirtyActorList.Clear();

            for (var data = dirtySkillList.First; data != null; data = data.Next)
            {
                data.Value.Calculate();
                data.Value.IsDirty = false;
            }

            dirtySkillList.Clear();
        }

        private void Calculate(ActorStateHandler actorStateHandler)
        {
            var runtime = new YeActorRuntimeData();
            var baseData = actorStateHandler.ActorBaseData;
            var oldRuntime = actorStateHandler.RuntimeData;
            runtime.Setup(baseData);

            runtime.SetProperty("CurHp", oldRuntime.GetProperty("CurHp"));
            runtime.SetProperty("CurMp", oldRuntime.GetProperty("CurMp"));

            var effectList = actorEffectList[actorStateHandler];
            foreach (var data in effectList)
            {
                data.Processor(baseData: baseData, runtimeData: runtime);
            }

            runtimeProcessors.ForEach(x => x.Processor(baseData, runtime));

            actorStateHandler.RuntimeData = runtime;
        }

        public List<PropertyEffectData> GetCurrentEffectList(ActorStateHandler actorStateHandler)
        {
            return actorEffectList[actorStateHandler];
        }

        public void DeleteEffect(PropertyEffectData propertyEffectData, ActorStateHandler actorStateHandler)
        {
            actorStateHandler.IsDirty = true;
            actorEffectList[actorStateHandler].Remove(propertyEffectData);
            dirtyActorList.AddLast(actorStateHandler);
        }

        public void ApplyEffect(PropertyEffectData propertyEffectData, ActorStateHandler actorStateHandler)
        {
            actorStateHandler.IsDirty = true;
            actorEffectList[actorStateHandler].Add(propertyEffectData);
            dirtyActorList.AddLast(actorStateHandler);
            actorSkillList[actorStateHandler].ForEach(SetSkillDirty);
        }

        public void AddSkill(SkillObject skillObject, ActorStateHandler actorStateHandler)
        {
            var skillList = actorSkillList[actorStateHandler];
            if (skillList.Any(x => x.Compare(skillObject)))
            {
                return;
            }

            var newRuntimeSkill = new RuntimeSkill(skillObject, actorStateHandler);
            actorSkillList[actorStateHandler].Add(newRuntimeSkill);
            SetSkillDirty(newRuntimeSkill);
            skillChangeReceivers.ForEach(receiver => receiver.SkillChanged(actorStateHandler, skillObject));
        }

        private void SetSkillDirty(RuntimeSkill newRuntimeSkill)
        {
            if (newRuntimeSkill.IsDirty) return;
            newRuntimeSkill.IsDirty = true;
            dirtySkillList.AddLast(newRuntimeSkill);
        }

        public List<RuntimeSkill> GetRuntimeList(ActorStateHandler actorStateHandler)
        {
            return actorSkillList[actorStateHandler];
        }
    }
}
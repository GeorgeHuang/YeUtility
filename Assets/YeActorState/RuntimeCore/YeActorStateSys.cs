using System.Collections.Generic;
using System.Linq;
using YeUtility;
using Zenject;

namespace YeActorState.RuntimeCore
{
    public class YeActorStateSys : ITickable, IInitializable
    {
        private readonly Dictionary<ActorStateHandler, List<PropertyEffectData>> actorEffectList = new();
        private readonly Dictionary<ActorStateHandler, List<RuntimeSkill>> actorSkillList = new();
        private static readonly DefaultPropertyProcessor defaultPropertyProcessor = new();
        private readonly DefaultPropertyProcessor[] defaultPropertyProcessors = new[] { defaultPropertyProcessor };

        private readonly LinkedList<ActorStateHandler> dirtyActorList = new();
        private readonly LinkedList<RuntimeSkill> dirtySkillList = new();

        private readonly List<ActorStateHandler> handlers = new();
        private readonly List<IBasePropertyProcessor> runtimeProcessors = new();

        [InjectOptional] private YeActorBaseDataRepo actorBaseDataRepo;
        [Inject] private DiContainer container;
        [Inject] private List<IDealDamage> dealDamageReceiver;
        [Inject] private List<ISkillChangeReceiver> skillChangeReceivers;
        private bool needDefault = false;

        public IEnumerable<ActorStateHandler> AllHandlers => handlers;

        public void Initialize()
        {
        }

        public void NeedDefaultProcessor()
        {
            if (needDefault) return;
            runtimeProcessors.Add(new DefaultRuntimePropertyProcessor());
            needDefault = true;
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

        public ActorStateHandler AddActor(string name)
        {
            return (from baseData in actorBaseDataRepo.Datas where baseData.name == name select AddActor(baseData))
                .FirstOrDefault();
        }

        public void AddRuntimeProcessor(IBasePropertyProcessor processor)
        {
            if (runtimeProcessors.Contains(processor) == true) return;
            runtimeProcessors.Add(processor);
        }

        public ActorStateHandler AddActor(YeActorBaseData baseData)
        {
            var rv = new ActorStateHandler();
            actorEffectList.Add(rv, new List<PropertyEffectData>());
            actorSkillList.Add(rv, new List<RuntimeSkill>());
            var runtime = new YeActorRuntimeData();
            runtime.Setup(baseData);
            if (needDefault) defaultPropertyProcessor.Processor(baseData, runtime);
            runtimeProcessors.ForEach(x => x.Processor(baseData, runtime));
            var perimeter = new List<object> { runtime, baseData, this };
            container.Inject(rv, perimeter);
            handlers.Add(rv);
            return rv;
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
            foreach (var data in effectList) data.Processor(baseData, runtime);

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
            var runtimeSkill = skillList.FirstOrDefault(x => x.Compare(skillObject));
            if (runtimeSkill != null)
            {
                runtimeSkill.AddLv(1);
                return;
            }

            var newRuntimeSkill = new RuntimeSkill(skillObject, actorStateHandler);
            actorSkillList[actorStateHandler].Add(newRuntimeSkill);
            SetSkillDirty(newRuntimeSkill);
            skillChangeReceivers.ForEach(receiver => receiver.SkillChanged(actorStateHandler, skillObject));
        }

        public void SetSkillDirty(RuntimeSkill newRuntimeSkill)
        {
            if (newRuntimeSkill.IsDirty) return;
            newRuntimeSkill.IsDirty = true;
            dirtySkillList.AddLast(newRuntimeSkill);
        }

        public List<RuntimeSkill> GetRuntimeList(ActorStateHandler actorStateHandler)
        {
            return actorSkillList[actorStateHandler];
        }

        public void Attack(ActorStateHandler owner, ActorStateHandler receiver, SkillObject skillObject)
        {
            var runtimeSkill = actorSkillList[owner].FirstOrDefault(x => x.Compare(skillObject));
            if (runtimeSkill == null) return;

            var damage = runtimeSkill.Damage;
            var criticalRate = owner.GetRuntimeProperty("CriticalRate");
            var criticalDamageRate = owner.GetRuntimeProperty("CriticalDamageRatio");
            var addDamage = 0f;
            if (Common.Random((int)criticalRate)) addDamage = damage * criticalDamageRate * 0.01f;

            damage += addDamage;

            if (dealDamageReceiver.Count > 0)
            {
                var eventData = new DealDamageEventData
                    { Owner = owner, Receiver = receiver, Damage = damage, IsCritical = addDamage > 0 };
                dealDamageReceiver.ForEach(x => x.DealDamageEvent(eventData));
            }

            receiver.DealDamage(damage);
        }

        public void SetActorDirty(ActorStateHandler actorStateHandler)
        {
            if (actorStateHandler.IsDirty) return;
            actorStateHandler.IsDirty = true;
            dirtyActorList.AddLast(actorStateHandler);
            actorSkillList[actorStateHandler].ForEach(SetSkillDirty);
        }

        public void SetSkillDirty(ActorStateHandler handler)
        {
            actorSkillList[handler].ForEach((i, x) =>
            {
                x.IsDirty = true;
                dirtySkillList.AddLast(x);
            });
        }

        public RuntimeSkill GetRuntimeSkill(ActorStateHandler actorStateHandler, SkillObject skillObject)
        {
            return actorSkillList[actorStateHandler].FirstOrDefault(x => x.Compare(skillObject));
        }
    }
}
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using UnityEngine;
using Zenject;

namespace YeActorState.RuntimeCore
{
    public class YeActorStateSys : ITickable
    {
        [InjectOptional] private YeActorBaseDataRepo actorBaseDataRepo;
        [Inject] private DiContainer container;
        private List<ActorStateHandler> handlers = new();
        private DefaultPropertyProcessor defaultPropertyProcessor = new();
        private Dictionary<ActorStateHandler, List<PropertyEffectData>> actorEffectList = new();

        public IEnumerable<ActorStateHandler> AllHandlers => handlers;

        public ActorStateHandler AddActor(string name)
        {
            return (from baseData in actorBaseDataRepo.Datas where baseData.name == name select AddActor(baseData))
                .FirstOrDefault();
        }

        public ActorStateHandler AddActor(YeActorBaseData baseData)
        {
            var rv = new ActorStateHandler();
            actorEffectList.Add(rv, new List<PropertyEffectData>());
            var runtime = new YeActorRuntimeData();
            runtime.Setup(baseData);
            defaultPropertyProcessor.Processor(baseData, runtime);
            var perimeter = new List<object> { runtime, baseData, this };
            container.Inject(rv, perimeter);
            handlers.Add(rv);
            return rv;
        }

        public void Tick()
        {
            foreach (var actorStateHandler in handlers.Where(actorStateHandler => actorStateHandler.IsDirty))
            {
                Calculate(actorStateHandler);
                actorStateHandler.IsDirty = false;
            }
        }

        private void Calculate(ActorStateHandler actorStateHandler)
        {
            var runtimeData = new YeActorRuntimeData();
            runtimeData.Setup(actorStateHandler.ActorBaseData);
            var baseData = actorStateHandler.ActorBaseData;
            defaultPropertyProcessor.Processor(baseData, runtimeData);
            var effectList = actorEffectList[actorStateHandler];
            foreach (var data in effectList)
            {
                data.Processor(baseData: baseData, runtimeData: runtimeData);
            }

            //可能要給處理器
            var moveSpeed = runtimeData.GetProperty("MoveSpeed");
            var moveSpeedRatio = runtimeData.GetProperty("MoveSpeedRatio");
            moveSpeed = moveSpeed * (1 + moveSpeedRatio * 0.01f);
            runtimeData.SetProperty("MoveSpeed", moveSpeed);
            actorStateHandler.RuntimeData = runtimeData;
        }

        public List<PropertyEffectData> GetCurrentEffectList(ActorStateHandler actorStateHandler)
        {
            return actorEffectList[actorStateHandler];
        }

        public void DeleteEffect(PropertyEffectData propertyEffectData, ActorStateHandler actorStateHandler)
        {
            actorStateHandler.IsDirty = true;
            actorEffectList[actorStateHandler].Remove(propertyEffectData);
        }

        public void ApplyEffect(PropertyEffectData propertyEffectData, ActorStateHandler actorStateHandler)
        {
            actorStateHandler.IsDirty = true;
            actorEffectList[actorStateHandler].Add(propertyEffectData);
        }
    }
}
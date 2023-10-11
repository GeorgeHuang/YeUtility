using YeActorState;
using YeActorState.RuntimeCore;
using Zenject;

namespace ActorStateTest.Systems
{
    public class SkillChangeHandler : ISkillChangeReceiver
    {
        [Inject] private ActorMgr actorMgr;

        //為了Demo的反call，一般來說是Domain叫ActorStateSys裝技能，不會這樣call出來
        public void SkillChanged(ActorStateHandler actorStateHandler, SkillObject skillObject)
        {
            foreach (var actorHandler in actorMgr.ActorHandlers)
                if (actorHandler.Compare(actorStateHandler))
                    actorHandler.AddSkill(skillObject);
        }
    }
}
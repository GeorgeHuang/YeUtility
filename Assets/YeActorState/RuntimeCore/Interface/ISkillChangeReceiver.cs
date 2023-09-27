namespace YeActorState.RuntimeCore
{
    public interface ISkillChangeReceiver
    {
        void SkillChanged(ActorStateHandler actorStateHandler, SkillObject skillObject);
    }
}
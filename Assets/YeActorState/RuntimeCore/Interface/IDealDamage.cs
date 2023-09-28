namespace YeActorState.RuntimeCore
{
    public interface IDealDamage
    {
        void DealDamageEvent(DealDamageEventData data);
    }

    public class DealDamageEventData
    {
        public ActorStateHandler Owner;
        public ActorStateHandler Receiver;
        public float Damage;
        public bool IsCritical;
    }
}
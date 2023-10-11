namespace YeActorState.RuntimeCore
{
    public interface IDealDamage
    {
        void DealDamageEvent(DealDamageEventData data);
    }

    public class DealDamageEventData
    {
        public float Damage;
        public bool IsCritical;
        public ActorStateHandler Owner;
        public ActorStateHandler Receiver;
    }
}
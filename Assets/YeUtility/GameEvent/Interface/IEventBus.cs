using System;

namespace GameEvent.Interface
{
    public interface IEventBus
    {
        void SendEvent(object args);
        void Register<T>(Action<T> callBase, bool highPriority = false);
        void Unregister<T>(Action<T> onWeaponBtnPress);
    }
}
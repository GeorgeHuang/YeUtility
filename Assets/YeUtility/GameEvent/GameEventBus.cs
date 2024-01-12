using System;
using System.Collections.Generic;
using GameEvent.Interface;

namespace GameEvent
{
    public class GameEventBus : IEventBus
    {
        readonly Dictionary<Type, List<Action<object>>> callBacks = new();
        readonly Dictionary<int, Action<object>> funMap = new();

        public void Register<T>(Action<T> callBack, bool highPriority = false)
        {
            var type = typeof(T);
            var hasKey = callBacks.ContainsKey(type);

            if (funMap.ContainsKey(callBack.GetHashCode()))
                return;

            funMap.Add(callBack.GetHashCode(), o => callBack((T)o));
                
            if (hasKey)
            {
                var value = callBacks[type];
                var fun = funMap[callBack.GetHashCode()];
                if (highPriority)
                    value.Insert(0, fun);
                else
                    value.Add(fun);
            }
            else
            {
                var value = new List<Action<object>>
                {
                    funMap[callBack.GetHashCode()]
                };
                callBacks[type] = value;
            }
        }

        public void SendEvent(object args)
        {
            var type = args.GetType();
            var hasKey = callBacks.ContainsKey(type);
            if (hasKey)
            {
                var value = callBacks[type];
                value.ForEach(x => x(args));
            }
        }
        public void SendEvent<T>(T args)
        {
            var type = typeof(T);
            var hasKey = callBacks.ContainsKey(type);
            if (hasKey)
            {
                var value = callBacks[type];
                value.ForEach(x => x(args));
            }
        }

        public void Unregister<T>(Action<T> callBack)
        {
            var type = typeof(T);
            var hasKey = callBacks.ContainsKey(type);
            if (hasKey)
            {
                var value = callBacks[type];
                value.Remove(funMap[callBack.GetHashCode()]);
                funMap.Remove(callBack.GetHashCode());
            }
        }
    }
}

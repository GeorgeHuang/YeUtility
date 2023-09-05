using UnityEngine;

namespace YeUtility
{
    public interface IReusePool
    {
        public Object Get(Object prefab);
        public T Get<T>(T prefab) where T : Object;
        public void Back(Object obj);
    }
}

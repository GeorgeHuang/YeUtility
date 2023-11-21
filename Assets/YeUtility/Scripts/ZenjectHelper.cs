using UnityEngine;
using Zenject;

namespace YeUtility
{
    public class ZenjectHelper
    {
        public static void BindSingleMgr<T>(DiContainer Container, T obj)
            where T : Object
        {
            Container.Bind<T>()
                .FromComponentInNewPrefab(obj)
                .AsSingle()
                .NonLazy();
        }

        public static void BindInterfaceToSingleMgr<T>(DiContainer Container, T obj)
            where T : Object
        {
            Container.BindInterfacesAndSelfTo<T>()
                .FromComponentInNewPrefab(obj)
                .AsSingle()
                .NonLazy();
        }

        public static void BindSingleSO<T>(DiContainer Container, ScriptableObject obj)
        {
            Container.Bind<T>()
                .FromScriptableObject(obj)
                .AsSingle();
        }

        public static void BindSingleSO<T>(DiContainer Container, T obj)
            where T : ScriptableObject
        {
            Container.Bind<T>()
                .FromScriptableObject(obj)
                .AsSingle();
        }

        public static void BindInterfaceToSingleSO<T>(DiContainer Container, T obj)
            where T : ScriptableObject
        {
            Container.BindInterfacesAndSelfTo<T>()
                .FromScriptableObject(obj)
                .AsSingle();
        }
    }
}
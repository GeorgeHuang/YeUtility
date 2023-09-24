using ActorStateTest.Element;
using UnityEngine;
using YeActorState.UI;
using Zenject;

namespace ActorStateTest.Systems
{
    public class ShootingRageBind : MonoInstaller
    {
        [SerializeField] private YeActorStateDashboard dashboard;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputState>().AsSingle();
            Container.BindInterfacesAndSelfTo<TimeSys>().AsSingle();
            Container.BindInterfacesAndSelfTo<ShootingRageSys>().AsSingle();
            Container.BindInterfacesAndSelfTo<ActorMgr>().AsSingle();
            Container.BindInterfacesAndSelfTo<YeActorStateDashboard>().FromComponentInNewPrefab(dashboard).AsSingle().NonLazy();
        }
    }
}
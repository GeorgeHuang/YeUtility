using UnityEngine;
using Zenject;

namespace YeActorState
{
    [CreateAssetMenu(fileName = "ActorStateSOInstaller", menuName = "Tools/YeActorState", order = 0)]
    public class ActorStateSOInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private YeActorBaseDataRepo actorBaseDataRepo;

        public override void InstallBindings()
        {
            Container.BindInstance(actorBaseDataRepo).AsSingle();
        }
    }
}
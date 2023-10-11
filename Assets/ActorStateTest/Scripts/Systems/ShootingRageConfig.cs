using ActorStateTest.Data;
using Sirenix.OdinInspector;
using UnityEngine;
using YeActorState;
using Zenject;

namespace ActorStateTest.Systems
{
    public class ShootingRageConfig : ScriptableObjectInstaller
    {
        [SerializeField] private YeActorBaseDataRepo actorBaseDataRepo;
        [SerializeField] private ActorDataRepo actorDataRepo;
        [SerializeField] private SkillDataRepo skillDataRepo;

        [ValueDropdown("@EditorHelper.ActorNames")] [SerializeField]
        public string PlayerDataName;

        [ValueDropdown("@EditorHelper.ActorNames")] [SerializeField]
        public string EnemyDataName;

        [SerializeField] public int MaxEnemyNumber;

        public override void InstallBindings()
        {
            Container.BindInstance(this).AsSingle();
            Container.BindInstance(actorBaseDataRepo).AsSingle();
            Container.BindInstance(actorDataRepo).AsSingle();
            Container.BindInstance(skillDataRepo).AsSingle();
        }
    }
}
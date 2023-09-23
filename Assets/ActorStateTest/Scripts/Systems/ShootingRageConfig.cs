using System;
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
        
        [ValueDropdown("@EditorHelper.ActorNames")]
        [SerializeField] public string PlayerDataName;

        public override void InstallBindings()
        {
            Container.BindInstance(this).AsSingle();
            Container.BindInstance(actorBaseDataRepo).AsSingle();
            Container.BindInstance(actorDataRepo).AsSingle();
        }
    }
}
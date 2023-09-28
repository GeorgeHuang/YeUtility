using System;
using UnityEngine;
using YeActorState.RuntimeCore;
using Zenject;

namespace YeActorState.UI
{
    public class DashboardBind : MonoInstaller
    {
        [SerializeField] private TagDataRepo tagDataRepo;
        public override void InstallBindings()
        {
            var propertyNames = OdinUnit.OdinEditorHelpers.GetScriptableObject<PropertyNames>();
            if (Container.HasBinding<PropertyNames>() == false)
            {
                Container.BindInstance(propertyNames).AsSingle();
            }

            Container.BindInstance(tagDataRepo).AsSingle();
        }

    }
}
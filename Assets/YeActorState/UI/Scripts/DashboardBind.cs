using UnityEngine;
using YeActorState.RuntimeCore;
using YeUtility.EditorHelper;
using Zenject;

namespace YeActorState.UI
{
    public class DashboardBind : MonoInstaller
    {
        [SerializeField] private TagDataRepo tagDataRepo;

        public override void InstallBindings()
        {
            var propertyNames = OdinEditorHelpers.GetScriptableObject<PropertyNames>();
            if (Container.HasBinding<PropertyNames>() == false) Container.BindInstance(propertyNames).AsSingle();

            Container.BindInstance(tagDataRepo).AsSingle();
        }
    }
}
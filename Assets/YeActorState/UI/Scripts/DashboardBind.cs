using Zenject;

namespace YeActorState.UI
{
    public class DashboardBind : MonoInstaller
    {
        public override void InstallBindings()
        {
            var propertyNames = OdinUnit.OdinEditorHelpers.GetScriptableObject<PropertyNames>();
            if (Container.HasBinding<PropertyNames>() == false)
            {
                Container.BindInstance(propertyNames).AsSingle();
            }
        }

    }
}
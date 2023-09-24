using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using YeActorState.RuntimeCore;
using Zenject;

namespace YeActorState.UI
{
    public class PropertyEffectElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI labelText;
        [SerializeField] private Button btn;

        [Inject] private YeActorStateDashboard dashboard;

        private PropertyEffectData propertyEffectData;
        private ActorStateHandler stateHandler;
        private IDisposable disposable;

        public void Setup(PropertyEffectData propertyEffectData, ActorStateHandler handler)
        {
            stateHandler = handler;
            this.propertyEffectData = propertyEffectData;
            labelText.text = propertyEffectData.GetDisplayName();
            disposable = btn.OnClickAsObservable().Subscribe(OnBtnPress);
        }

        private void OnDestroy()
        {
            disposable.Dispose();
        }

        void OnBtnPress(Unit u)
        {
            stateHandler.ApplyEffect(propertyEffectData);
            dashboard.RefreshPropertyView();
        }
    }
}
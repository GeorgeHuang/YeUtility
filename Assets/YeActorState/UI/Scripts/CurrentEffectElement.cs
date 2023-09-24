using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using YeActorState.RuntimeCore;
using Zenject;

namespace YeActorState.UI
{
    public class CurrentEffectElement : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI labelText;
        [SerializeField] private Button deleteButton;

        [Inject] private YeActorStateDashboard dashboard;

        private PropertyEffectData propertyEffectData;
        private ActorStateHandler stateHandler;
        private IDisposable disposable;

        public void Setup(PropertyEffectData propertyEffectData, ActorStateHandler handler, Color color)
        {
            stateHandler = handler;
            this.propertyEffectData = propertyEffectData;
            labelText.text = propertyEffectData.GetDisplayName();
            disposable = deleteButton.OnClickAsObservable().Subscribe(OnBtnPress);
            image.color = color;
        }

        private void OnDestroy()
        {
            disposable.Dispose();
        }

        void OnBtnPress(Unit u)
        {
            stateHandler.DeleteEffect(propertyEffectData);
            dashboard.RefreshPropertyView();
        }
    }
}
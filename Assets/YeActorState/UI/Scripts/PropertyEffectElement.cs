using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
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
        [Inject(Id = "Message")] private TextMeshProUGUI messageGUI;

        private PropertyEffectData propertyEffectData;
        private ActorStateHandler stateHandler;

        public void Setup(PropertyEffectData propertyEffectData, ActorStateHandler handler)
        {
            stateHandler = handler;
            this.propertyEffectData = propertyEffectData;
            labelText.text = propertyEffectData.GetDisplayName();
            btn.OnClickAsObservable().Subscribe(OnBtnPress).AddTo(gameObject);
            btn.OnPointerEnterAsObservable().Subscribe(OnPointEnter).AddTo(gameObject);
        }

        private void OnPointEnter(PointerEventData pointerEventData)
        {
            var text = "";
            propertyEffectData.Datas.ForEach((i, data) => text += $"{data.targetPropertyName} + {data.value}<br>");
            messageGUI.text = text;
        }

        private void OnBtnPress(Unit u)
        {
            stateHandler.ApplyEffect(propertyEffectData);
            dashboard.RefreshPropertyView();
        }
    }
}
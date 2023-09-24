using System;
using System.Globalization;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using Image = UnityEngine.UI.Image;

namespace YeActorState.UI
{
    public class PropertyElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI propertyNameText;
        [SerializeField] private Image image;
        [SerializeField] private TMP_InputField value;

        [Inject] private PropertyNames propertyNames;

        private ActorStateHandler stateHandler;
        private string propertyName;
        private IDisposable disposable;

        public void Setup(string propertyName, ActorStateHandler stateHandler, Color color)
        {
            this.stateHandler = stateHandler;
            this.propertyName = propertyName;
            var displayName = propertyNames.GetDisplayName(propertyName);
            propertyNameText.text = displayName;
            image.color = color;
            value.text = stateHandler.GetRuntimeProperty(this.propertyName).ToString();
            disposable = value.onValueChanged.AsObservable().Subscribe(_ => ValueChanged(_));
        }

        private void ValueChanged(string s)
        {
            if (float.TryParse(s, out var v))
            {
                stateHandler.SetProperty(propertyName, v);
            }
            else
            {
                value.text = stateHandler.GetRuntimeProperty(this.propertyName).ToString();
            }
        }

        private void OnDestroy()
        {
           disposable.Dispose();
        }

        public void Refresh()
        {
            value.text = stateHandler.GetRuntimeProperty(this.propertyName).ToString();
        }
    }
}
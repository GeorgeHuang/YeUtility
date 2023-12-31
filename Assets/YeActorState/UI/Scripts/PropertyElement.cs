﻿using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace YeActorState.UI
{
    public class PropertyElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI propertyNameText;
        [SerializeField] private Image image;
        [SerializeField] private TMP_InputField value;
        private IDisposable disposable;
        private string propertyName;

        [Inject] private PropertyNames propertyNames;

        private ActorStateHandler stateHandler;

        private void OnDestroy()
        {
            disposable.Dispose();
        }

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
                stateHandler.SetSkillDirty();
                //stateHandler.SetActorDirty();
            }
            else
            {
                value.text = stateHandler.GetRuntimeProperty(propertyName).ToString();
            }
        }

        public void Refresh()
        {
            value.text = stateHandler.GetRuntimeProperty(propertyName).ToString();
        }
    }
}
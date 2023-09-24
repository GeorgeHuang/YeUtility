using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace YeActorState.UI
{
    public class YeActorStateDashboard : MonoBehaviour
    {
        [SerializeField] private PropertyElement propertyElementPrefab;

        [Inject(Id = "RuntimeListDropdown")] private TMP_Dropdown runtimeListDropdown;
        [Inject(Id = "PropertyContent")] private RectTransform propertyContentTrans;
        [Inject] private YeActorStateSys yeActorStateSys;

        private List<string> runtimeDropdownContent;
        private List<ActorStateHandler> actorStateHandlers;

        private void Start()
        {
            UpdateRuntimeDataDropdown();
            SetupPropertyContent(actorStateHandlers.First());
        }

        private void UpdateRuntimeDataDropdown()
        {
            var handlers = yeActorStateSys.AllHandlers;
            actorStateHandlers = handlers.ToList();
            runtimeDropdownContent = actorStateHandlers
                .Select(handler => handler.GetHashCode() + "_" + handler.GetBaseDataName())
                .ToList();
            runtimeListDropdown.AddOptions(runtimeDropdownContent);
            runtimeListDropdown.onValueChanged.AsObservable().Subscribe(RuntimeListDropdownValueChanged);
        }

        private void RuntimeListDropdownValueChanged(int i)
        {
            SetupPropertyContent(actorStateHandlers[i]);
        }

        private void SetupPropertyContent(ActorStateHandler actorStateHandler)
        {
            foreach (Transform c in propertyContentTrans.gameObject.transform)
            {
                Destroy(c.gameObject);
            }

            if (actorStateHandlers == null) return;

            foreach (DictionaryEntry runtimeProperty in actorStateHandler.AllRuntimeProperties)
            {
                var propertyElement = Instantiate(propertyElementPrefab, propertyContentTrans, true);
                propertyElement.Setup(runtimeProperty.Key as string, actorStateHandler);
            }
            //propertyScrollView.
        }
    }
}
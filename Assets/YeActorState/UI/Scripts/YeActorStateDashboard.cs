using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace YeActorState.UI
{
    public class YeActorStateDashboard : MonoBehaviour
    {
        [Inject(Id = "RuntimeListDropdown")] private TMP_Dropdown runtimeListDropdown;

        [Inject] private YeActorStateSys yeActorStateSys;
        private List<string> runtimeDropdownContent;

        private void Start()
        {
            UpdateRuntimeDataDropdown();
        }

        private void UpdateRuntimeDataDropdown()
        {
            var handlers = yeActorStateSys.AllHandlers;
            runtimeDropdownContent = handlers.Select(handler => handler.GetHashCode() + "_" + handler.GetBaseDataName())
                .ToList();
            runtimeListDropdown.AddOptions(runtimeDropdownContent);
            runtimeListDropdown.onValueChanged.AsObservable().Subscribe(RuntimeListDropdownValueChanged);
        }

        private void RuntimeListDropdownValueChanged(int i)
        {
            Debug.Log($"{i}");
        }
    }
}
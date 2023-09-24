﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using YeActorState.RuntimeCore;
using Zenject;

namespace YeActorState.UI
{
    public class YeActorStateDashboard : MonoInstaller
    {
        [SerializeField] private PropertyElement propertyElementPrefab;
        [SerializeField] private List<Color> elementColos;

        [Inject(Id = "RuntimeListDropdown")] private TMP_Dropdown runtimeListDropdown;
        [Inject(Id = "PropertyContent")] private RectTransform propertyContentTrans;
        [Inject] private YeActorStateSys yeActorStateSys;


        private List<string> runtimeDropdownContent;
        private List<ActorStateHandler> actorStateHandlers;
        private PropertyNames propertyNames;

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

            var runtimes = SortRuntimeProperties(actorStateHandler);

            var index = 0;
            foreach (var propertyName in runtimes)
            {
                var propertyElement = Container.InstantiatePrefabForComponent<PropertyElement>(propertyElementPrefab);
                propertyElement.transform.parent = propertyContentTrans;
                propertyElement.Setup(propertyName, actorStateHandler,
                    elementColos[index % elementColos.Count]);
                index++;
            }
        }

        //將 runtimeProperty 用總表順序排序
        private List<string> SortRuntimeProperties(ActorStateHandler actorStateHandler)
        {
            var names = new List<string>();
            var runtimes = (from DictionaryEntry runtimeProperty in actorStateHandler.AllRuntimeProperties
                select runtimeProperty.Key as string).ToList();

            foreach (var propertyNamesData in propertyNames.Datas)
            {
                var runtimePropertyKey = propertyNamesData.GetKeyName();
                var runtimeIndex = runtimes.FindIndex(x => x == runtimePropertyKey);
                if (runtimeIndex < 0) continue;
                names.Add(runtimePropertyKey);
                var nameIndex = names.Count - 1;
                if (runtimeIndex == nameIndex) continue;
                runtimes.Swap(runtimeIndex, nameIndex);
            }

            return runtimes;
        }

        public override void InstallBindings()
        {
            propertyNames = OdinUnit.OdinEditorHelpers.GetScriptableObject<PropertyNames>();
            if (Container.HasBinding<PropertyNames>() == false)
            {
                Container.BindInstance(propertyNames).AsSingle();
            }
        }
    }
}
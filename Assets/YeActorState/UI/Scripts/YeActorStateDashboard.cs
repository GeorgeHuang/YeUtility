using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OdinUnit;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using YeActorState.RuntimeCore;
using Zenject;

namespace YeActorState.UI
{
    public class YeActorStateDashboard : MonoBehaviour
    {
        [SerializeField] private PropertyElement propertyElementPrefab;
        [SerializeField] private PropertyEffectElement propertyEffectElementPrefab;
        [SerializeField] private CurrentEffectElement currentEffectElementPrefab;
        [SerializeField] private SkillObjectElement skillObjectElementPrefab;
        [SerializeField] private List<Color> elementColos;

        [Inject(Id = "RuntimeListDropdown")] private TMP_Dropdown runtimeListDropdown;
        [Inject(Id = "PropertyContent")] private RectTransform propertyContentTrans;

        [Inject(Id = "CurrentEffectViewContent")]
        private RectTransform CurrentEffectViewContentTrans;

        [Inject(Id = "DatabaseEffectViewContent")]
        private RectTransform DatabaseEffectViewContentTrans;
        
        [Inject(Id = "SkillObjectViewContent")]
        private RectTransform SkillObjectViewContentTrans;
        
        [Inject(Id = "RefreshBtn")] private Button refreshBtn;
        [Inject(Id = "Message")] private TextMeshProUGUI messageGUI;

        [Inject] private YeActorStateSys yeActorStateSys;
        [Inject] private PropertyNames propertyNames;
        [Inject] private DiContainer Container;

        private List<string> runtimeDropdownContent;
        private List<ActorStateHandler> actorStateHandlers;
        private ActorStateHandler curActorStateHandler;

        private async void Start()
        {
            await UniTask.WaitUntil(() => yeActorStateSys.AllHandlers.ToList().Count > 0);
            UpdateRuntimeDataDropdown();
            SetupPropertyContent(actorStateHandlers.First());
            SetupDatabaseEffect();
            SetupSkillContent();
            refreshBtn.OnClickAsObservable().Subscribe(RefreshBtnPress);
        }

        private void RefreshBtnPress(Unit unit)
        {
            SetupPropertyContent(curActorStateHandler);
            SetupApplyEffectContent(curActorStateHandler);
        }

        private void SetupApplyEffectContent(ActorStateHandler actorStateHandler)
        {
            ClearViewContent(CurrentEffectViewContentTrans);
            var effects = actorStateHandler.GetCurrentEffectList();
            var index = 0;
            foreach (var propertyEffectData in effects)
            {
                var effectElement =
                    Container.InstantiatePrefabForComponent<CurrentEffectElement>(currentEffectElementPrefab);
                effectElement.transform.SetParent(CurrentEffectViewContentTrans);
                effectElement.Setup(propertyEffectData, curActorStateHandler, elementColos[index % elementColos.Count]);
                index++;
            }
        }

        private void SetupSkillContent()
        {
            ClearViewContent(SkillObjectViewContentTrans);
            var repo = OdinEditorHelpers.GetScriptableObject<SkillObjectRepo>();
            foreach (var skillObject in repo.Datas)
            {
                var element = Container.InstantiatePrefabForComponent<SkillObjectElement>(skillObjectElementPrefab);
                element.transform.SetParent(SkillObjectViewContentTrans);
                element.Setup(skillObject);
                element.Btn.OnPointerEnterAsObservable().Subscribe(_ => OnSkillElementEnter(skillObject));
                element.Btn.OnPointerExitAsObservable().Subscribe(_ => OnSkillElementExit());
                element.Btn.OnClickAsObservable().Subscribe(_ => OnSkillClick(skillObject));
            }
        }

        private void OnSkillClick(SkillObject skillObject)
        {
            
        }

        private void OnSkillElementExit()
        {
            messageGUI.text = "";
        }

        private void OnSkillElementEnter(SkillObject skillObject)
        {
            var tagString = string.Join(",", skillObject.tagEffectList.Select(e => e.tagName).ToList());
            var customString = string.Join(",", skillObject.customEffects.Select(_ => _.propertyName).ToList());
            messageGUI.text =
                $"{skillObject.GetDisplayName()}<br>Tags:{tagString}<br>Customs:{customString}";
        }

        private void SetupDatabaseEffect()
        {
            ClearViewContent(DatabaseEffectViewContentTrans);

            var repo = OdinEditorHelpers.GetScriptableObject<PropertyEffectRepo>();

            foreach (var propertyEffectData in repo.Datas)
            {
                var effectElement =
                    Container.InstantiatePrefabForComponent<PropertyEffectElement>(propertyEffectElementPrefab);
                effectElement.transform.SetParent(DatabaseEffectViewContentTrans);
                effectElement.Setup(propertyEffectData, curActorStateHandler);
            }
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

        private async void SetupPropertyContent(ActorStateHandler actorStateHandler)
        {
            curActorStateHandler = actorStateHandler;

            await UniTask.WaitUntil(() => actorStateHandler.IsDirty == false);

            ClearViewContent(propertyContentTrans);

            if (actorStateHandlers == null) return;

            var runtimes = SortRuntimeProperties(actorStateHandler);

            var index = 0;
            foreach (var propertyName in runtimes)
            {
                var propertyElement = Container.InstantiatePrefabForComponent<PropertyElement>(propertyElementPrefab);
                propertyElement.transform.SetParent(propertyContentTrans);
                propertyElement.Setup(propertyName, actorStateHandler,
                    elementColos[index % elementColos.Count]);
                index++;
            }
        }

        private static void ClearViewContent(RectTransform trans)
        {
            foreach (Transform c in trans.gameObject.transform)
            {
                Destroy(c.gameObject);
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

        public void RefreshPropertyView()
        {
            // foreach (Transform c in propertyContentTrans.transform)
            // {
            //     var element = c.GetComponent<PropertyElement>();
            //     element.Refresh();
            // }

            //有點那個
            RefreshBtnPress(Unit.Default);
        }
    }
}
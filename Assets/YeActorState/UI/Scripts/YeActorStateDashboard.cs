using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using YeActorState.RuntimeCore;
using YeUtility;
using YeUtility.EditorHelper;
using Zenject;

namespace YeActorState.UI
{
    public class YeActorStateDashboard : MonoBehaviour, IAddActorReceiver, IInitializable
    {
        [SerializeField] private PropertyElement propertyElementPrefab;
        [SerializeField] private PropertyEffectElement propertyEffectElementPrefab;
        [SerializeField] private CurrentEffectElement currentEffectElementPrefab;
        [SerializeField] private SkillObjectElement skillObjectElementPrefab;
        [SerializeField] private RuntimeSkillElement runtimeSkillElementPrefab;
        [SerializeField] private List<Color> elementColos;
        private List<ActorStateHandler> actorStateHandlers;
        [Inject] private DiContainer Container;
        private ActorStateHandler curActorStateHandler;

        [Inject(Id = "CurrentEffectViewContent")]
        private RectTransform CurrentEffectViewContentTrans;

        [Inject(Id = "CurrentSkillObjectViewContent")]
        private RectTransform CurrentSkillObjectViewContentTrans;

        [Inject(Id = "DatabaseEffectViewContent")]
        private RectTransform DatabaseEffectViewContentTrans;

        [Inject(Id = "Message")] private TextMeshProUGUI messageGUI;
        [Inject(Id = "PropertyContent")] private RectTransform propertyContentTrans;
        [Inject] private PropertyNames propertyNames;

        [Inject(Id = "RefreshBtn")] private Button refreshBtn;

        private List<string> runtimeDropdownContent;

        [Inject(Id = "RuntimeListDropdown")] private TMP_Dropdown runtimeListDropdown;

        [Inject(Id = "SkillObjectViewContent")]
        private RectTransform SkillObjectViewContentTrans;

        [Inject] private TagDataRepo tagDataRepo;

        [Inject] private YeActorStateSys yeActorStateSys;

        public void Initialize()
        {
            InitASync().Forget();
        }

        private async UniTaskVoid InitASync()
        {
            await UniTask.WaitUntil(() => yeActorStateSys != null);
            await UniTask.WaitUntil(() => yeActorStateSys.AllHandlers.ToList().Count > 0);
            runtimeListDropdown.onValueChanged.AsObservable().Subscribe(RuntimeListDropdownValueChanged);
            UpdateRuntimeDataDropdown();
            SetupPropertyContent(actorStateHandlers.First());
            SetupDatabaseEffect();
            SetupSkillContent();
            refreshBtn.OnClickAsObservable().Subscribe(RefreshBtnPress);
        }

        public void SetEnable(bool e)
        {
            gameObject.SetActive(e);
        }

        public bool GetEnable()
        {
            return gameObject.activeSelf;
        }

        public void AddRuntimeData(ActorStateHandler yeActorHandler)
        {
            UpdateRuntimeDataDropdown();
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
            var repo = OdinEditorHelpers.GetScriptableObject<SkillYeObjectRepo>();
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

        private async void SetupCurrentSkillPanel()
        {
            ClearViewContent(CurrentSkillObjectViewContentTrans);
            var currentSkillList = curActorStateHandler.GetSkillList();
            foreach (var runtimeSkill in currentSkillList)
            {
                await UniTask.WaitUntil(() => runtimeSkill.IsDirty == false);
                var element = Container.InstantiatePrefabForComponent<RuntimeSkillElement>(runtimeSkillElementPrefab);
                element.transform.SetParent(CurrentSkillObjectViewContentTrans);
                element.Setup(runtimeSkill);
            }
        }

        private void OnSkillClick(SkillObject skillObject)
        {
            curActorStateHandler.AddSkill(skillObject);
            RefreshBtnPress(Unit.Default);
            SetupCurrentSkillPanel();
        }

        private void OnSkillElementExit()
        {
            messageGUI.text = "";
        }

        private void OnSkillElementEnter(SkillObject skillObject)
        {
            var tagString = string.Join(",",
                skillObject.tagEffectList.Select(e => tagDataRepo.GetDataWithKeyName(e.tagName).GetDisplayName())
                    .ToList());
            var customString = string.Join(",", skillObject.customEffects.Select(_ => _.propertyName).ToList());

            var key = skillObject.baseDamage.propertyName;

            var damageStr = "";

            damageStr = $"{GetPropertyStr(key)}x{skillObject.baseDamage.values[0]}% x";

            foreach (var tag in skillObject.tagEffectList)
                damageStr += $"{GetPropertyStr(tag.tagName)}%x{tag.value * 100}%x";

            foreach (var customEffect in skillObject.customEffects)
                damageStr += $"{GetPropertyStr(customEffect.propertyName)}%x{customEffect.value * 100}%x";

            damageStr = damageStr.Substring(0, damageStr.Length - 1);

            messageGUI.text =
                $"{skillObject.GetDisplayName()}<br>Tags:{tagString}<br>Customs:{customString}<br>{damageStr}";

            return;

            string GetPropertyStr(string key)
            {
                var propertyValue = curActorStateHandler.GetRuntimeProperty(key);
                var propertyName = propertyNames.GetDisplayName(key);
                return $"{propertyName}(<color=#007700>{propertyValue}</color>)";
            }
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
            runtimeListDropdown.ClearOptions();
            runtimeListDropdown.AddOptions(runtimeDropdownContent);
        }

        private void RuntimeListDropdownValueChanged(int i)
        {
            SetupPropertyContent(actorStateHandlers[i]);
            SetupApplyEffectContent(actorStateHandlers[i]);
            SetupSkillContent();
            SetupCurrentSkillPanel();
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
            for (var i = 0; i < trans.childCount; ++i)
            {
                var child = trans.GetChild(i);
                Destroy(child.gameObject);
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
            SetupCurrentSkillPanel();
        }
    }
}
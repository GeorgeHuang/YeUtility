using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using YeActorState.RuntimeCore;
using YeUtility.Editor;
using YeUtility.EditorHelper;

namespace YeActorState.Editor
{
    public class YeActorStateEditor : OdinMenuEditorWindow
    {
        private OdinMenuTree _tree;

        [MenuItem("Tools/YeActorStateEditor")]
        private static void Open()
        {
            var window = GetWindow<YeActorStateEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1200, 750);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            _tree = new OdinMenuTree(true);

            var key = "總覽";
            _tree.Add(key, new InitPage());

            key = "屬性總表";
            _tree.Add(key, OdinEditorHelpers.GetScriptableObject<PropertyNames>());

            key = "角色基本屬性";
            var page = new ObjectRepoPage<YeActorBaseDataRepo, YeActorBaseData>();
            _tree.Add(key, page);
            //page.AddDateItem(_tree, key);
            var actorDataRepo = OdinEditorHelpers.GetScriptableObject<YeActorBaseDataRepo>();
            if (actorDataRepo != null)
            {
                actorDataRepo.Datas.ForEach(x => { _tree.Add($"{key}/{x.name}", new ActorDataPage(x)); });
            }

            key = "角色模板";
            _tree.Add(key, OdinEditorHelpers.GetScriptableObject<ActorDataTemplateRepo>());

            key = "屬性效果資料庫";
            _tree.Add(key, OdinEditorHelpers.GetScriptableObject<PropertyEffectRepo>());
                
            key = "Tag庫";
            _tree.Add(key, OdinEditorHelpers.GetScriptableObject<TagDataRepo>());
            
            key = "技能庫";
            _tree.Add(key, OdinEditorHelpers.GetScriptableObject<SkillYeObjectRepo>());

            return _tree;
        }
    }
}
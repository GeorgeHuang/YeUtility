using ActorStateTest.Data;
using ActorStateTest.Systems;
using OdinUnit;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using YeUtility.Editor;

namespace ActorStateTest.Editor
{
    public class MainEditor  : OdinMenuEditorWindow
    {
        private OdinMenuTree _tree;

        [MenuItem("Tools/MainEditor")]
        private static void Open()
        {
            var window = GetWindow<MainEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1200, 750);
        }
        
        protected override OdinMenuTree BuildMenuTree()
        {
            _tree = new OdinMenuTree(true);
            
            var key = "角色資料";
            _tree.Add(key, new ObjectRepoPage<ActorDataRepo, ActorData>());
            var actorDataRepo = OdinEditorHelpers.GetScriptableObject<ActorDataRepo>();
            actorDataRepo.Datas.ForEach(x => _tree.Add($"{key}/{x.GetDisplayName()}", x));

            key = "技能資料";
            _tree.Add(key, new ObjectRepoPage<SkillDataRepo, SkillData>());
            var skillDataRepo = OdinEditorHelpers.GetScriptableObject<SkillDataRepo>();
            skillDataRepo.Datas.ForEach(x => _tree.Add($"{key}/{x.GetDisplayName()}", x));

            key = "設定";
            _tree.Add(key, OdinEditorHelpers.GetScriptableObject<ShootingRageConfig>());
            
            return _tree;
        }
    }
}
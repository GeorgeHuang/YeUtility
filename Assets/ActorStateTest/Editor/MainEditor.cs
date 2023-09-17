using ActorStateTest.Data;
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
            var repo = OdinEditorHelpers.GetScriptableObject<ActorDataRepo>();
            repo.Datas.ForEach(x => _tree.Add($"{key}/{x.GetDisplayName()}", x));
            
            return _tree;
        }
    }
}
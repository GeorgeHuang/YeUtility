using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using YeUtility.Editor;

namespace YeActorState.Editor
{
    public class YeActorStateEditor :OdinMenuEditorWindow
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

            key = "角色基本屬性";
            var page = new ObjectRepoPage<YeActorBaseDataRepo, YeActorBaseData>();
            _tree.Add(key, page);
            page.AddDateItem(_tree, key);
            
            return _tree;
        }
    }
}

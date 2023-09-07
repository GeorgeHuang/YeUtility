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
            
            _tree.Add("角色基本屬性", new ObjectRepoPage<YeActorBaseDataRepo, YeActorBaseData>());
            
            return _tree;
        }
    }
}
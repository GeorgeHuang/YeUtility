using OdinUnit;
using Sirenix.OdinInspector;
using YeUtility;

namespace YeActorState.Editor
{
    public class InitPage
    {
        [BoxGroup("資料夾")] [FolderPath(RequireExistingPath = true), OnValueChanged("CheckDir")]
        public string rootPath = "Assets\\SO\\ActorStateData";

        [BoxGroup("資料夾")]
        [FolderPath(RequireExistingPath = true, ParentFolder = "$rootPath"), OnValueChanged("CheckDir")]
        public string metaDirName = "Meta";

        [BoxGroup("資料夾")]
        [FolderPath(RequireExistingPath = true, ParentFolder = "$rootPath"), OnValueChanged("CheckDir")]
        public string actorDataDirName = "ActorData";
        
        [BoxGroup("資料庫")] public PropertyNames propertyNames;
        [BoxGroup("資料庫")] public YeActorBaseDataRepo actorBaseDataRepo;
        [BoxGroup("資料庫")] public ActorDataTemplateRepo templateRepo;

        private bool showCreateDirButton = false;
        private bool hasRootDir;
        private bool hasMetaDir;
        private bool hasActorDataDir;
        private string actorDataPath;
        private string metaPath;

        public InitPage()
        {
            CheckDir();
            propertyNames = OdinEditorHelpers.GetScriptableObject<PropertyNames>(metaPath);
            actorBaseDataRepo = OdinEditorHelpers.GetScriptableObject<YeActorBaseDataRepo>(actorDataPath);
        }

        private void CheckDir()
        {
            hasRootDir = Common.HasDir(rootPath);
            metaPath = Common.PathCombine(new[] { rootPath, metaDirName });
            hasMetaDir = Common.HasDir(metaPath);
            actorDataPath = Common.PathCombine(new[] { rootPath, actorDataDirName });
            hasActorDataDir = Common.HasDir(actorDataPath);
            showCreateDirButton = !hasRootDir || !hasMetaDir || !hasActorDataDir;
        }

        [BoxGroup("資料夾")]
        [Button("建立缺少資料夾"), ShowIf("showCreateDirButton")]
        private void CreateDir()
        {
            if (!hasRootDir) Common.CreateDir(rootPath);
            if (!hasMetaDir) Common.CreateDir(metaPath);
            if (!hasActorDataDir) Common.CreateDir(actorDataPath);
        }

        [BoxGroup("資料庫"), Button("建立資料庫")]
        private void CreateDataBase()
        {
            var hasFile = propertyNames != null;
            if (hasFile == false)
                propertyNames = OdinEditorHelpers.CreateScriptableObject<PropertyNames>(metaPath);
            hasFile = actorBaseDataRepo != null;
            if (hasFile == false)
                actorBaseDataRepo = OdinEditorHelpers.CreateScriptableObject<YeActorBaseDataRepo>(actorDataPath);
        }

        [BoxGroup("資料庫"), Button("建立角色模板庫")]
        private void CreateActorDataTemplate()
        {
            templateRepo = OdinEditorHelpers.GetScriptableObject<ActorDataTemplateRepo>();
            if (templateRepo == null)
            {
                templateRepo = OdinEditorHelpers.CreateScriptableObject<ActorDataTemplateRepo>(metaPath);
            }
        }
    }
}
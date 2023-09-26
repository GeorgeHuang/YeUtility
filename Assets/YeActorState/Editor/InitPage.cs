using OdinUnit;
using Sirenix.OdinInspector;
using YeActorState.RuntimeCore;
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
        
        [BoxGroup("資料庫")] public PropertyNames PropertyNames;
        [BoxGroup("資料庫")] public YeActorBaseDataRepo ActorBaseDataRepo;
        [BoxGroup("資料庫")] public ActorDataTemplateRepo TemplateRepo;
        [BoxGroup("資料庫")] public PropertyEffectRepo PropertyEffectRepo;
        [BoxGroup("資料庫")] public TagDataRepo TagDataRepo;
        [BoxGroup("資料庫")] public SkillObjectRepo SkillObjectRepo;
        

        private bool showCreateDirButton = false;
        private bool hasRootDir;
        private bool hasMetaDir;
        private bool hasActorDataDir;
        private string actorDataPath;
        private string metaPath;

        public InitPage()
        {
            CheckDir();
            PropertyNames = OdinEditorHelpers.GetScriptableObject<PropertyNames>(metaPath);
            ActorBaseDataRepo = OdinEditorHelpers.GetScriptableObject<YeActorBaseDataRepo>(actorDataPath);
            TemplateRepo = OdinEditorHelpers.GetScriptableObject<ActorDataTemplateRepo>(metaPath);
            PropertyEffectRepo = OdinEditorHelpers.GetScriptableObject<PropertyEffectRepo>(metaPath);
            TagDataRepo = OdinEditorHelpers.GetScriptableObject<TagDataRepo>(metaPath);
            SkillObjectRepo = OdinEditorHelpers.GetScriptableObject<SkillObjectRepo>();
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
            var hasFile = PropertyNames != null;
            if (hasFile == false)
                PropertyNames = OdinEditorHelpers.CreateScriptableObject<PropertyNames>(metaPath);
            hasFile = ActorBaseDataRepo != null;
            if (hasFile == false)
                ActorBaseDataRepo = OdinEditorHelpers.CreateScriptableObject<YeActorBaseDataRepo>(actorDataPath);
        }

        [BoxGroup("資料庫"), Button("建立角色模板庫")]
        private void CreateActorDataTemplate()
        {
            TemplateRepo = OdinEditorHelpers.GetScriptableObject<ActorDataTemplateRepo>();
            if (TemplateRepo == null)
            {
                TemplateRepo = OdinEditorHelpers.CreateScriptableObject<ActorDataTemplateRepo>(metaPath);
            }
        }

        [BoxGroup("資料庫"), Button("建立屬性效果資料庫")]
        private void CreateEffectDataRepo()
        {
            PropertyEffectRepo = OdinEditorHelpers.GetScriptableObject<PropertyEffectRepo>();
            if (PropertyEffectRepo != null) return;
            PropertyEffectRepo = OdinEditorHelpers.CreateScriptableObject<PropertyEffectRepo>(metaPath);
        }
        
        [BoxGroup("資料庫"), Button("建立Tag庫")]
        private void CreateTagDataRepo()
        {
            TagDataRepo = OdinEditorHelpers.GetScriptableObject<TagDataRepo>();
            if (TagDataRepo != null) return;
            TagDataRepo = OdinEditorHelpers.CreateScriptableObject<TagDataRepo>(metaPath);
        }
        
        [BoxGroup("資料庫"), Button("建立技能庫")]
        private void CreateSkillRepo()
        {
            SkillObjectRepo = OdinEditorHelpers.GetScriptableObject<SkillObjectRepo>();
            if (SkillObjectRepo != null) return;
            SkillObjectRepo = OdinEditorHelpers.CreateScriptableObject<SkillObjectRepo>(metaPath);
        }
    }
}
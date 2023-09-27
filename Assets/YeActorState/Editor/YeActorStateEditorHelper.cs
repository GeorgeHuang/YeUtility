using System.Collections;
using YeActorState.RuntimeCore;
using PropertyNamesRepo = YeActorState.PropertyNames;
using ActorDataTemplateRepo = YeActorState.ActorDataTemplate;

namespace YeActorState.Editor
{
    public class YeActorStateEditorHelper
    {
        public static IEnumerable PropertyNames => PropertyNamesRepo.GetStringDropdown();
        public static IEnumerable ActorTemplates => ActorDataTemplateRepo.GetObjectDropdown();
        public static IEnumerable BaseActorNames => YeActorBaseDataRepo.GetStringDropdown();
        public static IEnumerable BaseActorData => YeActorBaseDataRepo.GetObjectDropdown();
        public static IEnumerable PropertyEffectNames => PropertyEffectRepo.GetStringDropdown();
        public static IEnumerable PropertyEffectObjects => PropertyEffectRepo.GetObjectDropdown();
        public static IEnumerable Tags => TagDataRepo.GetStringDropdown();
        public static IEnumerable Skills => SkillObjectRepo.GetObjectDropdown();
    }
}
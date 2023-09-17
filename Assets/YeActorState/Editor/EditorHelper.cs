using System.Collections;

using PropertyNamesRepo = YeActorState.PropertyNames;
using ActorDataTemplateRepo = YeActorState.ActorDataTemplate;

namespace YeActorState.Editor
{
    public class EditorHelper
    {
        public static IEnumerable PropertyNames => PropertyNamesRepo.GetStringDropdown();
        public static IEnumerable ActorTemplates => ActorDataTemplateRepo.GetObjectDropdown();

        public static IEnumerable BaseActorNames => YeActorBaseDataRepo.GetStringDropdown();
    }
}
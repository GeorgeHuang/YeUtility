using System.Collections;

using PropertyNamesRepo = YeActorState.PropertyNames;

namespace YeActorState.Editor
{
    public class EditorHelper
    {
        public static IEnumerable PropertyNames => PropertyNamesRepo.GetDropdownOdin();
    }
}
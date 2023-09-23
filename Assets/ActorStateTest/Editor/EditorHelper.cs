using System.Collections;
using ActorStateTest.Data;

namespace ActorStateTest.Editor
{
    public class EditorHelper
    {
        public static IEnumerable ActorNames => ActorDataRepo.GetStringDropdown();
    }
}
using MergetoolGui;
using Sirenix.OdinInspector;
using UnityEngine;
using YeActorState;
using YeUtility;

namespace ActorStateTest.Data
{
    [CreateAssetMenu(fileName = "ActorData", menuName = "Tools/SO/Create ActorData", order = 0)]
    public class ActorData : ScriptableObject, INamedObject
    {
        [ValueDropdown("@EditorHelper.BaseActorData")]
        public YeActorBaseData yeActorBaseData;

        public string displayName;

        public string GetDisplayName()
        {
            return displayName;
        }

        public string GetKeyName()
        {
            return name;
        }
    }
}
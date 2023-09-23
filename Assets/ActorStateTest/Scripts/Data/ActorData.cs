using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using YeActorState;
using YeUtility;

namespace ActorStateTest.Data
{
    [CreateAssetMenu(fileName = "ActorData", menuName = "Tools/SO/Create ActorData", order = 0)]
    public class ActorData : ScriptableObject, INamedObject
    {
        [ValueDropdown("@YeActorStateEditorHelper.BaseActorData")]
        public YeActorBaseData yeActorBaseData;

        public string displayName;
        [SerializeField] public GameObject modelPrefab;

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
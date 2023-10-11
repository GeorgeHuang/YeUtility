using UnityEngine;

namespace YeUtility
{
    public class NamedScriptableObject : ScriptableObject, INamedObject
    {
        [SerializeField] private string displayName;

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
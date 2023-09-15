using UnityEngine;
using YeUtility;

namespace CommonUnit
{
    public class NamedObject : INamedObject
    {
        [SerializeField] private string displayName;
        [SerializeField] private string keyName;
        
        public string GetDisplayName()
        {
            return displayName;
        }

        public string GetKeyName()
        {
            return keyName;
        }
    }
}
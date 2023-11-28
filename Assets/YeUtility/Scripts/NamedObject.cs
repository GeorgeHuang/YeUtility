using System;
using UnityEngine;

namespace YeUtility
{
    [Serializable]
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

        public void SetDisplayName(string v)
        {
            displayName = v;
        }

        public void SetKeyName(string v)
        {
            keyName = v;
        }
    }
}
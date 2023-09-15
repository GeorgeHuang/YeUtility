using System.Collections;
using System.Collections.Generic;
using OdinUnit;
using Sirenix.OdinInspector;
using UnityEngine;

namespace YeUtility
{
    public class ObjectRepo<T> : ScriptableObject where T : INamedObject
    {
        [SerializeField] protected List<T> datas = new();

        public int Count => datas.Count;
        
        public static IEnumerable GetDropdownOdin()
        {
            var repo = OdinEditorHelpers.GetScriptableObject<ObjectRepo<T>>();
            var rv = new List<ValueDropdownItem>();
            for (var i = 0; i < repo.Count; ++i)
            {
                var data = repo.datas[i];
                rv.Add(new ValueDropdownItem ( data.GetDisplayName(), data.GetKeyName() ));
            }
            return rv;
        }
    }
}
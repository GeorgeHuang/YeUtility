using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OdinUnit;
using Sirenix.OdinInspector;
using UnityEngine;

namespace YeUtility
{
    public class ObjectRepo<RT, T> : ScriptableObject where RT : ObjectRepo<RT, T> where T : INamedObject
    {
        [Searchable]
        [SerializeField] protected List<T> datas = new();

        public int Count => datas.Count;
        public IEnumerable<T> Datas => datas;

        public T GetDataWithKeyName(string keyName)
        {
            return datas.FirstOrDefault(x => x.GetKeyName() == keyName);
        }
        
        public static IEnumerable GetStringDropdown()
        {
            var repo = OdinEditorHelpers.GetScriptableObject<RT>();
            if (repo == null)
            {
                return new List<ValueDropdownItem>();
            }
            var rv = new List<ValueDropdownItem>();
            for (var i = 0; i < repo.Count; ++i)
            {
                var data = repo.datas[i];
                rv.Add(new ValueDropdownItem ( data.GetDisplayName(), data.GetKeyName() ));
            }
            return rv;
        }
        public static IEnumerable GetObjectDropdown()
        {
            var repo = OdinEditorHelpers.GetScriptableObject<RT>();
            if (repo == null)
            {
                return new List<ValueDropdownItem>();
            }
            var rv = new List<ValueDropdownItem>();
            for (var i = 0; i < repo.Count; ++i)
            {
                var data = repo.datas[i];
                rv.Add(new ValueDropdownItem ( data.GetDisplayName(), data ));
            }
            return rv;
        }
    }
}
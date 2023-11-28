using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using YeUtility.EditorHelper;

namespace YeUtility
{
    //T不是ScriptableObject的時候用, 如果是ScriptableObject要用ScriptableObjectRepo
    public class YeObjectRepo<RT, T> : ScriptableObject where RT : YeObjectRepo<RT, T> where T : INamedObject
    {
        [Searchable] [SerializeField] protected List<T> datas = new();

        public int Count => datas.Count;
        public IEnumerable<T> Datas => datas;

        public T GetDataWithKeyName(string keyName)
        {
            return datas.FirstOrDefault(x => x.GetKeyName() == keyName);
        }

        public bool HasDataWithKeyName(string keyName)
        {
            return GetDataWithKeyName(keyName) != null;
        }

        public void AddDataWithKeyName(string keyName, T data)
        {
            if (HasDataWithKeyName(keyName) != false) return;
            datas.Add(data);
#if UNITY_EDITOR
            OdinEditorHelpers.SetDirty(this);
#endif
        }

        public void RemoveDataWithKeyName(string keyName)
        {
            var data = GetDataWithKeyName(keyName);
            if (data == null) return;
            datas.Remove(data);
#if UNITY_EDITOR
            OdinEditorHelpers.SetDirty(this);
#endif
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
                rv.Add(new ValueDropdownItem(data.GetDisplayName(), data.GetKeyName()));
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
                rv.Add(new ValueDropdownItem(data.GetDisplayName(), data));
            }

            return rv;
        }
    }
}
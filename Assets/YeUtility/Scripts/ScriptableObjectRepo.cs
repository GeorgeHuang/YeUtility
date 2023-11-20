using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using YeUtility.EditorHelper;

namespace YeUtility
{
    //T是ScriptableObject的時候用, 如果不是ScriptableObject要用YeObjectRepo
    public class ScriptableObjectRepo<TR, T> : ScriptableObject where TR : ScriptableObjectRepo<TR, T> where T : ScriptableObject
    {
        [ReadOnly] [SerializeField, ListDrawerSettings(ShowFoldout = true)]
        protected List<T> datas = new();

        public T GetData(string dataName)
        {
            return datas.FirstOrDefault(x => x.name == dataName);
        }

        public bool HasData(string dataName)
        {
            return datas.Any(x => x.name == dataName);
        }

        public IEnumerable<T> Datas => datas;

        public int Count => datas.Count;

#if UNITY_EDITOR
        [Button(ButtonSizes.Medium), PropertyOrder(-1)]
        public void UpdateList()
        {
            datas.Clear();
            var temp = AssetDatabase.GetAssetPath(this);
            temp = Path.GetDirectoryName(temp); //.Replace("\\","/");
            var temp1 = AssetDatabase.FindAssets("a:all", new string[] { temp });
            foreach (var id in temp1)
            {
                var path = AssetDatabase.GUIDToAssetPath(id);
                var obj = AssetDatabase.LoadAssetAtPath<T>(path);
                if (obj != null) datas.Add(obj);
            }

            OdinEditorHelpers.SetDirty(this);
        }

        public static IEnumerable GetStringDropdown()
        {
            var repo = OdinEditorHelpers.GetScriptableObject<TR>();
            var rv = new List<ValueDropdownItem>();
            for (var i = 0; i < repo.Count; ++i)
            {
                var data = repo.datas[i];
                rv.Add(new ValueDropdownItem(data.name, data.name));
            }

            return rv;
        }

        public static IEnumerable GetObjectDropdown()
        {
            var repo = OdinEditorHelpers.GetScriptableObject<TR>();
            var rv = new List<ValueDropdownItem>();
            for (var i = 0; i < repo.Count; ++i)
            {
                var data = repo.datas[i];
                rv.Add(new ValueDropdownItem(data.name, data));
            }
            return rv;
        }
#endif
    }
}
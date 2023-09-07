using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System.IO;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using System.Collections;
#endif
namespace OdinUnit
{
    public class ScriptableObjectRepo<T> : ScriptableObject where T : ScriptableObject
    {
        [ReadOnly] [SerializeField, ListDrawerSettings(ShowFoldout = true)]
        protected List<T> datas = new();

        public T GetData(string dataName)
        {
            return datas.FirstOrDefault(x => x.name == dataName);
        }

        public bool HasData(string dataName)
        {
            return datas.Any(x =>x.name == dataName);
        }

        public IEnumerable<T> Datas => datas;

        public int Count => datas.Count;

#if UNITY_EDITOR
        [Button(ButtonSizes.Medium), PropertyOrder(-1)]
        public void UpdateList()
        {
            datas.Clear();
            var temp = AssetDatabase.GetAssetPath(this);
            temp = Path.GetDirectoryName(temp);//.Replace("\\","/");
            var temp1 = AssetDatabase.FindAssets("a:all", new string[] { temp });
            foreach (var id in temp1)
            {
                var path = AssetDatabase.GUIDToAssetPath(id);
                var obj = AssetDatabase.LoadAssetAtPath<T>(path);
                if (obj != null) datas.Add(obj);
            }
            OdinEditorHelpers.SetDirty(this);
        }

        public static IEnumerable GetDropdownOdin()
        {
            var repo = OdinEditorHelpers.GetScriptableObject<ScriptableObjectRepo<T>>();
            var rv = new List<ValueDropdownItem>();
            for (var i = 0; i < repo.Count; ++i)
            {
                var data = repo.datas[i];
                rv.Add(new ValueDropdownItem ( data.name, data.name ));
            }
            return rv;
        }
#endif
    }
}

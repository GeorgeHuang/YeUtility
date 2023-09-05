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
    public class ObjectRepo<T> : ScriptableObject where T : Object
    {
        [ReadOnly]
        [SerializeField, ListDrawerSettings(Expanded = true)]
        protected List<T> datas = new List<T>();

        public T GetData(string name)
        {
            foreach (var data in datas)
            {
                if (data.name == name)
                    return data;
            }
            return null;
        }

        public bool HasData(string name)
        {
            return datas.Any(x =>x.name == name);
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

        static public IEnumerable GetDropdownOdin()
        {
            var repo = OdinEditorHelpers.GetScriptableObject<ObjectRepo<T>>();
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

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace YeUtility
{
    public class EditorUnit
    {
        private static readonly string ActorPath = "Assets/SO/Actor/Player";

        public static IEnumerable GetObjList<T>(Object sourceObj) where T : Object
        {
            var datas = new List<ValueDropdownItem>();
            datas.Add(new ValueDropdownItem());
            var temp = AssetDatabase.GetAssetPath(sourceObj);
            temp = Path.GetDirectoryName(temp);//.Replace("\\","/");
            var temp1 = AssetDatabase.FindAssets("a:all", new string[] { temp });
            foreach (var id in temp1)
            {
                var path = AssetDatabase.GUIDToAssetPath(id);
                var obj = AssetDatabase.LoadAssetAtPath<T>(path);
                if (obj != null) datas.Add(new ValueDropdownItem(obj.name, obj));
            }
            return datas;
        }
        public static IEnumerable GetObjList<T>(string sourcePath) where T : Object
        {
            var datas = new List<ValueDropdownItem>();
            datas.Add(new ValueDropdownItem());
            var temp1 = AssetDatabase.FindAssets("a:all", new string[] { sourcePath });
            foreach (var id in temp1)
            {
                var path = AssetDatabase.GUIDToAssetPath(id);
                var obj = AssetDatabase.LoadAssetAtPath<T>(path);
                if (obj != null) datas.Add(new ValueDropdownItem(obj.name, obj));
            }
            return datas;
        }
        public static IEnumerable GetActorNameList()
        {
            var datas = new List<ValueDropdownItem>();
            var temp1 = AssetDatabase.FindAssets("t:PlayerActorData", new string[] { ActorPath });
            foreach (var id in temp1)
            {
                var path = AssetDatabase.GUIDToAssetPath(id);
                var obj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                if (obj != null) datas.Add(new ValueDropdownItem(obj.name, obj.name));
            }
            return datas;
        }
    }
}
#endif

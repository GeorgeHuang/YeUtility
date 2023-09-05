using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OdinUnit
{
    public class OdinEditorHelpers
    {
        public static void SetDirty(Object obj)
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(obj);
#endif
        }
#if UNITY_EDITOR
        static public List<T> GetObjList<T>(string path) where T : Object
        {
            var rv = new List<T>();
            var temp1 = AssetDatabase.FindAssets("a:all",
                                                 new string[] { path });
            foreach (var id in temp1)
            {
                var objPath = AssetDatabase.GUIDToAssetPath(id);
                var obj = AssetDatabase.LoadAssetAtPath<T>(objPath);
                if (obj != null) rv.Add(obj);
            }

            return rv;
        }
        public static T GetScriptableObject<T>() where T : ScriptableObject
        {
            var list = GetScriptableObjects<T>();
            if (list.Any())
                return list.First();
            return null;
        }
        public static List<T> GetScriptableObjects<T>() where T : ScriptableObject
        {
            var ts   = new List<T>();
            var type = typeof(T);
            var guids2 = AssetDatabase.FindAssets($"t:{type}");
            foreach (var guid2 in guids2)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid2);
                ts.Add((T)AssetDatabase.LoadAssetAtPath(assetPath , type));
            }
            return ts;
        }

        public static ValueDropdownItem GetValueDropdownItem(string text, string value)
        {
            return new ValueDropdownItem(text, value);
        }
#endif
    }
}

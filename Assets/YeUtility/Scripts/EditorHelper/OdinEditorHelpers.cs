using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace YeUtility.EditorHelper
{
    public class OdinEditorHelpers
    {
        public static void SetDirty(Object obj)
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(obj);
#endif
        }

        public static List<T> GetObjList<T>(string path = "Assets") where T : Object
        {
            var rv = new List<T>();
#if UNITY_EDITOR
            var temp1 = AssetDatabase.FindAssets("a:all",
                new string[] { path });
            foreach (var id in temp1)
            {
                var objPath = AssetDatabase.GUIDToAssetPath(id);
                var obj = AssetDatabase.LoadAssetAtPath<T>(objPath);
                if (obj != null) rv.Add(obj);
            }
#endif
            return rv;
        }

        public static T CreateScriptableObject<T>(string path) where T : ScriptableObject
        {
            var newFile = ScriptableObject.CreateInstance<T>();
            var newFileType = typeof(T).ToString().Split(".").LastOrDefault();
            var newFilePath = $"{path}\\{newFileType}.asset";
            AssetDatabase.CreateAsset(newFile, newFilePath);
            return newFile;
        }

        public static T GetScriptableObject<T>(string path = "Assets") where T : ScriptableObject
        {
            var list = GetScriptableObjects<T>(path);
            return list.Any() ? list.First() : null;
        }

        public static List<T> GetScriptableObjects<T>(string path = "Assets") where T : ScriptableObject
        {
            var ts = new List<T>();
#if UNITY_EDITOR
            var type = typeof(T);
            var guids2 = AssetDatabase.FindAssets($"t:{type}", new[] { path });
            foreach (var guid2 in guids2)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid2);
                ts.Add((T)AssetDatabase.LoadAssetAtPath(assetPath, type));
            }
#endif
            return ts;
        }

        public static ValueDropdownItem GetValueDropdownItem(string text, string value)
        {
            return new ValueDropdownItem(text, value);
        }
    }
}
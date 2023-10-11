using System;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace YeUtility.Editor
{
    public class TypeSelector<T> : OdinSelector<Type> where T : ScriptableObject
    {
        protected override void BuildSelectionTree(OdinMenuTree tree)
        {
            tree.Config.DrawSearchToolbar = true;

            if (typeof(T).IsClass && !typeof(T).IsAbstract) tree.Add(typeof(T).FullName, typeof(T));

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            foreach (var type in assembly.GetTypes())
                if (type.IsSubclassOf(typeof(T)) && !type.IsAbstract)
                    tree.Add(type.FullName, type);
        }
    }
}
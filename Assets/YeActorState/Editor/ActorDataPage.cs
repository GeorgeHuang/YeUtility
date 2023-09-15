using System.Collections.Generic;
using OdinUnit;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace YeActorState.Editor
{
    public class ActorDataPage
    {
        [SerializeField, InlineEditor] private YeActorBaseData data;

        [ValueDropdown("@EditorHelper.ActorTemplates")]
        [SerializeField] private ActorDataTemplate template;
        
        public ActorDataPage(YeActorBaseData data)
        {
            this.data = data;
        }

        [Button("套用模板", ButtonSizes.Medium)]
        void ApplyActorTemplate()
        {
            if (template == null)
            {
                EditorUtility.DisplayDialog("錯誤", $"沒有任何模板", "OK");
            }

            var newPropertyNames = new List<string>(template.propertyNames);
            newPropertyNames.ForEach(x=>data.AddPropertyData(x));

            for (int i = 0; i < newPropertyNames.Count; i++)
            {
                var name = newPropertyNames[i];
                var index = data.properties.FindIndex(x => x.Name == name);
                if (index != i)
                {
                    swap(data.properties, index, i);
                }
            }
            OdinEditorHelpers.SetDirty(data);
            return;
            void swap<T>(IList<T> L, int i, int j)
            {
                (L[i], L[j]) = (L[j], L[i]);
            }
        }
    }
}
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using YeUtility;
using YeUtility.EditorHelper;

namespace YeActorState.Editor
{
    public class ActorDataPage
    {
        [SerializeField] [InlineEditor(inlineEditorMode:InlineEditorModes.FullEditor)] private YeActorBaseData data;

        [ValueDropdown("@YeActorStateEditorHelper.ActorTemplates")] [SerializeField]
        private ActorDataTemplate template;

        public ActorDataPage(YeActorBaseData data)
        {
            this.data = data;
        }

        [Button("套用模板", ButtonSizes.Medium)]
        private void ApplyActorTemplate()
        {
            if (template == null) EditorUtility.DisplayDialog("錯誤", "沒有任何模板", "OK");

            var newPropertyNames = new List<string>(template.propertyNames);
            newPropertyNames.ForEach(x => data.AddPropertyData(x));

            for (var i = 0; i < newPropertyNames.Count; i++)
            {
                var name = newPropertyNames[i];
                var index = data.properties.FindIndex(x => x.Name == name);
                if (index != i) newPropertyNames.Swap(index, i);
            }

            OdinEditorHelpers.SetDirty(data);
        }
    }
}
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using YeUtility.EditorHelper;

namespace YeUtility.Editor
{
    public class ObjectRepoPage<TR, T> where TR : ScriptableObjectRepo<TR, T> where T : ScriptableObject
    {
        [BoxGroup("倉庫")] [SerializeField] private readonly string _dataPath;

        [BoxGroup("新資料")] [InlineEditor] [HideLabel] [SerializeField]
        private T _newData;

        [BoxGroup("新資料")] [LabelText("新資料檔名")] [SerializeField]
        private string _newDataFileName;

        [BoxGroup("倉庫")] [InlineEditor] [SerializeField]
        private readonly TR _repo;

        public ObjectRepoPage()
        {
            _repo = OdinEditorHelpers.GetScriptableObject<TR>();
            if (_repo == null) return;
            _dataPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(_repo));
            RefreshNewData();
        }

        private void RefreshNewData()
        {
            _newData = ScriptableObject.CreateInstance<T>();
        }

        [BoxGroup("新資料")]
        [Button("選擇子型別")]
        [PropertyOrder(-1)]
        public void ShowTypeSelector()
        {
            var selector = new TypeSelector<T>();
            selector.SelectionConfirmed += selection =>
            {
                var selectedType = selector.GetCurrentSelection().FirstOrDefault();
                if (selectedType != null) _newData = (T)ScriptableObject.CreateInstance(selectedType);
            };

            selector.ShowInPopup();
        }

        [BoxGroup("新資料")]
        [Button("新增資料", ButtonSizes.Large)]
        [PropertyOrder(0)]
        public void AddNewDataToRepo()
        {
            if (_repo == null || _repo.HasData(_newDataFileName))
            {
                EditorUtility.DisplayDialog("錯誤", $"檔名已經存在 {_newDataFileName}", "OK");
                return;
            }

            var newDataPath = $"{_dataPath}/{_newDataFileName}.asset";
            AssetDatabase.CreateAsset(_newData, newDataPath);
            _repo.UpdateList();
            RefreshNewData();
        }

        public void AddDateItem(OdinMenuTree tree, string key)
        {
            if (_repo == null) return;
            foreach (var item in _repo.Datas) tree.Add($"{key}/{item.name}", item);
        }
    }
}
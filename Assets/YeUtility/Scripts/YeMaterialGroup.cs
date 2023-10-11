using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace YeUtility
{
    public class YeMaterialGroup : MonoBehaviour
    {
        [SerializeField] private List<SpriteRenderer> sprites = new();

        public List<SpriteRenderer> Sprites
        {
            get => sprites;
            set => sprites = value;
        }

        public Material M { get; set; }

        private void Awake()
        {
        }

        private void Start()
        {
            //M = sprites[0].sharedMaterial;
            M = sprites[0].material;
            Apply();
        }

        [ContextMenu("Apply")]
        public void Apply()
        {
            sprites.ForEach(x =>
            {
                //x.material = m;
                x.sharedMaterial = M;
            });
        }

        [ContextMenu("Find All Sprite Renderer")]
        public void FindAllSpriteRenderer()
        {
            sprites.Clear();
            var objs = gameObject.GetComponentsInChildren(typeof(SpriteRenderer), true);
            foreach (var com in objs)
            {
                var sr = com as SpriteRenderer;
                sprites.Add(sr);
            }
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
}
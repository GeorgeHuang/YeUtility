using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace YeUtility
{
    public class MaterialGroup : MonoBehaviour
    {
        [SerializeField]
        List<SpriteRenderer> sprites = new List<SpriteRenderer>();
        public List<SpriteRenderer> Sprites { get => sprites; set => sprites = value; }
        public Material M { get => m; set => m = value; }

        Material m;

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
            Component[] objs = gameObject.GetComponentsInChildren(typeof(SpriteRenderer), true);
            foreach (Component com in objs)
            {
                SpriteRenderer sr = com as SpriteRenderer;
                sprites.Add(sr);
            }
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
}

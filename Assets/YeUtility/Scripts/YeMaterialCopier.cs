using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YeUtility
{
    public class YeMaterialCopier : MonoBehaviour
    {
        public List<Renderer> renders = new List<Renderer>();
        public List<Image> images = new List<Image>();
        public Material mat;
        public Shader shader;

        private void Start()
        {
#if UNITY_EDITOR
            foreach (var ren in renders)
            {
                ren.sharedMaterial = Instantiate(ren.material);
            }

            foreach (var im in images)
            {
                im.material = Instantiate(mat);
            }
#endif
        }
    }
}

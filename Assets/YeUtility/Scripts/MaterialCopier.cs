using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CommonUnit
{
    public class MaterialCopier : MonoBehaviour
    {
        public List<Renderer> renders = new List<Renderer>();
        public List<Image> images = new List<Image>();
        public Material mat;
        public Shader shader;

        private void Start()
        {
#if UNITY_EDITOR
            //放在Awake沒有用
            foreach (var ren in renders)
            {
                //ren.sharedMaterial.CopyPropertiesFromMaterial(mat);
                //ren.material = Instantiate(mat);
                //ren.material.CopyPropertiesFromMaterial(mat);
                //ren.sharedMaterial = Instantiate(ren.sharedMaterial);
                ren.sharedMaterial = Instantiate(ren.material);
                //var sh = Instantiate(shader);
                //ren.sharedMaterial = new Material(sh);
                //ren.sharedMaterial.CopyPropertiesFromMaterial(mat);
                //ren.material = ren.sharedMaterial;
                //ren.
                //ren.sharedMaterial.CopyPropertiesFromMaterial(mat);
                //ren.sharedMaterial = null;
            }

            foreach (var im in images)
            {
                im.material = Instantiate(mat);
            }
#endif
        }
    }
}

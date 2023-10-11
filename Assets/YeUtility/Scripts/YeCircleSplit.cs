using System.Collections.Generic;
using UnityEngine;

namespace YeUtility
{
    public class YeCircleSplit : MonoBehaviour
    {
        //public float radiusOffset;

        public float radius = 1;

        public int number = 8;

        List<GameObject> objs = new();

        public List<GameObject> Apply(int number, bool firstAtTop = false)
        {
            this.number = number;
            return Apply(firstAtTop);
        }

        [ContextMenu("Apply")]
        public List<GameObject> Apply()
        {
            return Apply(false);
        }
        [ContextMenu("ApplyUp")]
        public List<GameObject> ApplyUp()
        {
            return Apply(true);
        }
        
        public List<GameObject> Apply(bool firstAtTop)
        {
            Clear();

            var dirs = Common.SplitCircle(
                number, 
                firstAtTop ? new Vector3(0,1,0) : null );
            for (int i = 0; i < dirs.Count; i++)
            {
                var dir = dirs[i];
                var obj = new GameObject(i.ToString());
                Common.ChangeGOParent(obj,gameObject);
                obj.transform.right = dir;
                obj.transform.Translate(radius,0,0);
                obj.transform.localScale = Vector3.one;
                objs.Add(obj);
            }
            
            transform.rotation = Quaternion.identity;
            return objs;
        }

        [ContextMenu("Clear")]
        public void Clear()
        {
            foreach (var go in objs)
            {
                if (Application.isPlaying)
                {
                    Destroy(go);
                }
                else
                {
                    DestroyImmediate(go);   
                }
            }
            objs.Clear();
        }
    }
}
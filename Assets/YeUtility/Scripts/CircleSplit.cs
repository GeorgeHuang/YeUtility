using System.Collections.Generic;
using UnityEngine;

namespace CommonUnit
{
    public class CircleSplit : MonoBehaviour
    {
        //public float radiusOffset;

        public float radius = 1;

        public int number = 8;

        List<GameObject> objs = new List<GameObject>();

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public List<GameObject> Apply(int number, bool firstAtTop = false)
        {
            this.number = number;
            return Apply(firstAtTop);
        }

        [ContextMenu("Apply")]
        public List<GameObject> Apply(bool firstAtTop = false)
        {
            Clear();
            float angleP = 360.0f / number;
            for (int i = 0; i < number; ++i)
            {
                //神器的力量
                var obj = new GameObject("" + i);
                Vector3 angle = Vector3.zero;
                angle.z = angleP * i + angle.z;
                obj.transform.Rotate(angle, Space.World);
                Common.changeGOParent(obj, gameObject, false, false);
                obj.transform.Translate(radius, 0, 0, Space.Self);
                objs.Add(obj);
            }
            transform.rotation = Quaternion.identity;
            if (firstAtTop)
            {
                transform.Rotate(0, 0, -90);
            }
            return objs;
        }

        public void Clear()
        {
            foreach (var go in objs)
            {
                Destroy(go);
            }
            objs.Clear();
        }
    }
}

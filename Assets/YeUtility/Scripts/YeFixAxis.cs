using UnityEngine;

namespace YeUtility
{
    public class YeFixAxis : MonoBehaviour
    {
        [SerializeField]
        bool UseLateUpdate;
        [SerializeField]
        bool Local;
        [SerializeField]
        bool x;
        [SerializeField]
        bool y;
        [SerializeField]
        bool z;

        Transform trans;
        Vector3 curAngle;
        bool needSet;
        // Start is called before the first frame update
        void Start()
        {
            trans = transform;
        }

        // Update is called once per frame
        void Update()
        {
            if (!UseLateUpdate)
                update();
        }

        private void LateUpdate()
        {
            if (UseLateUpdate)
                update();
        }

        void update()
        {
            if (Local)
                curAngle = trans.localEulerAngles;
            else
                curAngle = trans.eulerAngles;
            needSet = false;

            if (x)
            {
                if (curAngle.x != 0) needSet = true;
                curAngle.x = 0;
            }
            if (y)
            {
                if (curAngle.y != 0) needSet = true;
                curAngle.y = 0;
            }
            if (z)
            {
                if (curAngle.z != 0) needSet = true;
                curAngle.z = 0;
            }

            if (Local && needSet)
                trans.localEulerAngles = curAngle;
            else if (needSet)
                trans.eulerAngles = curAngle;
        }
    }
}

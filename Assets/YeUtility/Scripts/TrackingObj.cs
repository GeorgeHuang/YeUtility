using UnityEngine;

namespace CommonUnit
{
    public class TrackingObj : MonoBehaviour
    {

        public Transform targetObj;

        public float smoothTime = 0.2f;

        bool withRotate = false;

        public bool WithRotate { get { return withRotate; } set { withRotate = value; } }

        public bool Is2D { get; set; }
        public bool IsTrackPos { get; private set; }
        public Vector3 TargetPos { get; private set; }

        Vector3 posVelocity = Vector3.zero;
        Transform trans;

        private void Awake()
        {
            trans = transform;
        }

        void Start()
        {
            if (targetObj == null) return;
            IsTrackPos = false;
        }
        public void update()
        {

            //transform.position = targetObj.position;
            updatePos();


            if (targetObj == null) return;
            if (withRotate)
            {
                if (Is2D)
                {
                    trans.right = targetObj.right;
                }
                else
                {
                    trans.forward = targetObj.forward;
                }
            }
        }

        void updatePos()
        {
            var pos = TargetPos;

            if (IsTrackPos == false)
            {
                if (targetObj != null)
                {
                    pos = targetObj.transform.position;
                }
                else
                {
                    return;
                }
            }

            if (smoothTime < 0.0001f)
            {
                trans.position = pos;
            }
            else
            {
                trans.position = Vector3.SmoothDamp(trans.position, pos, ref posVelocity, smoothTime);
            }
        }

        public void setTarget(Transform target, bool needResetPos = true)
        {
            IsTrackPos = false;
            targetObj = target;
            if (needResetPos && target)
            {
                resetPos(target.transform.position);
            }
            posVelocity = Vector3.zero;
        }

        public void resetPos(Vector3 pos)
        {
            transform.position = targetObj.position;
        }

        public void setTrackPos(Vector3 targetPos)
        {
            IsTrackPos = true;
            TargetPos = targetPos;
        }
    }
}

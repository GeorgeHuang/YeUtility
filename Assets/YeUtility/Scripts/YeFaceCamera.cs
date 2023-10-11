using UnityEngine;

namespace YeUtility
{
    public class YeFaceCamera : MonoBehaviour
    {
        enum Axis
        {
            X,
            Y,
            Z,
            NX,
            NY,
            NZ
        }
        public bool SelfUpdate = true;

        Transform cameraTrans;
        Transform trans;
        [SerializeField]
        Axis axis = Axis.Z;

        // Start is called before the first frame update
        void Start()
        {
            cameraTrans = Camera.main.transform;
            trans = transform;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (SelfUpdate) Tick();
            //trans.LookAt(cameraTrans);
            //print(trans.forward);
            //trans.rotation = Quaternion.identity;
            //var tarDir = cameraTrans.forward;
            //var dir = trans.forward;
            //var rotDir = Vector3.Cross(dir, tarDir).normalized;
            //var angle = Vector3.SignedAngle(dir, tarDir, rotDir);
            //trans.Rotate(rotDir, angle);
        }

        public void Tick()
        {
            if (axis == Axis.Z)
            {
                trans.forward = cameraTrans.forward;
            }
            else if (axis == Axis.Y)
            {
                trans.up = cameraTrans.forward;
            }
            else if (axis == Axis.X)
            {
                trans.right = cameraTrans.forward;
            }
            else if (axis == Axis.NZ)
            {
                trans.forward = -cameraTrans.forward;
            }
            else if (axis == Axis.NY)
            {
                trans.up = -cameraTrans.forward;
            }
            else if (axis == Axis.NX)
            {
                trans.right = -cameraTrans.forward;
            }
        }

        //private void OnDrawGizmos()
        //{
        //    if (cameraTrans == null) return;
        //    trans.rotation = Quaternion.identity;
        //    var tarDir = cameraTrans.forward;
        //    var dir = trans.forward;
        //    var rotDir = Vector3.Cross(dir, tarDir).normalized;
        //    var angle = Vector3.SignedAngle(dir, tarDir, rotDir);
        //    var pos = trans.position;
        //    trans.Rotate(rotDir, angle);
        //    //Gizmos.DrawRay(pos, rotDir * 3 + pos);
        //    Gizmos.DrawLine(pos, rotDir * 3 + pos);
        //}
    }
}

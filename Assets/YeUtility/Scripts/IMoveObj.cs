using UnityEngine;

namespace CommonUnit
{
    public interface IMoveObj
    {
        public void Input(Vector3 input);
        public void Stop();
        public void FaceDir(Vector3 dir);
        public Vector3 GetPos();
        public bool UseFaceDir { get; set; }
    }
}

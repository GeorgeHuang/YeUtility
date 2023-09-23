using UnityEngine;

namespace YeUtility
{
    public class HeavyRotation : MonoBehaviour
    {
        public bool m_useRotateCenter = false;
        public Vector3 m_speed = Vector3.zero;
        public Vector3 m_rotateCenter = Vector3.zero;

        bool m_pause;
        Transform m_transCache;

        public void SetPause(bool pause)
        {
            m_pause = pause;
        }

        void Awake()
        {
            m_transCache = transform;
        }


        bool HasValue()
        {
            return m_speed.sqrMagnitude > 0;
        }

        public void UpdateRotation(float dt)
        {
            if (HasValue() == false || m_pause == true)
                return;
            if (m_useRotateCenter == false)
            {
                //m_transCache.localEulerAngles = m_transCache.localEulerAngles + m_speed;
                m_transCache.Rotate(m_speed * dt);
            }
            else
            {
                m_transCache.RotateAround(m_transCache.TransformPoint(m_rotateCenter), Vector3.forward, m_speed.z);
            }
        }
    }
}

using UnityEngine;

namespace YeUtility
{
    public class HeavyRotation : MonoBehaviour
    {
        public bool m_fixedUpdate = false;
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

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (m_fixedUpdate == false)
            {
                UpdateRotation();
            }
        }

        void FixedUpdate()
        {
            if (m_fixedUpdate == true)
            {
                UpdateRotation();
            }
        }

        bool HasValue()
        {
            return m_speed.sqrMagnitude > 0;
        }

        void UpdateRotation()
        {
            if (HasValue() == false || m_pause == true)
                return;
            if (m_useRotateCenter == false)
            {
                //m_transCache.localEulerAngles = m_transCache.localEulerAngles + m_speed;
                m_transCache.Rotate(m_speed * Time.deltaTime);
            }
            else
            {
                m_transCache.RotateAround(m_transCache.TransformPoint(m_rotateCenter), Vector3.forward, m_speed.z);
            }
        }
    }
}

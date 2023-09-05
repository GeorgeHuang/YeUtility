using UnityEngine;

namespace CommonUnit
{
    public class HeavyRotation : MonoBehaviour
    {
        public bool m_fixedUpdate = false;
        public bool m_useRotateCenter = false;
        public Vector3 m_speed = Vector3.zero;
        public Vector3 m_rotateCenter = Vector3.zero;

        bool m_pause;
        Transform m_transCache;

        public void setPause(bool pause)
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
                updateRotation();
            }
        }

        void FixedUpdate()
        {
            if (m_fixedUpdate == true)
            {
                updateRotation();
            }
        }

        bool hasValue()
        {
            return m_speed.sqrMagnitude > 0;
        }

        void updateRotation()
        {
            if (hasValue() == false || m_pause == true)
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

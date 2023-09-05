using UnityEngine;

namespace CommonUnit
{
    public class RotateTween : MonoBehaviour
    {

        public Vector3 m_from;
        public Vector3 m_to;
        public float m_time;

        private float m_timer;
        private Transform m_transCache;

        void Awake()
        {
            m_transCache = transform;
        }

        void Start()
        {
            m_transCache.eulerAngles = m_from;
        }

        void Update()
        {
            m_transCache.eulerAngles = Vector3.Lerp(m_from, m_to, Mathf.InverseLerp(0, m_time, m_timer));
            m_timer += Time.deltaTime;
        }
    }
}

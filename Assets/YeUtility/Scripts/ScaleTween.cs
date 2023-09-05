using UnityEngine;

namespace CommonUnit
{
    public class ScaleTween : MonoBehaviour
    {

        public bool m_autoPlay = false;
        public bool m_pinpon = false;
        public Vector3 m_from;
        public Vector3 m_to;
        public float m_time;

        private bool m_playing = false;
        private float m_timer;
        private Vector3 m_origin = Vector3.one;
        private Vector3 m_curValue = Vector3.one;
        private Transform m_transCache;

        void Awake()
        {
            m_transCache = transform;
            m_origin = m_transCache.localScale;
            m_curValue = m_origin;
        }

        void Start()
        {
            m_transCache.localScale = m_from;
            if (m_autoPlay)
            {
                play();
            }
        }

        void Update()
        {
            if (m_playing == false) return;
            m_timer += Time.deltaTime;

            float ratio = Mathf.InverseLerp(0, m_time, m_timer);
            if (m_pinpon == true)
            {
                if (ratio > 0.5f)
                {
                    ratio = Mathf.InverseLerp(1, 0.5f, ratio);
                }
                else
                {
                    ratio = Mathf.InverseLerp(0, 0.5f, ratio);
                }
            }

            m_curValue = Vector3.Lerp(m_from, m_to, ratio);
            //m_curValue.x = m_origin.x * m_curValue.x;
            //m_curValue.y = m_origin.y * m_curValue.y;
            //m_curValue.z = m_origin.z * m_curValue.z;
            m_transCache.localScale = m_curValue;

            if (m_timer > m_time)
            {
                if (m_pinpon == false)
                {
                    m_playing = false;
                }
                else
                {
                    m_timer = 0;
                }
            }
        }

        internal void reset()
        {
            m_timer = 0;
            m_curValue = m_origin;
            m_transCache.localScale = m_from;
            play();
        }

        public void play()
        {
            if (m_playing == true) return;
            m_playing = true;
            m_timer = 0;
        }

        public void stop()
        {
            m_timer = 0;
            m_playing = false;
        }
    }
}
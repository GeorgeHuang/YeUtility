using UnityEngine;

namespace CommonUnit
{
    public class PinponTween : MonoBehaviour
    {

        public Vector3 m_targetPos;
        public float m_totalTime;
        public bool m_autoPlay;
        public bool m_useFixedUpdate;

        bool m_playing = false;
        float m_timer = 0;
        float m_onceTime;
        Vector3 m_curTarget;
        Vector3 m_originPos;
        Transform m_trans;

        public bool IsPlaying
        {
            get { return m_playing; }
            set { m_playing = value; }
        }

        void Awake()
        {
            m_trans = transform;
        }

        // Use this for initialization
        void Start()
        {
            if (m_autoPlay == true)
            {
                play();
            }
        }

        // Update is called once per frame
        void Update()
        {
            tweenUpdate(Time.deltaTime);
        }

        [ContextMenu("Play")]
        public void play()
        {
            play(m_trans.localPosition);
        }

        public void play(Vector3 pos)
        {
            if (m_playing == true || Common.compareFloat(m_totalTime, 0)
                || Common.compareVector3(m_targetPos, m_trans.localPosition))
            {
                return;
            }
            m_playing = true;
            m_onceTime = m_totalTime / 2;
            m_originPos = pos;
        }

        public void reset()
        {
            //m_playing = false;
            m_timer = 0;
        }

        void tweenUpdate(float deltaTime)
        {
            if (m_playing == true)
            {
                m_timer += deltaTime;
                if (m_timer < m_onceTime)
                {
                    float ratio = Mathf.InverseLerp(0, m_onceTime, m_timer);
                    m_trans.localPosition = m_originPos + Vector3.Lerp(Vector3.zero, m_targetPos, ratio);
                }
                else
                {
                    float ratio = Mathf.InverseLerp(m_onceTime, m_totalTime, m_timer);
                    m_trans.localPosition = m_originPos + Vector3.Lerp(m_targetPos, Vector3.zero, ratio);
                }

                if (m_timer > m_totalTime)
                {
                    reset();
                }
            }
        }
    }
}

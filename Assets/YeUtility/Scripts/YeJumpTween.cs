using UnityEngine;

namespace YeUtility
{
    public class YeJumpTween : MonoBehaviour
    {
        public Vector3 m_velocity;
        public Vector3 m_gravity = new(0, -9.8f, 0);

        public bool m_autoPlay;
        public bool m_useFixedUpdate;
        private Vector3 m_orgV = Vector3.zero;

        private Vector3 m_posCatch = Vector3.zero;
        private Transform m_trans;

        public bool IsPlaying { get; set; }

        private void Awake()
        {
            m_orgV = m_velocity;
            m_trans = transform;
        }

        // Use this for initialization
        private void Start()
        {
            if (m_autoPlay) play();
        }

        // Update is called once per frame
        private void Update()
        {
            tweenUpdate(m_useFixedUpdate ? Time.fixedDeltaTime : Time.deltaTime);
        }

        [ContextMenu("Play")]
        public void play()
        {
            if (IsPlaying) return;
            IsPlaying = true;
        }

        public void reset()
        {
            m_velocity = m_orgV;
            IsPlaying = false;
        }

        public void tweenUpdate(float deltaTime)
        {
            deltaTime = Time.deltaTime;
            if (IsPlaying)
            {
                m_posCatch = m_trans.localPosition;
                m_velocity = m_velocity + m_gravity * deltaTime;
                m_posCatch = m_posCatch + m_velocity * deltaTime;
                //m_posCatch = m_posCatch + (m_velocity + m_gravity) * deltaTime;
                //m_trans.position = m_posCatch;
                //print(m_posCatch);
                m_trans.localPosition = m_posCatch;
            }
        }

        public struct Data
        {
            public bool playing;
            public Vector3 velocity;
            public Vector3 gravity;
            public Vector3 pos;

            public Data(bool playing, Vector3 velocity, Vector3 gravity, Vector3 pos)
            {
                this.playing = playing;
                this.velocity = velocity;
                this.gravity = gravity;
                this.pos = pos;
            }

            public Data Update(float deltaTime)
            {
                velocity += gravity * deltaTime;
                pos += velocity * deltaTime;
                return this;
            }
        }
    }
}
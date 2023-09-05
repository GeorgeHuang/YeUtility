using UnityEngine;

namespace CommonUnit
{
    public class TimeToDestroy : MonoBehaviour
    {

        public float m_time;
        public bool m_hide;
        public bool m_autoStart;
        public ReuseManager m_reuseMgr;

        protected float m_timer;
        protected float m_timeRatio = 1;

        public bool IsCreated {get;set;}

        // Use this for initialization
        virtual public void Start()
        {
            if (m_autoStart)
            {
                start();
            }
        }

        virtual public void start()
        {
            m_timer = m_time;
        }

        virtual public void update(float dt)
        {
            if (m_timer > 0)
            {
                m_timer -= dt * m_timeRatio;
            }
            else
            {
                stop();
            }
        }

        public void stop()
        {
            if (IsCreated == false) return;
            subStop();
            IsCreated = false;
            if (m_reuseMgr == null)
            {
                if (m_hide == false)
                {
                    Destroy(gameObject);
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                gameObject.SetActive(false);
                m_reuseMgr.back(gameObject);
            }
        }

        virtual public void subStop()
        {

        }
    }
}
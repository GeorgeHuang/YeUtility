using UnityEngine;

namespace YeUtility
{
    public class YeTimeToDestroy : MonoBehaviour
    {

        public float m_time;
        public bool m_hide;
        public bool m_autoStart;
        public ReuseManager m_reuseMgr;

        protected float m_timer;
        protected float m_timeRatio = 1;

        public bool IsCreated {get;set;}

        // Use this for initialization
        public virtual void Start()
        {
            if (m_autoStart)
            {
                MyStart();
            }
        }

        protected virtual void MyStart()
        {
            m_timer = m_time;
        }

        public virtual void Tick(float dt)
        {
            if (m_timer > 0)
            {
                m_timer -= dt * m_timeRatio;
            }
            else
            {
                Stop();
            }
        }

        public void Stop()
        {
            if (IsCreated == false) return;
            SubStop();
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

        protected virtual void SubStop()
        {

        }
    }
}
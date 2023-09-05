using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace CommonUnit
{
    public class MaterialTween : MonoBehaviour
    {
        #region Variable
        public float m_from = 0;
        public float m_to = 1;
        public float m_origin = 0;
        public float m_totalTime;
        public bool m_pinpon;
        public bool m_loop;
        public bool m_autoPlay = true;
        public bool m_work = false;
        public string m_name;
        public Renderer r;
        public Image i;
        private Material m;
        private float m_cur;
        private float m_timer = 0;

        public float TotalTime { get => m_totalTime; set => m_totalTime = value; }
        #endregion

        #region MonoBehaviour
        // Use this for initialization
        void Start()
        {
            init();
        }

        // Update is called once per frame
        void Update()
        {
            if (m_work)
            {
                m_timer += Time.deltaTime;
                if (m_timer > m_totalTime)
                {
                    if (m_loop == true)
                    {
                        m_timer = 0;
                    }
                    else
                    {
                        m_work = false;
                    }
                }
                float ratio = Mathf.InverseLerp(0, m_totalTime, m_timer);
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
                m_cur = (float)Common.Lerp(m_from, m_to, ratio);
                setValue(m_cur);
            }
        }
        void setValue(float v)
        {
            m.SetFloat(m_name, v);
        }
        #endregion

        #region public method
        public void start()
        {
            if (m_work) return;
            m_timer = 0;
            m_work = true;
        }
        public void init()
        {
            if (m_pinpon == true)
            {
                m_totalTime *= 2;
            }
            if (m_autoPlay)
            {
                start();
            }

            if (r) m = r.material;
            if (i) m = i.material;
        }

        public void reset()
        {
            if (r) m = r.material;
            if (i) m = i.material;
            setValue(m_origin);
            m_work = false;
            if (m_autoPlay)
            {
                start();
            }
        }

        public bool isWorking()
        {
            return m_work;
        }
        
        public MaterialTweenProcData GetProcData()
        {
            return new MaterialTweenProcData(this);
        }

        public void SetProcData([ReadOnly] ref MaterialTweenProcData data)
        {
            m_timer = data.timer;
            if (data.work)
                setValue(data.curValue);
        }
        #endregion

        public struct MaterialTweenProcData
        {
            [WriteOnly]
            public float timer;
            [ReadOnly]
            public float totalTime;
            [ReadOnly]
            public bool pinpon;
            [ReadOnly]
            public bool loop;
            [ReadOnly]
            public bool work;
            [ReadOnly]
            public float from;
            [ReadOnly]
            public float to;
            [WriteOnly]
            public float curValue;

            public MaterialTweenProcData(MaterialTween mt)
            {
                timer = mt.m_timer;
                totalTime = mt.m_totalTime;
                curValue = 0;
                pinpon = mt.m_pinpon;
                loop = mt.m_loop;
                work = mt.m_work;
                from = mt.m_from;
                to = mt.m_to;
            }
        }

        static public void UpdateProcData(ref MaterialTweenProcData data, float dt)
        {
            if (data.work)
            {
                data.timer += dt;
                if (data.timer > data.totalTime)
                {
                    if (data.loop == true)
                    {
                        data.timer = 0;
                    }
                    else
                    {
                        data.work = false;
                    }
                }
                float ratio = Mathf.InverseLerp(0, data.totalTime, data.timer);
                if (data.pinpon == true)
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
                data.curValue = (float)Common.Lerp(data.from, data.to, ratio);
            }
        }
    }
}

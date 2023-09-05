using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace CommonUnit
{
    public class ColorTween : MonoBehaviour
    {
        #region Variable
        public Color m_from = Color.white;
        public Color m_to = Color.white;
        public float m_totalTime;
        public float m_fFrom = 0;
        public float m_fTo = 1;
        public bool m_pinpon;
        public bool m_loop;
        public bool m_autoPlay = true;
        public bool m_work = false;
        public bool Intensity = false;
        public Text m_text;
        public SpriteRenderer m_renderer;
        public Graphic m_uGUIGraphic;
        public Light2D m_light;
        private Color m_cur;
        private Color m_origin = Color.white;
        private float m_timer = 0;
        private float m_fOrigin = 0;
        private float m_fCur = 0;
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
                m_cur = Color.Lerp(m_from, m_to, ratio);
                m_fCur = (float)Common.Lerp(m_fFrom, m_fTo, ratio);
                setColor(m_cur);
                if (Intensity)
                {
                    setIntensity(m_fCur);
                }
            }
        }
        void setColor(Color color)
        {
            if (m_renderer != null)
            {
                m_renderer.color = color;
            }
            else if (m_uGUIGraphic != null)
            {
                m_uGUIGraphic.color = color;
            }
            else if (m_text != null)
            {
                m_text.color = color;
            }
            else if (m_light != null)
            {
                m_light.color = color;
            }
        }

        void setIntensity(float intensity)
        {
            if (m_light)
            {
                m_light.intensity = intensity;
            }
        }
        #endregion

        #region public method
        public void start()
        {
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
        }

        public void reset()
        {
            setColor(m_origin);
            m_work = false;
            if (m_autoPlay)
            {
                start();
            }
            setIntensity(m_fOrigin);
        }

        public bool isWorking()
        {
            return m_work;
        }
        #endregion
    }
}

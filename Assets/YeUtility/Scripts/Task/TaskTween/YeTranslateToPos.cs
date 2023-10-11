using System;
using System.Collections;
using UnityEngine;

namespace YeUtility.Task.TaskTween
{
    public class YeTranslateToPos : MonoBehaviour {

        public System.Action OnArrive;

        #region Variable
        public Vector3 m_targetPos = Vector3.zero;

        public float m_soomthTime = 0.033f;
        public float m_deltaTime = 0;
        public bool useFixedUpdate = true;

        private bool useLocal = false;
        private bool finish = false;
        private Transform m_trans;
        private Vector3 m_currentVelocity = Vector3.zero;
        private Vector3 m_originPos = Vector3.zero;
        private TaskLauncher m_tasks = new TaskLauncher();
        #endregion

        #region Mono
        void Start()
        {
            init();
        }
        void Update()
        {
            if (useFixedUpdate == false)
            {
                m_deltaTime = Time.deltaTime;
                m_tasks.update(Time.deltaTime);
            }
            else
            {
                m_deltaTime = Time.fixedDeltaTime;
                m_tasks.update(Time.fixedDeltaTime);
            }
        }
        #endregion

        #region public method 
        [ContextMenu("init")]
        public void init()
        {
            init(true);
            finish = false;
        }

        public void init(bool local)
        {
            useLocal = local;
            m_trans = transform;
            m_originPos = getPos();
            m_tasks.start();
        }

        public void forceToEnd()
        {
            setPos(m_targetPos);
        }

        public void forceToStart()
        {
            setPos(m_originPos);
        }
    
        public IEnumerator playProc()
        {
            yield return moveTo(m_targetPos);
            finish = true;
        }

        public IEnumerator rewindProc()
        {
            yield return moveTo(m_originPos);
            finish = false;
        }

        [ContextMenu("Switch")]
        public void Switch()
        {
            if (finish)
                rewind();
            else
                play();
        }

        [ContextMenu("Play")]
        public void play()
        {
            m_tasks.AddTask(moveTo(m_targetPos, () => finish = true));
        }

        [ContextMenu("rewind")]
        public void rewind()
        {
            m_tasks.AddTask(moveTo(m_originPos, () => finish = false));
        }

        [ContextMenu("anchoredPosition3D")]
        public void anchoredPosition3D()
        {
            init(true);
            Debug.Log(getPos().ToString("f3"));
        }
        #endregion

        #region private
        Vector3 getPos()
        {
            if (m_trans == null) return Vector3.zero;

            if (useLocal)
            {
                if (m_trans is RectTransform)
                {
                    return (m_trans as RectTransform).anchoredPosition3D;
                }
                return m_trans.localPosition;
            }
            else
            {
                return m_trans.position;
            }
        }

        void setPos(Vector3 pos)
        {
            if (m_trans == null) return;

            if (useLocal)
            {
                if (m_trans is RectTransform)
                {
                    (m_trans as RectTransform).anchoredPosition3D = pos;
                }
                else
                {
                    m_trans.localPosition = pos;
                }
            }
            else
            {
                m_trans.position = pos;
            }
        }
        #endregion

        #region Tasks
        IEnumerator moveTo(Vector3 target, Action onComplete = null)
        {
            if (m_tasks == null) yield break;
            while ( (target - getPos()).sqrMagnitude > 0.0001f)
            {
                setPos(Vector3.SmoothDamp(getPos(), target, ref m_currentVelocity, m_soomthTime, 100000, deltaTime:m_deltaTime));
                yield return 0;
            }
            setPos(target);
            OnArrive?.Invoke();
            onComplete?.Invoke();
        }
        #endregion
    }
}

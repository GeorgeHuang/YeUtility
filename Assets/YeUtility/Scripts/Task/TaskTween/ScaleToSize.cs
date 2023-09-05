using UnityEngine;
using System.Collections;
using CommonUnit;

public class ScaleToSize : MonoBehaviour {

    public System.Action OnArrive;

    #region Variable
    public Vector3 m_targetSize = Vector3.one;

    public float m_soomthTime = 0.15f;
    public bool m_useFixedTime = true;

    private float m_deltaTime = 0;
    private Transform m_trans;
    private Vector3 m_currentVelocity = Vector3.zero;
    private Vector3 m_originSize = Vector3.one;
    private TaskLauncher m_tasks = new TaskLauncher();
    #endregion

    #region Mono
    void Update()
    {
        if (m_useFixedTime)
        {
            m_deltaTime = Time.fixedDeltaTime;
        }
        else
        {
            m_deltaTime = Time.deltaTime;
        }
        m_tasks.update(m_deltaTime);
    }
    #endregion

    #region public method 
    [ContextMenu("Play")]
    public void play()
    {
        m_tasks.AddTask(scaleTo(m_targetSize));
    }

    [ContextMenu("rewind")]
    public void rewind()
    {
        m_tasks.AddTask(scaleTo(m_originSize));
    }
    [ContextMenu("init")]
    public void init()
    {
        m_trans = transform;
        m_originSize = transform.localScale;
        m_tasks.start();
    }

    public void forceToEnd()
    {
        m_trans.localPosition = m_targetSize;
    }

    public void forceToStart()
    {
        m_trans.localPosition = m_originSize;
    }
    
    public IEnumerator playProc()
    {
        yield return scaleTo(m_targetSize);
    }

    public IEnumerator rewindProc()
    {
        yield return scaleTo(m_originSize);
    }
    #endregion

    #region Tasks
    IEnumerator scaleTo(Vector3 target)
    {
        while ( (target - m_trans.localScale).sqrMagnitude > 0.0001f)
        {
            m_trans.localScale = Vector3.SmoothDamp(m_trans.localScale, target, ref m_currentVelocity, m_soomthTime, 10000000, m_deltaTime);
            yield return 0;
        }
        m_trans.localScale = target;
        if (OnArrive != null)
        {
            OnArrive();
        }
    }
    #endregion
}

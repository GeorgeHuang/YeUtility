using System.Collections.Generic;
using UnityEngine;

namespace YeUtility
{
    /// <summary>
    /// prefab must have unique name
    /// </summary>
    public class ReuseManager : IReusePool
    {
        public System.Action<Object> OnObjectBack;

        public bool verbose = false;

        public int MaxNumber = 100000;

        #region Variable
        Dictionary<string, Queue<Object>> m_dict = new Dictionary<string, Queue<Object>>();
        Dictionary<string, int> m_numberDict = new Dictionary<string, int>();
        Dictionary<Object, string> m_parentTable = new Dictionary<Object, string>();
        GameObject m_parent;
        #endregion

        public UnityEngine.GameObject Parent
        {
            get { return m_parent; }
            set { m_parent = value; }
        }

        #region public method
        public int Count(Object prefab)
        {
            if (m_dict.ContainsKey(prefab.name) == false)
                return 0;
            return m_numberDict[prefab.name];
        }
        public Object get(Object prefab)
        {
            Object rv = null;

            //檢查是否為新登錄的物件
            if (m_dict.ContainsKey(prefab.name) == false)
            {
                m_dict.Add(prefab.name, new Queue<Object>());
                m_numberDict.Add(prefab.name, 1);
                rv = Object.Instantiate(prefab);
                m_parentTable.Add(rv, prefab.name);
                if (verbose)
                    Common.SysPrint("get Name: " + rv.name + " ID: " + rv.GetInstanceID() + " is new");
            }
            else
            {
                //已登錄但未有閒置物置
                if (m_dict[prefab.name].Count == 0)
                {
                    if (m_numberDict[prefab.name] >= MaxNumber) return rv;
                    rv = Object.Instantiate(prefab);
                    m_numberDict[prefab.name] += 1;
                    m_parentTable.Add(rv, prefab.name);
                    if (verbose)
                        Common.SysPrint("get Name: " + rv.name + " ID: " + rv.GetInstanceID() + " is new");
                }
                else
                {
                    rv = m_dict[prefab.name].Dequeue();
                    if (verbose)
                        Common.SysPrint("get Name: " + rv.name + " ID: " + rv.GetInstanceID() + " is old");
                }
            }
            //Common.sysPrint("get Name: " + rv.name + " ID: " + rv.GetInstanceID() + " is new");
            return rv;
        }

        public void back(Object obj)
        {
            if (m_parentTable.ContainsKey(obj) == true)
            {
#if YEDEBUG
                if (m_dict[m_parentTable[obj]].Contains(obj))
                {
                    Debug.LogError("Name: " + obj.name + " Back Twice");
                    return;
                }
#endif

                m_dict[m_parentTable[obj]].Enqueue(obj);
                if (OnObjectBack != null)
                {
                    OnObjectBack(obj);
                }
            }
            else
            {
                Common.SysPrint("Name: " + obj.name + " not found in ParentTable");
            }
        }

        public void clear()
        {
            m_numberDict.Clear();
            m_dict.Clear();
            m_parentTable.Clear();
        }

        public Object Get(Object prefab)
        {
            return get(prefab);
        }

        public T Get<T>(T prefab) where T : Object
        {
            return get(prefab) as T;
        }

        public void Back(Object obj)
        {
            back(obj);
        }
        #endregion
    }
}

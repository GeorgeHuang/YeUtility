using System;
using System.Collections.Generic;

namespace CommonUnit
{
    /// <summary>
    /// prefab must have unique name
    /// </summary>
    public class ReuseManagerSysVer 
    {
        public Action<object> OnObjectBack;

        public bool verbose = false;

        public int MaxNumber = 100000;

        #region Variable
        Dictionary<Type, Queue<object>> IdleObjectRepo = new Dictionary<Type, Queue<object>>();
        #endregion

        #region public method
        public object Get<T>()
        {
            var type = typeof(T);
            //已登錄過
            if (IdleObjectRepo.ContainsKey(type))
            {

            }
            //新登錄
            else
            {
                IdleObjectRepo.Add(type, new Queue<object>());
            }
            return null;
        }
        //public Object get(Object prefab)
        //{
        //    Object rv = null;

        //    //檢查是否為新登錄的物件
        //    if (m_dict.ContainsKey(prefab.name) == false)
        //    {
        //        m_dict.Add(prefab.name, new Queue<Object>());
        //        m_numberDict.Add(prefab.name, 1);
        //        rv = MonoBehaviour.Instantiate(prefab);
        //        m_parentTable.Add(rv, prefab.name);
        //        if (verbose)
        //            Common.sysPrint("get Name: " + rv.name + " ID: " + rv.GetInstanceID() + " is new");
        //    }
        //    else
        //    {
        //        //已登錄但未有閒置物置
        //        if (m_dict[prefab.name].Count == 0)
        //        {
        //            if (m_numberDict[prefab.name] >= MaxNumber) return rv;
        //            rv = MonoBehaviour.Instantiate(prefab);
        //            m_numberDict[prefab.name] += 1;
        //            m_parentTable.Add(rv, prefab.name);
        //            if (verbose)
        //                Common.sysPrint("get Name: " + rv.name + " ID: " + rv.GetInstanceID() + " is new");
        //        }
        //        else
        //        {
        //            rv = m_dict[prefab.name].Dequeue();
        //            if (verbose)
        //                Common.sysPrint("get Name: " + rv.name + " ID: " + rv.GetInstanceID() + " is old");
        //        }
        //    }
        //    //Common.sysPrint("get Name: " + rv.name + " ID: " + rv.GetInstanceID() + " is new");
        //    return rv;
        //}

        //public void back(Object obj)
        //{
        //    if (m_parentTable.ContainsKey(obj) == true)
        //    {
        //        if (m_dict[m_parentTable[obj]].Contains(obj))
        //        {
        //            Debug.LogError("Name: " + obj.name + " Back Twice");
        //            return;
        //        }
        //        m_dict[m_parentTable[obj]].Enqueue(obj);
        //        if (OnObjectBack != null)
        //        {
        //            OnObjectBack(obj);
        //        }
        //    }
        //    else
        //    {
        //        Common.sysPrint("Name: " + obj.name + " not found in ParentTable");
        //    }
        //}

        //public void clear()
        //{
        //    m_dict.Clear();
        //    m_parentTable.Clear();
        //}

        //public Object Get(Object prefab)
        //{
        //    return get(prefab);
        //}

        //public T Get<T>(T prefab) where T : Object
        //{
        //    return get(prefab) as T;
        //}

        //public void Back(Object obj)
        //{
        //    back(obj);
        //}
        #endregion
    }
}

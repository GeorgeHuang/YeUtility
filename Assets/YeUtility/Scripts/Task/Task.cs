using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace CommonUnit
{
    public partial class Task : IPoolable, IDisposable
    {
        static readonly PoolableStaticMemoryPool<Task> pool =
            new PoolableStaticMemoryPool<Task>();

        #region Variable
        State curState = State.Idle;
        IEnumerator mainTask;
        float timer = 0;
        float waitTime = 0;
        Task childTask;
        AsyncOperation asyncObj;

        protected float delayStart = 0;
        #endregion

        #region GetSet
        public State CurState
        {
            get
            {
                return curState;
            }
        }
        #endregion

        public enum State
        {
            Idle,
            Working,
            Pause,
            Wait,
            WaitAsync,
            Extension,
            End
        }

        public Task()
        {
        }

        public void SetMainTask(IEnumerator task)
        {
            mainTask = task;
        }

        #region public method
        public virtual void update(float deltaTime)
        {
            switch (curState)
            {
                case State.Working:
                    if (mainTask.MoveNext() == false)
                    {
                        setState(State.End);
                    }
                    else
                    {
                        var obj = mainTask.Current;
                        if (obj is float)
                        {
                            waitTime = (float)obj;
                            timer = 0;
                            setState(State.Wait);
                        }
                        else if (obj is int)
                        {
                            waitTime = (float)(int)obj;
                            if (waitTime >= 0)
                            {
                                timer = 0;
                                setState(State.Wait);
                            }
                        }
                        else if (obj is Task)
                        {
                            childTask = obj as Task;
                            childTask.start();
                            setState(State.Extension);
                        }
                        else if (obj is IEnumerator)
                        {
                            childTask = Get(obj as IEnumerator);
                            childTask.start();
                            setState(State.Extension);
                        }
                        else if (obj is AsyncOperation)
                        {
                            asyncObj = obj as AsyncOperation;
                            setState(State.WaitAsync);
                        }
                    }
                    break;
                case State.Extension:
                    childTask.update(deltaTime);
                    if (childTask.CurState == State.End)
                    {
                        childTask = null;
                        setState(State.Working);
                    }
                    break;
                case State.Wait:
                    timer += deltaTime;
                    if (timer > waitTime)
                    {
                        timer = 0;
                        setState(State.Working);
                    }
                    break;
                case State.WaitAsync:
                    {
                        if (asyncObj.isDone)
                        {
                            setState(State.Working);
                        }
                    }
                    break;
            }
        }

        public void setState(State state)
        {
            curState = state;
        }

        public virtual void start()
        {
            timer = 0;
            if (delayStart > 0)
            {
                waitTime = delayStart;
                setState(State.Wait);
            }
            else
            {
                setState(State.Working);
                onWorking();
            }
        }
        public void OnDespawned()
        {
        }

        public void OnSpawned()
        {
        }
        // ReSharper disable Unity.PerformanceAnalysis
        virtual public void Dispose()
        {
            pool.Despawn(this);
        }
        #endregion

        #region protected
        protected virtual void onWorking()
        {

        }
        #endregion

        #region Static Method
        static public Task Get(IEnumerator task)
        {
            var newTask = pool.Spawn();
            newTask.SetMainTask(task);
            return newTask;
        }
        #endregion
    }
}

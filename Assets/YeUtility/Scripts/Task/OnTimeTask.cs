using System;
using UnityEngine;
using Zenject;
using OnTimeTaskClass = YeUtility.Task.OnTimeTask;

namespace YeUtility.Task
{
    public class OnTimeTask : Task, IPoolable, IDisposable
    {

        static readonly PoolableStaticMemoryPool<OnTimeTask> onTimeTaskPool =
            new PoolableStaticMemoryPool<OnTimeTask>();

        float timer = 0;
        float totalTime = 0;
        Action mainTask;

        Action<float> setTimerFun;

        public OnTimeTask()
        {

        }

        static public OnTimeTask Get()
        {
            return onTimeTaskPool.Spawn();
        }

        public override void Dispose()
        {
            timer = 0;
            totalTime = 0;
            mainTask = null;

            setTimerFun = null;
            onTimeTaskPool.Despawn(this);
        }

        public OnTimeTask Setup(Action task, float totalTime, Action<float> setTimerFun)
        {
            mainTask = task;
            this.totalTime = totalTime;
            this.setTimerFun = setTimerFun;
            return this;
        }


        public override void update(float deltaTime)
        {
            if (CurState == State.Working)
            {
                setTimerFun?.Invoke(timer);
                timer += deltaTime;
                if (timer >= totalTime)
                {
                    if (mainTask != null)
                    {
                        mainTask();
                    }
                    setState(State.End);
                }
            }
        }

        internal float GetRemainTime()
        {
            return totalTime - timer;
        }

        public void SetTimePosNormal(float v)
        {
            v = Mathf.Clamp01(v);
            var newTime = totalTime * v;
            //Debug.Log($"new time {newTime}, org time {timer}");
            if (timer < newTime)
            {
                timer = newTime;
            }
        }
    }

    public partial class Task
    {
        public static OnTimeTask OnTimeTask(Action task, float totalTime)
        {
            var newTask = OnTimeTaskClass.Get();
            return newTask.Setup(task, totalTime, null);
        }
        public static OnTimeTask OnTimeTask(Action task, float totalTime, Action<float> setTimerFun)
        {
            var newTask = OnTimeTaskClass.Get();
            return newTask.Setup(task, totalTime, setTimerFun);
        }
    }
}

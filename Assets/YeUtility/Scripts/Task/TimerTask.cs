using System;
using Zenject;
using TimerTaskClass = CommonUnit.TimerTask;

namespace CommonUnit
{
    public class TimerTask : Task, IPoolable, IDisposable
    {

        static readonly PoolableStaticMemoryPool<TimerTask> TimerTaskPool = new();

        float timer = 0;
        float totalTime = 0;
        Action mainTask = null;
        Action<float> timeTask = null;

        static public TimerTask Get()
        {
            return TimerTaskPool.Spawn();
        }

        public override void Dispose()
        {
            timer = 0;
            mainTask = null;
            timeTask = null;
            TimerTaskPool.Despawn(this);
        }

        public float GetRemainTime()
        {
            return totalTime - timer;
        }

        public TimerTask()
        {

        }

        public TimerTask Setup(Action<float> task, float totalTime)
        {
            timeTask = task;
            this.totalTime = totalTime;
            return this;
        }
        public TimerTask Setup(Action task, float totalTime)
        {
            mainTask = task;
            this.totalTime = totalTime;
            return this;
        }


        public override void start()
        {
            base.start();
            timer = 0;
        }

        public override void update(float deltaTime)
        {
            if (CurState == State.Working)
            {
                mainTask?.Invoke();
                timeTask?.Invoke(timer);
                timer += deltaTime;
                if (timer >= totalTime)
                {
                    setState(State.End);
                }
            }
        }
    }
    public partial class Task
    {
        public static TimerTask TimerTask(Action task, float totalTime)
        {
            var newTask = TimerTaskClass.Get();
            return newTask.Setup(task, totalTime);
        }
        public static TimerTask TimerTask(Action<float> task, float totalTime)
        {
            var newTask = TimerTaskClass.Get();
            return newTask.Setup(task, totalTime);
        }
    }
}

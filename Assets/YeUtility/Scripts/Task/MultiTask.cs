using System;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using MultiTaskClass = YeUtility.Task.MultiTask;

namespace YeUtility.Task
{
    public class MultiTask : Task, IPoolable, IDisposable
    {
        static readonly PoolableStaticMemoryPool<MultiTask> multiTaskPool =
            new PoolableStaticMemoryPool<MultiTask>();
        List<Task> mainTasks = new List<Task>();

        #region constructor
        public MultiTask()
        {
        }
        static public MultiTask Get()
        {
            return multiTaskPool.Spawn();
        }

        public override void Dispose()
        {
            multiTaskPool.Despawn(this);
        }

        public void Setup(float delayStart)
        {
            this.delayStart = delayStart;
        }

        public void Setup(IEnumerator[] tasks)
        {
            if (tasks != null)
            {
                for (int i = 0, size = tasks.Length; i < size; ++i)
                {
                    mainTasks.Add(Task.Get(tasks[i]));
                }
            }
        }

        public void Setup(List<IEnumerator> tasks)
        {
            if (tasks != null)
            {
                for (int i = 0, size = tasks.Count; i < size; ++i)
                {
                    mainTasks.Add(Task.Get(tasks[i]));
                }
            }
        }
        #endregion
        public override void update(float deltaTime)
        {
            if (CurState == State.Working)
            {
                bool AllEnd = true;
                for (int i = 0, size = mainTasks.Count; i < size; ++i)
                {
                    Task task = mainTasks[i];
                    task.update(deltaTime);
                    if (task.CurState != State.End)
                    {
                        AllEnd = false;
                    }
                }
                if (AllEnd)
                {
                    setState(State.End);
                }
            }
        }

        protected override void onWorking()
        {
            for (int i = 0, size = mainTasks.Count; i < size; ++i)
            {
                mainTasks[i].start();
            }
        }
        public void AddTask(IEnumerator newTask)
        {
            mainTasks.Add(Task.Get(newTask));
        }
    }
    public partial class Task
    {
        public static MultiTask MultiTask()
        {
            return MultiTaskClass.Get();
        }
    }
}

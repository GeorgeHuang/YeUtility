using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YeUtility.Task
{
    public partial class TaskLauncher : ITaskLauncher
    {

        Queue<Task> taskPipe = new Queue<Task>();
        Task curTask;
        State curState = State.Idle;

        private int verbose = 0;

        public int Verbose { get => verbose; set => verbose = value; }
        public int Count { get => taskPipe.Count + (curTask == null ? 0 : 1); }

        enum State
        {
            Idle,
            Woking,
            Pause,
            End
        }

        public void start()
        {
            setState(State.Woking);
        }

        public float GetRemainTime()
        {
            float rv = 0;
            if (curTask is TimerTask)
            {
                rv = (curTask as TimerTask).GetRemainTime();
            }
            else if (curTask is OnTimeTask)
            {
                rv = (curTask as OnTimeTask).GetRemainTime();
            }
            return rv;
        }

        public void update(float deltaTime)
        {
            if (curState == State.Woking)
            {
                if (curTask == null && taskPipe.Count == 0)
                    return;

                if (curTask == null)
                {
                    curTask = taskPipe.Dequeue();
                    curTask.start();
                }
                else
                {
                    curTask.update(deltaTime);
                    if (curTask == null)
                    {
                        //when task do clear taskLauncher
                        return;
                    }
                    if (curTask.CurState == Task.State.End)
                    {
                        curTask.Dispose();
                        curTask = null;
                        if (taskPipe.Count > 0)
                        {
                            curTask = taskPipe.Dequeue();
                            curTask.start();
                        }
                    }
                }
            }
            else if (verbose > 0)
            {
                if (taskPipe.Count != 0)
                {
                    Debug.Log("Has Task But Not Working");
                }
            }
        }

        public void pause()
        {
            setState(State.Pause);
        }

        public void resume()
        {
            setState(State.Woking);
        }

        public void clear()
        {
            taskPipe.Clear();
            curTask?.Dispose();
            curTask = null;
            //setState(State.Idle);
        }

        public void ClearButCurrent()
        {
            taskPipe.Clear();
        }

        public int getTaskNumber()
        {
            int number = 0;
            if (curTask != null)
            {
                number = 1;
            }
            number += taskPipe.Count;
            return number;
        }


        void setState(State state)
        {
            curState = state;
        }

        public bool HasTask()
        {
            return curTask != null;
        }

        public Task AddTask(IEnumerator task, bool autoStart = true)
        {
            //var newTask = new Task(task);
            var newTask = Task.Get(task);
            taskPipe.Enqueue(newTask);
            if (autoStart && curState != State.Woking)
                start();
            return newTask;
        }
        public Task AddTask(Task newTask, bool autoStart = true)
        {
            //var newTask = new Task(task);
            taskPipe.Enqueue(newTask);
            if (autoStart && curState != State.Woking)
                start();
            return newTask;
        }
    }
}

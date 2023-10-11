using System.Collections;
using System.Collections.Generic;

namespace YeUtility.Task
{
    public class MultiTaskLauncher : ITaskLauncher
    {
        List<TaskLauncher> taskPipes = new List<TaskLauncher>();

        public void update(float dt)
        {
            //taskPipes.ForEach(x => x.update(dt));
            for (int i = 0; i < taskPipes.Count; ++i)
            {
                taskPipes[i].update(dt);
            }
        }
        public Task AddTask(IEnumerator task, bool autoStart = true)
        {
            return getEmptyLuncher().AddTask(task, autoStart);
        }

        public Task AddTask(Task task, bool autoStart = true)
        {
            return getEmptyLuncher().AddTask(task, autoStart);
        }

        public void clear()
        {
            taskPipes.ForEach(x => x.clear());
        }

        TaskLauncher getEmptyLuncher()
        {
            TaskLauncher rv = null;
            foreach (var tasks in taskPipes)
            {
                if (tasks.getTaskNumber() == 0)
                {
                    rv = tasks;
                }
            }

            if (rv == null)
            {
                rv = new TaskLauncher();
                rv.start();
                taskPipes.Add(rv);
            }
            return rv;
        }
    }
}

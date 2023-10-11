using System.Collections;

namespace YeUtility.Task
{
    public partial interface ITaskLauncher
    {
        public Task AddTask(IEnumerator task, bool autoStart = true);
        public Task AddTask(Task task, bool autoStart = true);
    }
}

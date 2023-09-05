using System.Collections;

namespace CommonUnit
{
    public partial interface ITaskLauncher
    {
        public Task AddTask(IEnumerator task, bool autoStart = true);
        public Task AddTask(Task task, bool autoStart = true);
    }
}

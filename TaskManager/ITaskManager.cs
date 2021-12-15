using System.Collections.Generic;

namespace TaskManager
{
    public interface ITaskManager
    {
        void Add(Process process);
        IList<Process> ListRunningProcesses(SortOrder order);
        void Kill(Process process);
        void KillGroup(TaskPriority priority);
        void KillAll();
    }
}
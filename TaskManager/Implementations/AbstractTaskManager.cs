using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace TaskManager
{
    public abstract class AbstractTaskManager : ITaskManager
    {

        protected readonly int capacity;
        protected IEnumerable<TaskElement> processes;

        public AbstractTaskManager(int capacity)
        {
            this.capacity = capacity;
        }
        public abstract void Add(Process process);

        public abstract void InitializeProcesses();

        public IList<Process> ListRunningProcesses(SortOrder order = SortOrder.TimeOfCreation)
        {
            switch (order)
            {
                case SortOrder.TimeOfCreation:
                    return processes.OrderBy(x => x.CreatedAt).Select(x => x.Process).ToList();
                case SortOrder.Priority:
                    return processes.OrderBy(x => x.Process.Priority).ThenBy(x => x.CreatedAt).Select(x => x.Process).ToList();
                case SortOrder.Id:
                    return processes.OrderBy(x => x.Process.PID).Select(x => x.Process).ToList();
                default:
                    return processes.OrderBy(x => x.CreatedAt).Select(x => x.Process).ToList();
            }
        }

        public void Kill(Process process)
        {
            var taskElement = processes.Where(x => x.Process == process).ToList();
            processes = processes.Except(taskElement);
        }

        public void KillGroup(TaskPriority priority)
        {
            var taskElement = processes.Where(x => x.Process.Priority == priority).ToList();
            processes = processes.Except(taskElement);
        }

        public void KillAll()
        {
            foreach (var taskElement in processes)
            {
                taskElement.Process.Kill();
            }
            InitializeProcesses();
        }
    }
}
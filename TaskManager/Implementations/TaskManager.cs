using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace TaskManager
{
    public class TaskManager : AbstractTaskManager
    {
        public TaskManager(int capacity) : base(capacity)
        {
            InitializeProcesses();
        }

        public override void Add(Process process)
        {
            if (this.processes.Count() == this.capacity)
            {
                throw new TaskManagerException("Task Manager capacity exceeded");
            }
            ((IList)processes).Add(process.ToTaskElement());
        }

        public sealed override void InitializeProcesses()
        {
            processes = new List<TaskElement>(capacity);
        }
    }
}
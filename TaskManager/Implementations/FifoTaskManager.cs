using System.Collections.Generic;
using System.Linq;

namespace TaskManager
{
    public class FifoTaskManager : AbstractTaskManager
    {
        public FifoTaskManager(int capacity) : base(capacity)
        {
            InitializeProcesses();
        }

        public override void Add(Process process)
        {
            if (this.processes.Count() == this.capacity)
            {
                var oldProcess = ((Queue<TaskElement>)processes).Dequeue();
                oldProcess.Process.Kill();
            }
            ((Queue<TaskElement>)processes).Enqueue(process.ToTaskElement());
        }

        public sealed override void InitializeProcesses()
        {
            processes = new Queue<TaskElement>(capacity);
        }
    }
}
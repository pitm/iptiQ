using System.Collections;
using System.Linq;

namespace TaskManager
{
    public class PriorityBasedTaskManager : TaskManager
    {
        public PriorityBasedTaskManager(int capacity) : base(capacity)
        {
        }

        public override void Add(Process process)
        {
            if (this.processes.Count() == this.capacity && !RemoveTaskWithLowerPriority(process.Priority))
            {
                return;
            }
            ((IList)processes).Add(process.ToTaskElement());

        }

        private bool RemoveTaskWithLowerPriority(TaskPriority priority)
        {
            var task = processes.Where(x=>x.Process.Priority<priority).OrderBy(x=>x.CreatedAt).FirstOrDefault();
            if (task != null)
            {
                task.Process.Kill();
                ((IList)processes).Remove(task);
                return true;
            }
            return false;
        }
    }
}
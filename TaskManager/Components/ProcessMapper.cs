using System;

namespace TaskManager
{
    public static class ProcessMapper
    {  
        public static TaskElement ToTaskElement(this Process process)
        {
            return new TaskElement() {CreatedAt = DateTime.Now, Process = process};
        }
    }
}
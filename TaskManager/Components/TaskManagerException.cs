using System;

namespace TaskManager
{
    public class TaskManagerException : Exception
    {
        public TaskManagerException(string message) : base(message)
        {
        }
    }
}
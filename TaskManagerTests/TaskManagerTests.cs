using System;
using System.Linq;
using NUnit.Framework;
using TaskManager;
using Moq;

namespace TaskManagerTests
{
    public class TaskManagerTests
    {
        [Test]
        public void Add_shouldAcceptNewProcess()
        {
            // Arrange
            var taskManager = new TaskManager.TaskManager(1);
            var process = new Process();

            // Act
            taskManager.Add(process);

            // Assert
            Assert.AreEqual(1, taskManager.ListRunningProcesses().Count);
            CollectionAssert.Contains(taskManager.ListRunningProcesses(), process);
        }

        [Test]
        public void Add_shouldAcceptNewProcessUntilMaxCapacityIsReached()
        {
            // Arrange
            var taskManager = new TaskManager.TaskManager(1);
            taskManager.Add(new Process());

            // Act
            TestDelegate execute = delegate { taskManager.Add(new Process()); };

            // Assert
            Assert.Throws<TaskManagerException>(execute);
        }

        [Test]
        public void List_ShallReturnOrderedListOfProcessesByCreationDate()
        {
            // Arrange
            var taskManager = new TaskManager.TaskManager(5);
            taskManager.Add(new Process() { PID = 1 });
            taskManager.Add(new Process() { PID = 2 });
            taskManager.Add(new Process() { PID = 3 });
            taskManager.Add(new Process() { PID = 4 });

            // Act
            var list = taskManager.ListRunningProcesses();

            // Assert
            Assert.AreEqual(1, list[0].PID);
            Assert.AreEqual(2, list[1].PID);
            Assert.AreEqual(3, list[2].PID);
            Assert.AreEqual(4, list[3].PID);
        }

        [Test]
        public void ListById_ShallReturnOrderedListByPID()
        {
            // Arrange
            var taskManager = new TaskManager.TaskManager(5);
            taskManager.Add(new Process() { PID = 2 });
            taskManager.Add(new Process() { PID = 4 });
            taskManager.Add(new Process() { PID = 3 });
            taskManager.Add(new Process() { PID = 1 });

            // Act
            var list = taskManager.ListRunningProcesses(SortOrder.Id);

            // Assert
            Assert.AreEqual(1, list[0].PID);
            Assert.AreEqual(2, list[1].PID);
            Assert.AreEqual(3, list[2].PID);
            Assert.AreEqual(4, list[3].PID);
        }

        [Test]
        public void ListById_ShallReturnOrderedListByPriority()
        {
            // Arrange
            var taskManager = new TaskManager.TaskManager(5);
            taskManager.Add(new Process() { PID = 2, Priority = TaskPriority.Low });
            taskManager.Add(new Process() { PID = 4, Priority = TaskPriority.Medium });
            taskManager.Add(new Process() { PID = 3, Priority = TaskPriority.High });
            taskManager.Add(new Process() { PID = 1, Priority = TaskPriority.Low });

            // Act
            var list = taskManager.ListRunningProcesses(SortOrder.Priority);

            // Assert
            Assert.AreEqual(2, list[0].PID);
            Assert.AreEqual(1, list[1].PID);
            Assert.AreEqual(4, list[2].PID);
            Assert.AreEqual(3, list[3].PID);
        }

        [Test]
        public void Kill_ShouldKillSpecifiedProcess()
        {
            // Arrange
            var taskManager = new TaskManager.TaskManager(5);
            var process1 = new Process() { PID = 3, Priority = TaskPriority.High };
            var process2 = new Process() { PID = 4, Priority = TaskPriority.Medium };
            taskManager.Add(process1);
            taskManager.Add(process2);

            // Act
            taskManager.Kill(process1);

            // Assert
            Assert.AreEqual(1, taskManager.ListRunningProcesses().Count);
        }

        [Test]
        public void KillGroup_ShouldKillAllProcesseswithASpecificPriority()
        {
            // Arrange
            var taskManager = new TaskManager.TaskManager(5);
            var process1 = new Process() { PID = 3, Priority = TaskPriority.High };
            var process2 = new Process() { PID = 4, Priority = TaskPriority.Medium };
            taskManager.Add(process1);
            taskManager.Add(process2);

            // Act
            taskManager.KillGroup(TaskPriority.Medium);

            // Assert
            Assert.AreEqual(1, taskManager.ListRunningProcesses().Count);
            Assert.AreEqual(3, taskManager.ListRunningProcesses().First().PID);
        }

        [Test]
        public void KillAll_ShouldKillAllProcesses()
        {
            // Arrange
            var taskManager = new TaskManager.TaskManager(5);
            var process1 = new Process() { PID = 3, Priority = TaskPriority.High };
            var process2 = new Process() { PID = 4, Priority = TaskPriority.Medium };
            taskManager.Add(process1);
            taskManager.Add(process2);

            // Act
            taskManager.KillAll();

            // Assert
            Assert.AreEqual(0, taskManager.ListRunningProcesses().Count);
        }
    }
}
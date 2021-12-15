using System;
using System.Linq;
using NUnit.Framework;
using TaskManager;

namespace TaskManagerTests
{
    public class FifoTaskManagerTests
    {
        [Test]
        public void Add_shouldAcceptNewProcess()
        {
            // Arrange
            var taskManager = new FifoTaskManager(1);
            var process = new Process();

            // Act
            taskManager.Add(process);

            // Assert
            Assert.AreEqual(1, taskManager.ListRunningProcesses().Count);
            CollectionAssert.Contains(taskManager.ListRunningProcesses(), process);
        }

        [Test]
        public void Add_MaxCapacityReached_KillOldProcess()
        {
            // Arrange
            var oldProcess = new Process() { PID = 11 };
            var newProcess = new Process() { PID = 12 };
            var taskManager = new FifoTaskManager(1);
            taskManager.Add(oldProcess);

            // Act
            taskManager.Add(newProcess);

            // Assert
            CollectionAssert.DoesNotContain(taskManager.ListRunningProcesses(), oldProcess);
        }

        [Test]
        public void Add_MaxCapacityReached_AddNewProcess()
        {
            // Arrange
            var oldPid = 12;
            var newProcess = new Process() { PID = 11};
            var taskManager = new FifoTaskManager(1);
            taskManager.Add(new Process() { PID = oldPid });

            // Act
            taskManager.Add(newProcess);

            // Assert
            Assert.Contains(newProcess, taskManager.ListRunningProcesses().ToList());
        }

        [Test]
        public void List_ShallReturnOrderedListOfProcessesByCreationDate()
        {
            // Arrange
            var taskManager = new FifoTaskManager(5);
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
            var taskManager = new FifoTaskManager(5);
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
            var taskManager = new FifoTaskManager(5);
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
            var taskManager = new TaskManager.FifoTaskManager(5);
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
            var taskManager = new TaskManager.FifoTaskManager(5);
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
            var taskManager = new TaskManager.FifoTaskManager(5);
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
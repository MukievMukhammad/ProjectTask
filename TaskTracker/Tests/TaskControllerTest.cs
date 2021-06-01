using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TaskTracker.Controllers;
using TaskTracker.DB;
using TaskTracker.Models;

namespace Tests
{
    public class TaskControllerTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateTask()
        {
            // Arrange
            var dbContext = new TaskTrackerDbContext();
            var controller = new TaskController(dbContext);

            // Act
            var task = new Task {Name = "My Task"};
            controller.Create(task);

            //Assert
            var tsk = dbContext.Tasks.First(p => p.Name == "My Task");
            Assert.IsNotNull(tsk);
        }

        [Test]
        public void CreateTaskWithNullName()
        {
            // Arrange
            var dbContext = new TaskTrackerDbContext();
            var controller = new TaskController(dbContext);

            // Act
            var task = new Task();
            var message = controller.Create(task);

            //Assert
            Assert.AreEqual(message, "Task Name should not be empty!");
        }

        [Test]
        public void CreateTaskWithNegativePriority()
        {
            // Arrange
            var dbContext = new TaskTrackerDbContext();
            var controller = new TaskController(dbContext);

            // Act
            var task = new Task()
            {
                Name = "My Task", 
                Priority = -10
            };
            var message = controller.Create(task);

            //Assert
            Assert.AreEqual(message, "Priority should be positive!");
        }
        
        [Test]
        public void ViewTask()
        {
            // Arrange
            var dbContext = new TaskTrackerDbContext();
            var controller = new TaskController(dbContext);

            // Act
            var tsk = controller.View(1);

            //Assert
            Assert.IsNotNull(tsk);
        }
        
        [Test]
        public void EditTaskName()
        {
            // Arrange
            var dbContext = new TaskTrackerDbContext();
            var controller = new TaskController(dbContext);

            // Act
            var task = new Task() {Id = 1, Name = "Another Task Name"};
            controller.Edit(task);

            //Assert
            var tsk = dbContext.Projects.First(p => p.Id == 1);
            Assert.AreEqual(tsk.Name, "Another Task Name");
        }

        [Test]
        public void EditTaskWithNegativePriority()
        {
            // Arrange
            var dbContext = new TaskTrackerDbContext();
            var controller = new TaskController(dbContext);

            // Act
            var task = new Task()
            {
                Id = 1,
                Name = "My Task", 
                Priority = -10
            };
            var message = controller.Edit(task);

            //Assert
            Assert.AreEqual(message, "Priority should be positive!");
        }

        [Test]
        public void ChangeTaskProject()
        {
            // Arrange
            var dbContext = new TaskTrackerDbContext();
            var controller = new TaskController(dbContext);
            
            // Act
            var task = dbContext.Tasks.First(t => t.Id == 1);
            var prj = dbContext.Projects
                .Include(p => p.Tasks).First(p => !p.Tasks.Contains(task));
            task.Project = prj;
            controller.Edit(task);
            
            // Assert
            Assert.AreEqual(dbContext.Tasks
                .Include(t => t.Project)
                .First(t => t.Id == 1).Project.Id, prj.Id);
        }

        [Test]
        public void DeleteTask()
        {
            // Arrange
            var dbContext = new TaskTrackerDbContext();
            var controller = new TaskController(dbContext);

            // Act
            var message = controller.Delete(1);

            // Assert
            var tsk = dbContext.Tasks.FirstOrDefault(p => p.Id == 1);
            Assert.IsNull(tsk);
        }
    }
}
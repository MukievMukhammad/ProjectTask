using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using TaskTracker.ActionFilter;
using TaskTracker.Controllers;
using TaskTracker.DB;
using TaskTracker.Models;
using Xunit;

namespace Tests
{
    public class ProjectControllerTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateProject()
        {
            // Arrange
            var dbContext = new TaskTrackerDbContext();
            var controller = new ProjectController(dbContext);

            // Act
            var project = new Project() {Name = "My Project"};
            controller.Create(project);

            //Assert
            var prj = dbContext.Projects.First(p => p.Name == "My Project");
            Assert.IsNotNull(prj);
        }

        public ValidationFailedResult OnActionExecuting(string errorName, string errorMessage)
        {
            //Arrange
            var modelState = new ModelStateDictionary();
            modelState.AddModelError(errorName, errorMessage);
            var httpContext = new DefaultHttpContext();
            var context = new ActionExecutingContext(
                new ActionContext(
                    httpContext: httpContext,
                    routeData: new RouteData(),
                    actionDescriptor: new ActionDescriptor(),
                    modelState: modelState
                ),
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<Controller>().Object);

            var validation = new ValidationFilterAttribute();

            //Act
            validation.OnActionExecuting(context);

            return context.Result as ValidationFailedResult;
        }

        [Test]
        public void CreateProjectWithNullName()
        {
            var validationFailed = OnActionExecuting("Name", "Project Name should not be an empty!");
            
            //Assert
            var error = validationFailed?.Errors[0][0].ErrorMessage;
            Assert.AreEqual("Project Name should not be an empty!", error);
        }

        [Test]
        public void CreateProjectCompletedDateLessThanStart()
        {
            // Arrange
            var dbContext = new TaskTrackerDbContext();
            var controller = new ProjectController(dbContext);

            // Act
            var project = new Project() 
            {
                    Name = "My Project", 
                    StartDate = new DateTime(2021, 3, 3), 
                    CompletionDate = new DateTime(2020, 2, 3)
            };
            var message = controller.Create(project);

            //Assert
            Assert.AreEqual("Start Date Should be less than Complete", message);
        }
        
        [Test]
        public void CreateProjectWithNegativePriority()
        {
            // Arrange
            var dbContext = new TaskTrackerDbContext();
            var controller = new ProjectController(dbContext);

            // Act
            var project = new Project
            {
                Name = "My Project", 
                Priority = -10
            };
            var message = controller.Create(project);

            //Assert
            Assert.AreEqual("Priority should be positive!", message);
        }
        
        [Test]
        public void ViewProject()
        {
            // Arrange
            var dbContext = new TaskTrackerDbContext();
            var controller = new ProjectController(dbContext);

            // Act
            var prj = controller.View(1);

            //Assert
            Assert.IsNotNull(prj);
        }
        
        [Test]
        public void EditProjectName()
        {
            // Arrange
            var dbContext = new TaskTrackerDbContext();
            var controller = new ProjectController(dbContext);

            // Act
            var project = new Project {Id = 1, Name = "Another Project Name"};
            controller.Edit(project);

            //Assert
            var prj = dbContext.Projects.First(p => p.Id == 1);
            Assert.AreEqual(prj.Name, "Another Project Name");
        }
        
        [Test]
        public void EditProjectDate()
        {
            // Arrange
            var dbContext = new TaskTrackerDbContext();
            var controller = new ProjectController(dbContext);

            // Act
            var project = new Project
            {
                Id = 1, 
                Name = "Another Project Name", 
                StartDate = new DateTime(2020,5,6),
                CompletionDate = new DateTime(2020,11,10)
            };
            controller.Edit(project);

            //Assert
            var prj = dbContext.Projects.First(p => p.Id == 1);
            Assert.AreEqual(prj.StartDate, new DateTime(2020,5,6));
            Assert.AreEqual(prj.CompletionDate, new DateTime(2020,11,10));
        }
        
        [Test]
        public void EditProjectCompletedDateLessThanStart()
        {
            // Arrange
            var dbContext = new TaskTrackerDbContext();
            var controller = new ProjectController(dbContext);

            // Act
            var project = new Project
            {
                Id = 1, 
                Name = "Another Project Name", 
                StartDate = new DateTime(2020,11,6),
                CompletionDate = new DateTime(2020,3,10)
            };
            var message = controller.Edit(project);

            //Assert
            Assert.AreEqual(message, "Start Date should be less than Complete Date!");
        }
        
        [Test]
        public void EditProjectWithNegativePriority()
        {
            // Arrange
            var dbContext = new TaskTrackerDbContext();
            var controller = new ProjectController(dbContext);

            // Act
            var project = new Project
            {
                Id = 1,
                Name = "My Project", 
                Priority = -10
            };
            var message = controller.Edit(project);

            //Assert
            Assert.AreEqual(message, "Priority should be positive!");
        }

        [Test]
        public void ViewProjectTasks()
        {
            // Arrange
            var dbContext = new TaskTrackerDbContext();
            var controller = new ProjectController(dbContext);
            
            // Act
            var tasks = controller.GetTasks(1);
            
            // Assert
            Assert.IsNotEmpty(tasks);
        }

        [Test]
        public void EditProjectStatus()
        {
            // Arrange
            var dbContext = new TaskTrackerDbContext();
            var controller = new ProjectController(dbContext);

            // Act
            var project = new Project
            {
                Id = 1, 
                Name = "Another Project Name",
                Status = ProjectStatus.Active
            };
            controller.Edit(project);

            //Assert
            var prj = dbContext.Projects.First(p => p.Id == 1);
            Assert.AreEqual(prj.Status, ProjectStatus.Active);
        }

        [Test]
        public void AddNewTaskToProject()
        {
            // Arrange
            var dbContext = new TaskTrackerDbContext();
            var controller = new ProjectController(dbContext);
            
            // Act
            var task = new Task {Name = "Some New Task"};
            controller.AddTask(1, task);
            
            // Assert
            var prj = dbContext.Projects
                .Include(p => p.Tasks)
                .First(p => p.Id == 1);
            Assert.IsNotEmpty(prj.Tasks.Where(t => t.Name == "Some New Task"));
        }
        
        [Test]
        public void AddExistingTaskToProject()
        {
            // Arrange
            var dbContext = new TaskTrackerDbContext();
            var controller = new ProjectController(dbContext);
            
            // Act
            controller.AddTaskById(1, 1);
            
            // Assert
            var prj = dbContext.Projects
                .Include(p => p.Tasks)
                .First(p => p.Id == 1);
            Assert.IsNotEmpty(prj.Tasks.Where(t => t.Id == 1));
        }
        
        [Test]
        public void RemoveTaskFromProject()
        {
            // Arrange
            var dbContext = new TaskTrackerDbContext();
            var controller = new ProjectController(dbContext);
            
            // Act
            controller.RemoveTask(1);
            
            // Assert
            var prj = dbContext.Projects
                .Include(p => p.Tasks)
                .First(p => p.Id == 1);
            Assert.IsEmpty(prj.Tasks.Where(t => t.Id == 1));
        }
        
        [Test]
        public void DeleteProject()
        {
            // Arrange
            var dbContext = new TaskTrackerDbContext();
            var controller = new ProjectController(dbContext);

            // Act
            var message = controller.Delete(1);

            // Assert
            var prj = dbContext.Projects.FirstOrDefault(p => p.Id == 1);
            Assert.IsNull(prj);
        }
    }
}
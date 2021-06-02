using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTracker.ActionFilter;
using TaskTracker.Models;
using TaskTracker.Models.ViewModels;

namespace TaskTracker.Controllers
{
    [Route("[controller]/[action]")]
    public class TaskController : Controller
    {
        private readonly DbContext _context;

        public TaskController(DbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public JsonResult View(long taskId)
        {
            var task = _context.Set<Task>()
                .Include(t => t.Project)
                .FirstOrDefault(t => t.Id == taskId);
            return task != null ? new JsonResult(task) : new JsonResult(NotFound());
        }

        [HttpPost]
        [ValidationFilter]
        public JsonResult Create(TaskViewModel taskViewModel)
        {
            var project = _context.Set<Project>().FirstOrDefault(p => p.Id == taskViewModel.ProjectId);
            if (project == null) return new JsonResult(NotFound("Project Not Found"));
            
            _context.Set<Task>().Add(new Task
            {
                Name = taskViewModel.Name,
                Status = taskViewModel.Status,
                Description = taskViewModel.Description,
                Priority = taskViewModel.Priority,
                Project = project
            });
            _context.SaveChanges();
            return new JsonResult(Ok());
        }
        
        [HttpPost]
        [ValidationFilter]
        public JsonResult Edit(TaskViewModelForEdit taskViewModel)
        {
            var tsk = _context.Set<Task>()
                .Include(p => p.Project)
                .FirstOrDefault(t => t.Id == taskViewModel.Id);
            if (tsk == null) return new JsonResult(NotFound("Task Not Found!"));

            var project = _context.Set<Project>().FirstOrDefault(p => p.Id == taskViewModel.ProjectId);
            if (project == null) 
                return new JsonResult(NotFound("Project Not Found!"));
            
            tsk.Update(new Task
            {
                Name = taskViewModel.Name,
                Description = taskViewModel.Description,
                Status = taskViewModel.Status,
                Priority = taskViewModel.Priority,
                Project = project
            });
            _context.SaveChanges();
            return new JsonResult(Ok());
        }

        [HttpPost]
        public JsonResult Delete(long taskId)
        {
            var task = _context.Set<Task>().FirstOrDefault(t => t.Id == taskId);
            if (task == null) return new JsonResult(NotFound("Task Not Found!"));
            _context.Set<Task>().Remove(task);
            _context.SaveChanges();
            return new JsonResult(Ok());
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using TaskTracker.ActionFilter;
using TaskTracker.Models;
using TaskTracker.Models.ViewModels;

namespace TaskTracker.Controllers
{
    [Route("[controller]/[action]")]
    public class ProjectController : Controller
    {
        private readonly DbContext _context;

        public ProjectController(DbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public JsonResult View(long id)
        {
            var project = _context.Set<Project>()
                .Include(p => p.Tasks)
                .FirstOrDefault(p => p.Id == id);
            return project != null ? new JsonResult(project) : new JsonResult(NotFound());
        }

        [HttpPost]
        [ValidationFilter]
        public JsonResult Create(ProjectViewModel projectViewModel)
        {
            _context.Set<Project>().Add(new Project
            {
                Name = projectViewModel.Name,
                Status = projectViewModel.Status,
                StartDate = projectViewModel.StartDate,
                CompletionDate = projectViewModel.CompletionDate,
                Priority = projectViewModel.Priority,
                Tasks = projectViewModel.Tasks
            });
            _context.SaveChanges();
            return new JsonResult(Ok());
        }
        
        [HttpPost]
        [ValidationFilter]
        public JsonResult Edit(Project project)
        {
            if (_context.Set<Project>().FirstOrDefault(p => p.Id == project.Id) == null)
                return new JsonResult(NotFound());
            _context.Set<Project>().Update(project);
            _context.SaveChanges();
            return new JsonResult(Ok());
        }
        
        [HttpPost]
        public JsonResult Delete(long projectId)
        {
            var project = _context.Set<Project>()
                .FirstOrDefault(p => p.Id == projectId);
            if (project == null) return new JsonResult(NotFound());
            _context.Set<Project>().Remove(project);
            _context.SaveChanges();
            return new JsonResult(Ok());
        }
        
        [HttpGet]
        public JsonResult GetTasks(long projectId)
        {
            var project = _context.Set<Project>()
                .Include(p => p.Tasks)
                .FirstOrDefault(p => p.Id == projectId);
            return project == null ? new JsonResult(NotFound()) : new JsonResult(project.Tasks);
        }

        [HttpPost]
        public JsonResult AddTask(long projectId, long taskId)
        {
            var project = _context.Set<Project>().FirstOrDefault(p => p.Id == projectId);
            var task = _context.Set<Task>()
                .Include(t => t.Project)
                .FirstOrDefault(t => t.Id == taskId);
            if (project == null || task == null)
                return new JsonResult(NotFound());
            if (task.Project.Id != projectId)
            {
                task.Project = project;
                _context.SaveChanges();
            }
            return new(Ok());
        }
    }
}
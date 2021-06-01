using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTracker.ActionFilter;
using TaskTracker.Models;

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
        public Project View(long id)
        {
            return new Project();
        }

        [HttpPost]
        [ValidationFilter]
        public JsonResult Create(Project project)
        {
            _context.Set<Project>().Add(project);
            _context.SaveChanges();
            return new JsonResult(Ok());
        }
        
        [HttpPost]
        public JsonResult Edit(Project project)
        {
            return new JsonResult(Ok());
        }
        
        [HttpPost]
        public JsonResult Delete(long id)
        {
            return new JsonResult(Ok());
        }
        
        [HttpGet]
        public IEnumerable<Task> GetTasks(long id)
        {
            return new List<Task>();
        }

        [HttpPost]
        public JsonResult AddTask(long prjId, Task task)
        {
            return new(Ok());
        }
        
        [HttpPost]
        public JsonResult AddTaskById(long prjId, long taskId)
        {
            return new(Ok());
        }

        [HttpPost]
        public JsonResult RemoveTask(long taskId)
        {
            return new(Ok());
        }
    }
}
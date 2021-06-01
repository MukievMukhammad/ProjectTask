using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
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
        public JsonResult View(long id)
        {
            var project = _context.Set<Project>().FirstOrDefault(p => p.Id == id);
            return project != null ? new JsonResult(project) : new JsonResult(NotFound());
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
        [ValidationFilter]
        public JsonResult Edit(Project project)
        {
            var prj = _context.Set<Project>()
                .Include(p => p.Tasks)
                .FirstOrDefault(p => p.Id == project.Id);
            if (prj == null)
                return new JsonResult(NotFound());
            prj.Update(project);
            _context.SaveChanges();
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
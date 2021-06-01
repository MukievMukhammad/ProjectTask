using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Models;

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
        public Task View(long id)
        {
            return new();
        }

        [HttpPost]
        public JsonResult Create(Task task)
        {
            return new(Ok());
        }
        
        [HttpPost]
        public JsonResult Edit(Task task)
        {
            return new(Ok());
        }

        [HttpPost]
        public JsonResult Delete(long taskId)
        {
            throw new System.NotImplementedException();
        }
    }
}
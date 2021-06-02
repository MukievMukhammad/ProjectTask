using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;

namespace TaskTracker.Models
{
    public enum TaskStatus
    {
        ToDo,
        InProgress,
        Done
    }
    
    public class Task
    {
        [Key]
        public long Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Task Name should not be an empty!")]
        public string Name { get; set; }
        public TaskStatus Status { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public Project Project { get; set; }

        public void Update(Task task)
        {
            Name = task.Name;
            Status = task.Status;
            Description = task.Description;
            Priority = task.Priority;
            Project = task.Project;
        }
    }
}
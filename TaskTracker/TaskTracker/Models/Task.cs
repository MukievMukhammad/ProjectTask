using System.ComponentModel.DataAnnotations;

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
        public string Name { get; set; }
        public TaskStatus Status { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public Project Project { get; set; }
    }
}
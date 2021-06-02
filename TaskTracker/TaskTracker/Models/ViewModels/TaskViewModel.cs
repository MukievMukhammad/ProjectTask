using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Models.ViewModels
{
    public class TaskViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Task Name should not be an empty!")]
        public string Name { get; set; }
        public TaskStatus Status { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public long ProjectId { get; set; }
    }
}
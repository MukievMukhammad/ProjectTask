using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Models.ViewModels
{
    public class TaskViewModelForEdit : TaskViewModel
    {
        [Key]
        public long Id { get; set; }
    }
}
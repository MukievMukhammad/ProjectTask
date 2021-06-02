using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Attributes;

namespace TaskTracker.Models.ViewModels
{
    public class ProjectViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Project Name should not be an empty!")]
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        [GreaterThan("StartDate", true)]
        public DateTime? CompletionDate { get; set; }
        public ProjectStatus Status { get; set; }
        [Range(0, int.MaxValue)]
        public int Priority { get; set; }
        
        public List<Task> Tasks { get; set; }
    }
}
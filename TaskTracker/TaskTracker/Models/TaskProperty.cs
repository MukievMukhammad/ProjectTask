using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Models
{
    public class TaskProperty
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class TodoItem
    {
        public int Id { get; init; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; init; } = DateTime.UtcNow;
    }
}

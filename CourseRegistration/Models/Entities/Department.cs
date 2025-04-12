using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseRegistration.Models.Entities
{
    public class Department
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Department code is required")]
        [StringLength(10, ErrorMessage = "Department code cannot exceed 10 characters")]
        public string Code { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Department name is required")]
        [StringLength(100, ErrorMessage = "Department name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;
        
        // Navigation properties
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
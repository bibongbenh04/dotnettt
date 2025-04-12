using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseRegistration.Models.Entities
{
    public class Course
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Course code is required")]
        [StringLength(20, ErrorMessage = "Course code cannot exceed 20 characters")]
        public string Code { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Course title is required")]
        [StringLength(200, ErrorMessage = "Course title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Credits are required")]
        [Range(1, 6, ErrorMessage = "Credits must be between 1 and 6")]
        public int Credits { get; set; }
        
        [Required(ErrorMessage = "Department is required")]
        public int DepartmentId { get; set; }
        
        // Navigation properties
        public Department Department { get; set; } = null!;
        public ICollection<Section> Sections { get; set; } = new List<Section>();
    }
}
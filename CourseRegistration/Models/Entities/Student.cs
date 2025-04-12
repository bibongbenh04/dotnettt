// Models/Entities/Student.cs
using System.Collections.Generic;

namespace CourseRegistration.Models.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        
        // Navigation properties
        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }
}
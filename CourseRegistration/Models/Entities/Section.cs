// Models/Entities/Section.cs
using System;
using System.Collections.Generic;

namespace CourseRegistration.Models.Entities
{
    public class Section
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string SectionNumber { get; set; } = string.Empty;
        public string Semester { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Instructor { get; set; } = string.Empty;
        public string Schedule { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public DateTime RegistrationDeadline { get; set; }
        
        // Navigation properties
        public Course Course { get; set; } = null!;
        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }
}
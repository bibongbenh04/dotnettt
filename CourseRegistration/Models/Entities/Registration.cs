// Models/Entities/Registration.cs
using System;

namespace CourseRegistration.Models.Entities
{
    public class Registration
    {
        public int Id { get; set; }
        public int SectionId { get; set; }
        public int StudentId { get; set; }
        public DateTime RegistrationDate { get; set; }
        
        // Navigation properties
        public Section Section { get; set; } = null!;
        public Student Student { get; set; } = null!;
    }
}
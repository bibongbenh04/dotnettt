// Models/ViewModels/SectionViewModel.cs
using System;

namespace CourseRegistration.Models.ViewModels
{
    public class SectionViewModel
    {
        public int Id { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseTitle { get; set; } = string.Empty;
        public string SectionNumber { get; set; } = string.Empty;
        public string Instructor { get; set; } = string.Empty;
        public string Schedule { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int Credits { get; set; }
        public int AvailableSeats { get; set; }
        public int Capacity { get; set; }
        public DateTime RegistrationDeadline { get; set; }
    }
}
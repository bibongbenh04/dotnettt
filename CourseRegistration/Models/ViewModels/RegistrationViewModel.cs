// Models/ViewModels/RegistrationViewModel.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace CourseRegistration.Models.ViewModels
{
    public class RegistrationViewModel
    {
        public int SectionId { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseTitle { get; set; } = string.Empty;
        public string SectionNumber { get; set; } = string.Empty;
        public string Instructor { get; set; } = string.Empty;
        public string Schedule { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int Credits { get; set; }
        public int AvailableSeats { get; set; }
        
        [Required(ErrorMessage = "Student ID is required")]
        public string StudentId { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Name is required")]
        public string StudentName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string StudentEmail { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits")]
        public string StudentPhone { get; set; } = string.Empty;
    }
}
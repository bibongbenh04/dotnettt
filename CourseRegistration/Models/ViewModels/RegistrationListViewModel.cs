// Models/ViewModels/RegistrationListViewModel.cs
using System;
using System.Collections.Generic;

namespace CourseRegistration.Models.ViewModels
{
    public class RegistrationListViewModel
    {
        public int SectionId { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseTitle { get; set; } = string.Empty;
        public string SectionNumber { get; set; } = string.Empty;
        public string Instructor { get; set; } = string.Empty;
        public string Schedule { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public int RegisteredCount { get; set; }
        public List<RegistrationDetail> Registrations { get; set; } = new List<RegistrationDetail>();
        
        public class RegistrationDetail
        {
            public int Id { get; set; }
            public string StudentId { get; set; } = string.Empty;
            public string StudentName { get; set; } = string.Empty;
            public string StudentEmail { get; set; } = string.Empty;
            public string StudentPhone { get; set; } = string.Empty;
            public DateTime RegistrationDate { get; set; }
        }
    }
}
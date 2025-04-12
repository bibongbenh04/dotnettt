// Services/RegistrationService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CourseRegistration.Models;
using CourseRegistration.Models.Entities;
using CourseRegistration.Models.ViewModels;

namespace CourseRegistration.Services
{
    public class RegistrationService
    {
        private readonly CourseRegistrationDbContext _context;
        
        public RegistrationService(CourseRegistrationDbContext context)
        {
            _context = context;
        }
        
        // Get all departments
        public async Task<List<Department>> GetDepartmentsAsync()
        {
            return await _context.Departments.ToListAsync();
        }
        
        // Get all courses
        public async Task<List<Course>> GetCoursesAsync()
        {
            return await _context.Courses
                .Include(c => c.Department)
                .ToListAsync();
        }
        
        // Get courses by department
        public async Task<List<Course>> GetCoursesByDepartmentAsync(int departmentId)
        {
            return await _context.Courses
                .Where(c => c.DepartmentId == departmentId)
                .Include(c => c.Department)
                .ToListAsync();
        }
        
        // Get all available sections (registration deadline not passed)
        public async Task<List<SectionViewModel>> GetAvailableSectionsAsync()
        {
            var sections = await _context.Sections
                .Include(s => s.Course)
                .ThenInclude(c => c.Department)
                .Where(s => s.RegistrationDeadline > DateTime.Now)
                .OrderBy(s => s.Course.Code)
                .ThenBy(s => s.SectionNumber)
                .ToListAsync();
                
            var viewModels = new List<SectionViewModel>();
            
            foreach (var section in sections)
            {
                int registeredCount = await _context.Registrations
                    .Where(r => r.SectionId == section.Id)
                    .CountAsync();
                    
                int availableSeats = section.Capacity - registeredCount;
                
                viewModels.Add(new SectionViewModel
                {
                    Id = section.Id,
                    CourseCode = section.Course.Code,
                    CourseTitle = section.Course.Title,
                    SectionNumber = section.SectionNumber,
                    Instructor = section.Instructor,
                    Schedule = section.Schedule,
                    Location = section.Location,
                    Credits = section.Course.Credits,
                    AvailableSeats = availableSeats,
                    Capacity = section.Capacity,
                    RegistrationDeadline = section.RegistrationDeadline
                });
            }
            
            return viewModels;
        }
        
        // Get section details
        public async Task<SectionViewModel?> GetSectionDetailsAsync(int sectionId)
        {
            var section = await _context.Sections
                .Include(s => s.Course)
                .FirstOrDefaultAsync(s => s.Id == sectionId);
                
            if (section == null)
                return null;
                
            int registeredCount = await _context.Registrations
                .Where(r => r.SectionId == section.Id)
                .CountAsync();
                
            int availableSeats = section.Capacity - registeredCount;
            
            return new SectionViewModel
            {
                Id = section.Id,
                CourseCode = section.Course.Code,
                CourseTitle = section.Course.Title,
                SectionNumber = section.SectionNumber,
                Instructor = section.Instructor,
                Schedule = section.Schedule,
                Location = section.Location,
                Credits = section.Course.Credits,
                AvailableSeats = availableSeats,
                Capacity = section.Capacity,
                RegistrationDeadline = section.RegistrationDeadline
            };
        }
        
        // Register for a section
        public async Task<(bool Success, string Message)> RegisterForSectionAsync(
            int sectionId, string studentId, string name, string email, string phone)
        {
            // Get the section
            var section = await _context.Sections
                .FirstOrDefaultAsync(s => s.Id == sectionId);
                
            if (section == null)
                return (false, "Section not found");
                
            // Check if registration deadline has passed
            if (section.RegistrationDeadline <= DateTime.Now)
                return (false, "Registration deadline has passed");
                
            // Check if section is full
            int registeredCount = await _context.Registrations
                .Where(r => r.SectionId == section.Id)
                .CountAsync();
                
            if (registeredCount >= section.Capacity)
                return (false, "Section is full");
                
            // Get or create student
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.StudentId == studentId);
                
            if (student == null)
            {
                student = new Student
                {
                    StudentId = studentId,
                    Name = name,
                    Email = email,
                    Phone = phone
                };
                
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Update student information
                student.Name = name;
                student.Email = email;
                student.Phone = phone;
                
                _context.Students.Update(student);
                await _context.SaveChangesAsync();
            }
            
            // Check if student is already registered for this section
            bool alreadyRegistered = await _context.Registrations
                .AnyAsync(r => r.SectionId == sectionId && r.StudentId == student.Id);
                
            if (alreadyRegistered)
                return (false, "You are already registered for this section");
                
            // Create registration
            var registration = new Registration
            {
                SectionId = sectionId,
                StudentId = student.Id,
                RegistrationDate = DateTime.Now
            };
            
            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();
            
            return (true, "Registration successful");
        }
        
        // Get registrations for a section
        public async Task<RegistrationListViewModel?> GetRegistrationsForSectionAsync(int sectionId)
        {
            var section = await _context.Sections
                .Include(s => s.Course)
                .FirstOrDefaultAsync(s => s.Id == sectionId);
                
            if (section == null)
                return null;
                
            var registrations = await _context.Registrations
                .Where(r => r.SectionId == sectionId)
                .Include(r => r.Student)
                .OrderBy(r => r.RegistrationDate)
                .ToListAsync();
                
            var viewModel = new RegistrationListViewModel
            {
                SectionId = section.Id,
                CourseCode = section.Course.Code,
                CourseTitle = section.Course.Title,
                SectionNumber = section.SectionNumber,
                Instructor = section.Instructor,
                Schedule = section.Schedule,
                Capacity = section.Capacity,
                RegisteredCount = registrations.Count,
                Registrations = registrations.Select(r => new RegistrationListViewModel.RegistrationDetail
                {
                    Id = r.Id,
                    StudentId = r.Student.StudentId,
                    StudentName = r.Student.Name,
                    StudentEmail = r.Student.Email,
                    StudentPhone = r.Student.Phone,
                    RegistrationDate = r.RegistrationDate
                }).ToList()
            };
            
            return viewModel;
        }
    }
}
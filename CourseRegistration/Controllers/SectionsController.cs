// Update your existing SectionsController.cs file with this code
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourseRegistration.Models;
using CourseRegistration.Models.Entities;
using CourseRegistration.Services;

namespace CourseRegistration.Controllers
{
    public class SectionsController : Controller
    {
        private readonly CourseRegistrationDbContext _context;
        private readonly RegistrationService _registrationService;
        
        public SectionsController(CourseRegistrationDbContext context, RegistrationService registrationService)
        {
            _context = context;
            _registrationService = registrationService;
        }
        
        // GET: Sections
        public async Task<IActionResult> Index()
        {
            var sections = await _registrationService.GetAvailableSectionsAsync();
            return View(sections);
        }
        
        // GET: Sections/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var section = await _registrationService.GetSectionDetailsAsync(id);
                
            if (section == null)
                return NotFound();
                
            return View(section);
        }
        
        // GET: Sections/Create
        [HttpGet]
        public async Task<IActionResult> Create(int? courseId)
        {
            ViewBag.Courses = await _context.Courses.ToListAsync();
            
            var model = new Section
            {
                CourseId = courseId ?? 0,
                Year = DateTime.Now.Year,
                Semester = GetCurrentSemester(),
                RegistrationDeadline = DateTime.Now.AddDays(30)
            };
            
            return View(model);
        }
        
        // POST: Sections/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,SectionNumber,Semester,Year,Instructor,Schedule,Location,Capacity,RegistrationDeadline")] Section section)
        {

            // Check if section number already exists for this course
            bool sectionExists = await _context.Sections
                .AnyAsync(s => s.CourseId == section.CourseId && 
                                s.SectionNumber == section.SectionNumber && 
                                s.Semester == section.Semester && 
                                s.Year == section.Year);
                                
            if (sectionExists)
            {
                ModelState.AddModelError("SectionNumber", "This section number already exists for this course in the selected semester");
                ViewBag.Courses = await _context.Courses.ToListAsync();
                return View(section);
            }
            
            _context.Add(section);
            await _context.SaveChangesAsync();
            
            // If this section was created from a course details page, redirect back to that page
            if (Request.Query.ContainsKey("courseId"))
            {
                return RedirectToAction("Details", "Courses", new { id = section.CourseId });
            }
            
            return RedirectToAction(nameof(Index));

        }
        
        private string GetCurrentSemester()
        {
            int month = DateTime.Now.Month;
            
            if (month >= 1 && month <= 5)
                return "Spring";
            else if (month >= 6 && month <= 7)
                return "Summer";
            else
                return "Fall";
        }
    }
}
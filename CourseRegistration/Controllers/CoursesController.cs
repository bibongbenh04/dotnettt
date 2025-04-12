using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourseRegistration.Models;
using CourseRegistration.Models.Entities;
using CourseRegistration.Services;

namespace CourseRegistration.Controllers
{
    public class CoursesController : Controller
    {
        private readonly CourseRegistrationDbContext _context;
        private readonly RegistrationService _registrationService;
        
        public CoursesController(CourseRegistrationDbContext context, RegistrationService registrationService)
        {
            _context = context;
            _registrationService = registrationService;
        }
        
        // GET: Courses
        public async Task<IActionResult> Index(int? departmentId)
        {
            ViewBag.Departments = await _context.Departments.ToListAsync();
            
            if (departmentId.HasValue)
            {
                var courses = await _registrationService.GetCoursesByDepartmentAsync(departmentId.Value);
                return View(courses);
            }
            else
            {
                var courses = await _registrationService.GetCoursesAsync();
                return View(courses);
            }
        }
        
        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();
                
            var course = await _context.Courses
                .Include(c => c.Department)
                .Include(c => c.Sections)
                .FirstOrDefaultAsync(c => c.Id == id);
                
            if (course == null)
                return NotFound();
                
            return View(course);
        }
        
        // GET: Courses/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await _context.Departments.ToListAsync();
            return View();
        }
        
        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Code,Title,Credits,DepartmentId")] Course course)
        {

            // Check if course code already exists
            bool codeExists = await _context.Courses.AnyAsync(c => c.Code == course.Code);
            if (codeExists)
            {
                ModelState.AddModelError("Code", "Course code already exists");
                ViewBag.Departments = await _context.Departments.ToListAsync();
                return View(course);
            }
            
            _context.Add(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
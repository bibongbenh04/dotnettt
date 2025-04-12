using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourseRegistration.Models;
using CourseRegistration.Models.Entities;

namespace CourseRegistration.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly CourseRegistrationDbContext _context;
        
        public DepartmentsController(CourseRegistrationDbContext context)
        {
            _context = context;
        }
        
        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var departments = await _context.Departments.ToListAsync();
            return View(departments);
        }
        
        // GET: Departments/Create
        public IActionResult Create()
        {
            return View();
        }
        
        // POST: Departments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Code,Name")] Department department)
        {
            if (ModelState.IsValid)
            {
                // Check if department code already exists
                bool codeExists = await _context.Departments.AnyAsync(d => d.Code == department.Code);
                if (codeExists)
                {
                    ModelState.AddModelError("Code", "Department code already exists");
                    return View(department);
                }
                
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }
    }
}
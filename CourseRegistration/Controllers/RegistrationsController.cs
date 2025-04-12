// Add to all controller and service files
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using CourseRegistration.Models;
using CourseRegistration.Models.Entities;
using CourseRegistration.Models.ViewModels;
using CourseRegistration.Services;

namespace CourseRegistration.Controllers
{
    [Route("[controller]")]
    public class RegistrationsController : Controller
    {
        private readonly CourseRegistrationDbContext _context;
        private readonly RegistrationService _registrationService;
        
        public RegistrationsController(CourseRegistrationDbContext context, RegistrationService registrationService)
        {
            _context = context;
            _registrationService = registrationService;
        }
        
        // GET: Registrations/Register/5
        [HttpGet("Register/{id}")]
        public async Task<IActionResult> Register(int? id)
        {
            if (id == null)
                return NotFound();
                
            var section = await _registrationService.GetSectionDetailsAsync(id.Value);
                
            if (section == null)
                return NotFound();
                
            // Check if registration deadline has passed
            if (section.RegistrationDeadline <= DateTime.Now)
                return RedirectToAction("Index", "Sections");
                
            // Check if section is full
            if (section.AvailableSeats <= 0)
            {
                TempData["ErrorMessage"] = "This section is full.";
                return RedirectToAction("Index", "Sections");
            }
            
            var viewModel = new RegistrationViewModel
            {
                SectionId = section.Id,
                CourseCode = section.CourseCode,
                CourseTitle = section.CourseTitle,
                SectionNumber = section.SectionNumber,
                Instructor = section.Instructor,
                Schedule = section.Schedule,
                Location = section.Location,
                Credits = section.Credits,
                AvailableSeats = section.AvailableSeats
            };
            
            return View(viewModel);
        }
        
        // POST: Registrations/Register
        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _registrationService.RegisterForSectionAsync(
                    model.SectionId,
                    model.StudentId,
                    model.StudentName,
                    model.StudentEmail,
                    model.StudentPhone);
                    
                if (result.Success)
                {
                    TempData["SuccessMessage"] = result.Message;
                    return RedirectToAction("Index", "Sections");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                }
            }
            
            // If we got this far, something failed, redisplay form
            var section = await _registrationService.GetSectionDetailsAsync(model.SectionId);
                
            if (section == null)
                return NotFound();
                
            model.CourseCode = section.CourseCode;
            model.CourseTitle = section.CourseTitle;
            model.SectionNumber = section.SectionNumber;
            model.Instructor = section.Instructor;
            model.Schedule = section.Schedule;
            model.Location = section.Location;
            model.Credits = section.Credits;
            model.AvailableSeats = section.AvailableSeats;
            
            return View(model);
        }
        
        // GET: Registrations/List/5
        [HttpGet("List/{id}")]
        public async Task<IActionResult> List(int? id)
        {
            if (id == null)
                return NotFound();
                
            var viewModel = await _registrationService.GetRegistrationsForSectionAsync(id.Value);
                
            if (viewModel == null)
                return NotFound();
                
            return View(viewModel);
        }
    }
}
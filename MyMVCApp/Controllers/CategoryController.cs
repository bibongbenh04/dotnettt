using Microsoft.AspNetCore.Mvc;
using MyMVCApp.Data;
using MyMVCApp.Models;

namespace MyMVCApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        // GET: CategoryController
        public IActionResult Index()
        {
            var objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }

        public IActionResult Create() {

            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString()) {
                ModelState.AddModelError("Name", "Display Order should not be same as Name");
            }
            if (category.Name.ToLower() == "test") {
                ModelState.AddModelError("", "test is not allowed");
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Add(category);
                _db.SaveChanges();
                TempData["success"] = "Category create successfully";
                return RedirectToAction("Index");
            }
            else {
                return View();
            }
        }
        public IActionResult Edit(int? id) {
            if (id == null || id == 0) {
                return NotFound();
            }
            Category? categoryFromDb = _db.Categories.Find(id);
            // Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.ID==id);
            // Category? categoryFromDb2 = _db.Categories.Where(u=>u.ID==id).FirstOrDefault();
            if (categoryFromDb == null) {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(category);
                _db.SaveChanges();
                TempData["success"] = "Category edit successfully";
                return RedirectToAction("Index");
            }
            else {
                return View();
            }
        }
        public IActionResult Delete(int? id) {
            if (id == null || id == 0) {
                return NotFound();
            }
            Category? categoryFromDb = _db.Categories.Find(id);
            // Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.ID==id);
            // Category? categoryFromDb2 = _db.Categories.Where(u=>u.ID==id).FirstOrDefault();
            if (categoryFromDb == null) {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _db.Categories.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}

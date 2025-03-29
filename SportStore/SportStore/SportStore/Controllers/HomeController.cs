using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SportStore.Models;
using SportStore.Models.ViewModels;

namespace SportStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStoreRepository repository;
        public int pageSize = 4;
        public HomeController(ILogger<HomeController> logger, IStoreRepository repo)
        {
            _logger = logger;
            repository = repo;
        }

        public IActionResult Index(string? category, int productPage = 1)
        {
            var filteredProducts = string.IsNullOrEmpty(category)
                ? repository.GetProducts
                : repository.GetProducts.Where(p => p.Category == category);
            return View(new ProductsListViewModel
            {
                Products = filteredProducts
                .OrderBy(p => p.ProductID)
                .Skip((productPage - 1) * pageSize)
                .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemPerPage = pageSize,
                    TotalItems = repository.GetProducts.Count()
                },
                Categories = repository.GetProducts
                    .Select(p => p.Category)
                    .Distinct()
                    .OrderBy(c => c),

                CurrentCategory = category
            });          
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

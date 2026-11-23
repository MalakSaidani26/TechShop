using Microsoft.AspNetCore.Mvc;
using TechShop.Models;
using TechShop.ViewModels;

namespace TechShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;

        public HomeController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            var productsOfTheWeek = _productRepository.ProductsOfTheWeek;
            var homeViewModel = new HomeViewModel(productsOfTheWeek);
            return View(homeViewModel);
        }
    }
}
using TechShop.Models;
using TechShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace TechShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IShoppingCartService _shoppingCartService;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, IShoppingCartService shoppingCartService)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _shoppingCartService = shoppingCartService;
        }

        public IActionResult List(string searchQuery = "", int? categoryId = null)
        {
            var languageCode = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var categories = _categoryRepository.AllCategories.ToList();

            var products = string.IsNullOrWhiteSpace(searchQuery)
                ? _productRepository.AllProducts
                : _productRepository.SearchProducts(searchQuery, languageCode);

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.Category.CategoryId == categoryId.Value);
            }

            var categoryTitle = categoryId.HasValue
                ? categories.FirstOrDefault(c => c.CategoryId == categoryId.Value)?.GetLocalizedName(languageCode)
                : null;

            var currentCategoryLabel = !string.IsNullOrWhiteSpace(searchQuery)
                ? (languageCode switch
                {
                    "en" => $"Results for: {searchQuery}",
                    "ar" => $"نتائج البحث: {searchQuery}",
                    _ => $"Résultats pour: {searchQuery}"
                })
                : (categoryTitle ?? (languageCode switch
                {
                    "en" => "All products",
                    "ar" => "كل المنتجات",
                    _ => "Tous les produits"
                }));

            ProductListViewModel productListViewModel = new ProductListViewModel(
                products,
                categories,
                currentCategoryLabel,
                categoryId
            );

            return View(productListViewModel);
        }

        public IActionResult Details(int id)
        {
            var product = _productRepository.GetProductById(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, string? searchQuery = null, int? categoryId = null)
        {
            var product = _productRepository.GetProductById(productId);
            var languageCode = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            if (product is not null && product.InStock)
            {
                _shoppingCartService.AddToCart(product);
                TempData["CartMessage"] = languageCode switch
                {
                    "en" => $"{product.GetLocalizedName(languageCode)} added to cart.",
                    "ar" => $"تمت إضافة {product.GetLocalizedName(languageCode)} إلى السلة.",
                    _ => $"{product.GetLocalizedName(languageCode)} ajouté au panier."
                };
            }
            else
            {
                TempData["CartMessage"] = languageCode switch
                {
                    "en" => "Product unavailable.",
                    "ar" => "المنتج غير متوفر.",
                    _ => "Produit indisponible."
                };
            }

            return RedirectToAction(nameof(List), new
            {
                searchQuery = searchQuery ?? string.Empty,
                categoryId
            });
        }
    }
}
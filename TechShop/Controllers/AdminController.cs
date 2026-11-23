using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using TechShop.Models;
using TechShop.ViewModels;
using System.Linq;

namespace TechShop.Controllers
{
    public class AdminController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IAuthService _authService;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IInvoicePdfService _invoicePdfService;

        public AdminController(
            IOrderRepository orderRepository,
            IAuthService authService,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IWebHostEnvironment webHostEnvironment,
            IInvoicePdfService invoicePdfService)
        {
            _orderRepository = orderRepository;
            _authService = authService;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
            _invoicePdfService = invoicePdfService;
        }

        private IActionResult? EnsureAdmin()
        {
            if (!_authService.IsAuthenticated() || !_authService.IsAdmin())
            {
                var returnUrl = Url.Action(nameof(Dashboard), "Admin");
                return RedirectToAction("Login", "Account", new { returnUrl });
            }

            return null;
        }

        private async Task<string?> SaveUploadedImageAsync(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            if (imageFile.Length > 5 * 1024 * 1024) // 5MB max
                return null;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var ext = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
                return null;

            try
            {
                var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
                Directory.CreateDirectory(uploadPath);

                var fileName = $"{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                return fileName;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            var guard = EnsureAdmin();
            if (guard is not null)
            {
                return guard;
            }

            var orders = _orderRepository.GetAll();
            var viewModel = new AdminDashboardViewModel
            {
                TotalOrders = orders.Count,
                PendingOrders = orders.Count(o => o.Status == OrderStatus.Pending),
                ValidatedOrders = orders.Count(o => o.Status == OrderStatus.Validated),
                TotalCustomers = _authService.GetRegisteredUsers().Count,
                TotalRevenue = orders.Sum(o => o.TotalAmount),
                LatestOrders = orders.OrderByDescending(o => o.CreatedAt).Take(5).ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult AdminDashboard()
        {
            var guard = EnsureAdmin();
            if (guard is not null)
            {
                return guard;
            }

            var orders = _orderRepository.GetAll();
            var viewModel = new AdminDashboardViewModel
            {
                TotalOrders = orders.Count,
                PendingOrders = orders.Count(o => o.Status == OrderStatus.Pending),
                ValidatedOrders = orders.Count(o => o.Status == OrderStatus.Validated),
                TotalCustomers = _authService.GetRegisteredUsers().Count,
                TotalRevenue = orders.Sum(o => o.TotalAmount),
                LatestOrders = orders.OrderByDescending(o => o.CreatedAt).Take(5).ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Orders()
        {
            var guard = EnsureAdmin();
            if (guard is not null)
            {
                return guard;
            }

            var viewModel = new AdminOrdersViewModel
            {
                Orders = _orderRepository.GetAll().OrderByDescending(o => o.CreatedAt).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ValidateOrder(Guid orderId)
        {
            var guard = EnsureAdmin();
            if (guard is not null)
            {
                return guard;
            }

            _orderRepository.Validate(orderId);
            return RedirectToAction(nameof(Orders));
        }

        [HttpPost]
        public IActionResult DeleteOrder(Guid orderId)
        {
            var guard = EnsureAdmin();
            if (guard is not null)
            {
                return guard;
            }

            _orderRepository.Delete(orderId);
            return RedirectToAction(nameof(Orders));
        }

        [HttpGet]
        public IActionResult Customers()
        {
            var guard = EnsureAdmin();
            if (guard is not null)
            {
                return guard;
            }

            var viewModel = new AdminCustomersViewModel
            {
                Customers = _authService.GetRegisteredUsers()
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Products()
        {
            var guard = EnsureAdmin();
            if (guard is not null)
            {
                return guard;
            }

            var viewModel = new AdminProductListViewModel
            {
                Products = _productRepository.AllProducts.ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult ViewProduct(int id)
        {
            var guard = EnsureAdmin();
            if (guard is not null)
            {
                return guard;
            }

            var product = _productRepository.GetProductById(id);
            if (product is null)
            {
                return RedirectToAction(nameof(Products));
            }

            var viewModel = new AdminProductDetailsViewModel { Product = product };
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            var guard = EnsureAdmin();
            if (guard is not null)
            {
                return guard;
            }

            var product = _productRepository.GetProductById(id);
            if (product is null)
            {
                return RedirectToAction(nameof(Products));
            }

            var viewModel = new AdminProductEditViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                NameEn = product.NameEn,
                NameAr = product.NameAr,
                ShortDescription = product.ShortDescription,
                ShortDescriptionEn = product.ShortDescriptionEn,
                ShortDescriptionAr = product.ShortDescriptionAr,
                LongDescription = product.LongDescription,
                TechnicalSpifications = product.TechnicalSpifications,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                ImageThumbnailUrl = product.ImageThumbnailUrl,
                ImageFileName = product.ImageFileName,
                IsProductOfTheWeek = product.IsProductOfTheWeek,
                InStock = product.InStock,
                CategoryId = product.CategoryId,
                Categories = _categoryRepository.AllCategories.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(AdminProductEditViewModel model, IFormFile? imageFile)
        {
            var guard = EnsureAdmin();
            if (guard is not null)
            {
                return guard;
            }

            if (!ModelState.IsValid)
            {
                model.Categories = _categoryRepository.AllCategories.ToList();
                return View(model);
            }

            var product = _productRepository.GetProductById(model.ProductId);
            if (product is null)
            {
                return RedirectToAction(nameof(Products));
            }

            // Gérer l'upload d'image
            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = await SaveUploadedImageAsync(imageFile);
                if (fileName == null)
                {
                    ModelState.AddModelError("imageFile", "Erreur lors du téléchargement de l'image. Assurez-vous que le fichier est valide et ne dépasse pas 5MB.");
                    model.Categories = _categoryRepository.AllCategories.ToList();
                    return View(model);
                }
                product.ImageFileName = fileName;
            }

            product.Name = model.Name;
            product.NameEn = model.NameEn;
            product.NameAr = model.NameAr;
            product.ShortDescription = model.ShortDescription;
            product.ShortDescriptionEn = model.ShortDescriptionEn;
            product.ShortDescriptionAr = model.ShortDescriptionAr;
            product.LongDescription = model.LongDescription;
            product.TechnicalSpifications = model.TechnicalSpifications;
            product.Price = model.Price;
            product.ImageUrl = model.ImageUrl;
            product.ImageThumbnailUrl = model.ImageThumbnailUrl;
            product.IsProductOfTheWeek = model.IsProductOfTheWeek;
            product.InStock = model.InStock;
            product.CategoryId = model.CategoryId;

            _productRepository.UpdateProduct(product);

            return RedirectToAction(nameof(Products));
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            var guard = EnsureAdmin();
            if (guard is not null)
            {
                return guard;
            }

            var viewModel = new AdminProductEditViewModel
            {
                Categories = _categoryRepository.AllCategories.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(AdminProductEditViewModel model, IFormFile? imageFile)
        {
            var guard = EnsureAdmin();
            if (guard is not null)
            {
                return guard;
            }

            if (!ModelState.IsValid)
            {
                model.Categories = _categoryRepository.AllCategories.ToList();
                return View(model);
            }

            // Gérer l'upload d'image
            string? fileName = null;
            if (imageFile != null && imageFile.Length > 0)
            {
                fileName = await SaveUploadedImageAsync(imageFile);
                if (fileName == null)
                {
                    ModelState.AddModelError("imageFile", "Erreur lors du téléchargement de l'image. Assurez-vous que le fichier est valide et ne dépasse pas 5MB.");
                    model.Categories = _categoryRepository.AllCategories.ToList();
                    return View(model);
                }
            }
            else if (!string.IsNullOrEmpty(model.ImageFileName))
            {
                fileName = model.ImageFileName;
            }

            var product = new Product
            {
                Name = model.Name,
                NameEn = model.NameEn,
                NameAr = model.NameAr,
                ShortDescription = model.ShortDescription,
                ShortDescriptionEn = model.ShortDescriptionEn,
                ShortDescriptionAr = model.ShortDescriptionAr,
                LongDescription = model.LongDescription,
                TechnicalSpifications = model.TechnicalSpifications,
                Price = model.Price,
                ImageUrl = model.ImageUrl,
                ImageThumbnailUrl = model.ImageThumbnailUrl,
                ImageFileName = fileName,
                IsProductOfTheWeek = model.IsProductOfTheWeek,
                InStock = model.InStock,
                CategoryId = model.CategoryId
            };

            _productRepository.AddProduct(product);

            return RedirectToAction(nameof(Products));
        }

        [HttpGet]
        public IActionResult ViewOrder(Guid orderId)
        {
            var guard = EnsureAdmin();
            if (guard is not null)
            {
                return guard;
            }

            var order = _orderRepository.GetById(orderId);
            if (order == null)
            {
                return RedirectToAction(nameof(Dashboard));
            }

            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadInvoice(Guid orderId)
        {
            var guard = EnsureAdmin();
            if (guard is not null)
            {
                return guard;
            }

            var order = _orderRepository.GetById(orderId);
            if (order == null)
                return NotFound();

            var pdfBytes = await _invoicePdfService.GenerateInvoicePdfAsync(order);
            return File(pdfBytes, "application/pdf", $"Facture_{order.OrderId:N}.pdf");
        }
    }
}

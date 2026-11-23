using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using TechShop.Models;
using TechShop.ViewModels;

namespace TechShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IAuthService _authService;
        private readonly IInvoicePdfService _invoicePdfService;

        public OrderController(IOrderRepository orderRepository, IAuthService authService, IInvoicePdfService invoicePdfService)
        {
            _orderRepository = orderRepository;
            _authService = authService;
            _invoicePdfService = invoicePdfService;
        }

        [HttpGet]
        public IActionResult MyOrders()
        {
            var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            if (!_authService.IsAuthenticated())
            {
                var returnUrl = Url.Action(nameof(MyOrders), "Order");
                return RedirectToAction("Login", "Account", new { returnUrl });
            }

            var email = _authService.GetCurrentUserEmail() ?? string.Empty;
            var orders = _orderRepository.GetByCustomerEmail(email);

            var viewModel = new CustomerOrdersViewModel
            {
                Orders = orders
            };

            return View(viewModel);
        }

        [HttpGet]
        [Obsolete("Use Admin/DownloadInvoice instead. Invoice download is now restricted to admin users.")]
        public async Task<IActionResult> DownloadInvoice(Guid orderId)
        {
            // Facture a été déplacée vers l'Admin
            return RedirectToAction("Dashboard", "Admin");
        }
    }
}

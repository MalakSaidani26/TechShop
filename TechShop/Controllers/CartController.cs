using Microsoft.AspNetCore.Mvc;
using TechShop.Models;
using TechShop.ViewModels;

namespace TechShop.Controllers
{
    public class CartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IAuthService _authService;
        private readonly IOrderRepository _orderRepository;

        public CartController(IShoppingCartService shoppingCartService, IAuthService authService, IOrderRepository orderRepository)
        {
            _shoppingCartService = shoppingCartService;
            _authService = authService;
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var viewModel = new CartViewModel
            {
                Items = _shoppingCartService.GetItems(),
                TotalItemsCount = _shoppingCartService.GetTotalItemsCount(),
                TotalAmount = _shoppingCartService.GetTotalAmount()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Remove(int productId)
        {
            _shoppingCartService.RemoveFromCart(productId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Clear()
        {
            _shoppingCartService.Clear();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult ContinuePurchase()
        {
            if (!_authService.IsAuthenticated())
            {
                var returnUrl = Url.Action(nameof(ContinuePurchase), "Cart");
                TempData["AuthRequiredMessage"] = "Veuillez vous connecter ou créer un compte pour continuer vos achats.";
                return RedirectToAction("Login", "Account", new { returnUrl });
            }

            var userName = _authService.GetCurrentUserName() ?? "Client";
            var userEmail = _authService.GetCurrentUserEmail() ?? "";
            var items = _shoppingCartService.GetItems();

            if (!items.Any())
            {
                TempData["CheckoutMessage"] = "Votre panier est vide.";
                return RedirectToAction(nameof(Index));
            }

            var order = _orderRepository.Create(userName, userEmail, items);
            _shoppingCartService.Clear();
            TempData["CheckoutMessage"] = $"Commande créée. Référence: {order.OrderId}";
            return RedirectToAction("MyOrders", "Order");
        }
    }
}

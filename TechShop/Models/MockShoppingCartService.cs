using System.Text.Json;

namespace TechShop.Models
{
    public class MockShoppingCartService : IShoppingCartService
    {
        private const string CartSessionKey = "CartItems";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MockShoppingCartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IReadOnlyList<CartItem> GetItems()
        {
            return GetCartItems();
        }

        public void AddToCart(Product product, int quantity = 1)
        {
            if (quantity <= 0)
            {
                return;
            }

            var items = GetCartItems();
            var existingItem = items.FirstOrDefault(i => i.ProductId == product.ProductId);

            if (existingItem is null)
            {
                items.Add(new CartItem
                {
                    ProductId = product.ProductId,
                    ProductName = product.Name,
                    UnitPrice = product.Price,
                    Quantity = quantity,
                    ImageFileName = product.ImageFileName
                });
            }
            else
            {
                existingItem.Quantity += quantity;
            }

            SaveCartItems(items);
        }

        public void RemoveFromCart(int productId)
        {
            var items = GetCartItems();
            items.RemoveAll(i => i.ProductId == productId);
            SaveCartItems(items);
        }

        public void Clear()
        {
            SaveCartItems(new List<CartItem>());
        }

        public int GetTotalItemsCount()
        {
            return GetCartItems().Sum(i => i.Quantity);
        }

        public decimal GetTotalAmount()
        {
            return GetCartItems().Sum(i => i.TotalPrice);
        }

        private List<CartItem> GetCartItems()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            var json = session?.GetString(CartSessionKey);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<CartItem>();
            }

            return JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
        }

        private void SaveCartItems(List<CartItem> items)
        {
            var session = _httpContextAccessor.HttpContext?.Session;

            if (session is null)
            {
                return;
            }

            var json = JsonSerializer.Serialize(items);
            session.SetString(CartSessionKey, json);
        }
    }
}

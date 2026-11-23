using System.Text.Json;

namespace TechShop.Models
{
    public class MockOrderRepository : IOrderRepository
    {
        private const string OrdersKey = "MockOrders";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MockOrderRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IReadOnlyList<Order> GetAll() => GetOrders();

        public IReadOnlyList<Order> GetByCustomerEmail(string customerEmail) =>
            GetOrders()
                .Where(o => string.Equals(o.CustomerEmail, customerEmail, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(o => o.CreatedAt)
                .ToList();

        public Order? GetById(Guid orderId) => GetOrders().FirstOrDefault(o => o.OrderId == orderId);

        public Order Create(string customerName, string customerEmail, IReadOnlyList<CartItem> cartItems)
        {
            var orders = GetOrders();
            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                CustomerName = customerName,
                CustomerEmail = customerEmail,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.Now,
                Items = cartItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity
                }).ToList()
            };

            orders.Add(order);
            SaveOrders(orders);
            return order;
        }

        public void Validate(Guid orderId)
        {
            var orders = GetOrders();
            var order = orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order is null)
            {
                return;
            }

            order.Status = OrderStatus.Validated;
            SaveOrders(orders);
        }

        public void Delete(Guid orderId)
        {
            var orders = GetOrders();
            var order = orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order is null)
            {
                return;
            }

            orders.Remove(order);
            SaveOrders(orders);
        }

        private List<Order> GetOrders()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            var json = session?.GetString(OrdersKey);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<Order>();
            }

            return JsonSerializer.Deserialize<List<Order>>(json) ?? new List<Order>();
        }

        private void SaveOrders(List<Order> orders)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session is null)
            {
                return;
            }

            var json = JsonSerializer.Serialize(orders);
            session.SetString(OrdersKey, json);
        }
    }
}

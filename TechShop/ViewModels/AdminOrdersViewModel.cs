using TechShop.Models;

namespace TechShop.ViewModels
{
    public class AdminOrdersViewModel
    {
        public IReadOnlyList<Order> Orders { get; init; } = new List<Order>();
    }
}

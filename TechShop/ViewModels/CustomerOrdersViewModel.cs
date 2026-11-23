using TechShop.Models;

namespace TechShop.ViewModels
{
    public class CustomerOrdersViewModel
    {
        public IReadOnlyList<Order> Orders { get; init; } = new List<Order>();
    }
}

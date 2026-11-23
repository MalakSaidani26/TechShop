using TechShop.Models;

namespace TechShop.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalOrders { get; init; }
        public int PendingOrders { get; init; }
        public int ValidatedOrders { get; init; }
        public int TotalCustomers { get; init; }
        public decimal TotalRevenue { get; init; }
        public IReadOnlyList<Order> LatestOrders { get; init; } = new List<Order>();
    }
}

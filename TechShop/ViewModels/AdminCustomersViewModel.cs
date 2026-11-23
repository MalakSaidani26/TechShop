using TechShop.Models;

namespace TechShop.ViewModels
{
    public class AdminCustomersViewModel
    {
        public IReadOnlyList<CustomerInfo> Customers { get; init; } = new List<CustomerInfo>();
    }
}

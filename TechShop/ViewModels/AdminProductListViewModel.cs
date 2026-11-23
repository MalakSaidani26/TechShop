using TechShop.Models;

namespace TechShop.ViewModels
{
    public class AdminProductListViewModel
    {
        public IReadOnlyList<Product> Products { get; init; } = new List<Product>();
    }
}

using TechShop.Models;

namespace TechShop.ViewModels
{
    public class CartViewModel
    {
        public IReadOnlyList<CartItem> Items { get; init; } = new List<CartItem>();
        public int TotalItemsCount { get; init; }
        public decimal TotalAmount { get; init; }
    }
}

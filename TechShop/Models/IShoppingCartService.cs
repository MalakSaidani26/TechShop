namespace TechShop.Models
{
    public interface IShoppingCartService
    {
        IReadOnlyList<CartItem> GetItems();
        void AddToCart(Product product, int quantity = 1);
        void RemoveFromCart(int productId);
        void Clear();
        int GetTotalItemsCount();
        decimal GetTotalAmount();
    }
}

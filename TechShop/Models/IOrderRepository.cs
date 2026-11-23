namespace TechShop.Models
{
    public interface IOrderRepository
    {
        IReadOnlyList<Order> GetAll();
        IReadOnlyList<Order> GetByCustomerEmail(string customerEmail);
        Order? GetById(Guid orderId);
        Order Create(string customerName, string customerEmail, IReadOnlyList<CartItem> cartItems);
        void Validate(Guid orderId);
        void Delete(Guid orderId);
    }
}

namespace TechShop.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public decimal TotalAmount => Items.Sum(i => i.TotalPrice);
    }
}

namespace TechShop.Models
{
    public interface IProductRepository
    {
        IEnumerable<Product> AllProducts { get; }
        IEnumerable<Product> ProductsOfTheWeek { get; }
        Product? GetProductById(int productId);
        IEnumerable<Product> SearchProducts(string searchQuery, string languageCode);
        void UpdateProduct(Product product);
        void AddProduct(Product product);
    }
}

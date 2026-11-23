using TechShop.Models;

namespace TechShop.ViewModels
{
    public class ProductListViewModel
    {
        public IEnumerable<Product> Products { get; }
        public IEnumerable<Category> Categories { get; }
        public string? CurrentCategory { get; }
        public int? SelectedCategoryId { get; }

        public ProductListViewModel(IEnumerable<Product> products, IEnumerable<Category> categories, string? currentCategory, int? selectedCategoryId)
        {
            Products = products;
            Categories = categories;
            CurrentCategory = currentCategory;
            SelectedCategoryId = selectedCategoryId;
        }
    }
}

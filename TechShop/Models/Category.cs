namespace TechShop.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? CategoryNameEn { get; set; }
        public string? CategoryNameAr { get; set; }
        public string? Description { get; set; }
        public List<Product>? Products { get; set; }

        public string GetLocalizedName(string languageCode) => languageCode switch
        {
            "en" => CategoryNameEn ?? CategoryName,
            "ar" => CategoryNameAr ?? CategoryName,
            _ => CategoryName
        };
    }
}

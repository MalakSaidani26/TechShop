namespace TechShop.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? NameEn { get; set; }
        public string? NameAr { get; set; }
        public string? ShortDescription { get; set; }
        public string? ShortDescriptionEn { get; set; }
        public string? ShortDescriptionAr { get; set; }
        public string? LongDescription { get; set; }
        public string? TechnicalSpifications { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageThumbnailUrl { get; set; }
        public string? ImageFileName { get; set; }
        public bool IsProductOfTheWeek { get; set; }
        public bool InStock { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!;

        public string GetLocalizedName(string languageCode) => languageCode switch
        {
            "en" => NameEn ?? Name,
            "ar" => NameAr ?? Name,
            _ => Name
        };

        public string GetLocalizedShortDescription(string languageCode) => languageCode switch
        {
            "en" => ShortDescriptionEn ?? ShortDescription ?? string.Empty,
            "ar" => ShortDescriptionAr ?? ShortDescription ?? string.Empty,
            _ => ShortDescription ?? string.Empty
        };
    }
}

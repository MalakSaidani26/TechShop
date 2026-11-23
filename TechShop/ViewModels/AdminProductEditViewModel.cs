using System.ComponentModel.DataAnnotations;
using TechShop.Models;

namespace TechShop.ViewModels
{
    public class AdminProductEditViewModel
    {
        public int ProductId { get; init; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? NameEn { get; set; }
        public string? NameAr { get; set; }

        public string? ShortDescription { get; set; }
        public string? ShortDescriptionEn { get; set; }
        public string? ShortDescriptionAr { get; set; }
        public string? LongDescription { get; set; }
        public string? TechnicalSpifications { get; set; }

        [Range(0, 100000)]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }
        public string? ImageThumbnailUrl { get; set; }
        public string? ImageFileName { get; set; }

        public bool IsProductOfTheWeek { get; set; }
        public bool InStock { get; set; }

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        public IReadOnlyList<Category> Categories { get; set; } = new List<Category>();
    }
}

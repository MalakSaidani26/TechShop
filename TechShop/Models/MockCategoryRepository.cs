namespace TechShop.Models
{
    public class MockCategoryRepository : ICategoryRepository
    {
        public IEnumerable<Category> AllCategories =>
        new List<Category>
        {
            new Category { CategoryId = 1, CategoryName = "Smartphones", CategoryNameEn = "Smartphones", CategoryNameAr = "الهواتف الذكية", Description = "Derniers smartphones et accessoires" },
            new Category { CategoryId = 2, CategoryName = "Ordinateurs portables", CategoryNameEn = "Laptops", CategoryNameAr = "الحواسيب المحمولة", Description = "Ordinateurs puissants pour le travail et le gaming" },
            new Category { CategoryId = 3, CategoryName = "Audio", CategoryNameEn = "Audio", CategoryNameAr = "الصوتيات", Description = "Casques, haut-parleurs et équipements hi-fi" },
            new Category { CategoryId = 4, CategoryName = "Gaming", CategoryNameEn = "Gaming", CategoryNameAr = "الألعاب", Description = "Consoles, manettes et accessoires de jeu" },
            new Category { CategoryId = 5, CategoryName = "Accessoires", CategoryNameEn = "Accessories", CategoryNameAr = "الإكسسوارات", Description = "Chargeurs, câbles et accessoires divers" },
            new Category { CategoryId = 6, CategoryName = "Tablettes", CategoryNameEn = "Tablets", CategoryNameAr = "الأجهزة اللوحية", Description = "Tablettes pour le travail et le divertissement" },
            new Category { CategoryId = 7, CategoryName = "Montres connectées", CategoryNameEn = "Smart Watches", CategoryNameAr = "الساعات الذكية", Description = "Smartwatches et bracelets connectés" },
            new Category { CategoryId = 8, CategoryName = "Stockage", CategoryNameEn = "Storage", CategoryNameAr = "التخزين", Description = "Disques durs, SSD et solutions de stockage" }
        };

    }
}

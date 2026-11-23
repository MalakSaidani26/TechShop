namespace TechShop.Models
{
    public class MockProductRepository : IProductRepository
    {
        private readonly ICategoryRepository _categoryRepository = new MockCategoryRepository();
        private static readonly List<Product> _products =
            new List<Product>
            {
                // Smartphones
                new Product {
                    ProductId = 101,
                    Name = "Apple iPhone 15 128Go",
                    NameEn = "Apple iPhone 15 128GB",
                    NameAr = "آبل آيفون 15 سعة 128 جيجا",
                    Price = 3499.00M,
                    ShortDescription = "Smartphone Apple avec écran Super Retina.",
                    ShortDescriptionEn = "Apple smartphone with Super Retina display.",
                    ShortDescriptionAr = "هاتف آبل بشاشة سوبر ريتينا.",
                    LongDescription = "iPhone 15 avec 128Go de stockage, appareil photo performant et autonomie solide.",
                    TechnicalSpifications = "Écran: 6.1\" | Stockage: 128Go | Caméra: 48MP",
                    CategoryId = 1,
                    Category = new MockCategoryRepository().AllCategories.First(c => c.CategoryId == 1),
                    ImageFileName = "iphone-15-128go-noir-apple-a.jpg",
                    InStock = true,
                    IsProductOfTheWeek = true
                },
                new Product {
                    ProductId = 102,
                    Name = "Samsung Galaxy S25 Ultra 5G",
                    NameEn = "Samsung Galaxy S25 Ultra 5G",
                    NameAr = "سامسونج جالاكسي S25 ألترا 5G",
                    Price = 4999.00M,
                    ShortDescription = "Smartphone Samsung haut de gamme 5G.",
                    ShortDescriptionEn = "Premium Samsung 5G smartphone.",
                    ShortDescriptionAr = "هاتف سامسونج 5G من الفئة العليا.",
                    LongDescription = "Galaxy S25 Ultra avec couleurs éclatantes, performances puissantes et espace de stockage ample.",
                    TechnicalSpifications = "Écran: 6.8\" | RAM: 12Go | Stockage: 256Go",
                    CategoryId = 1,
                    Category = new MockCategoryRepository().AllCategories.First(c => c.CategoryId == 1),
                    ImageFileName = "smartphone-samsung-galaxy-s25-ultra-5g-12go-256go-silver-001.jpg",
                    InStock = true,
                    IsProductOfTheWeek = true
                },

                // PC Gamer & Informatique
                new Product {
                    ProductId = 201,
                    Name = "Pc de Bureau Gamer i5 10è Gén 16Go RTX 3050 6G",
                    NameEn = "Gaming Desktop i5 10th Gen 16GB RTX 3050 6GB",
                    NameAr = "حاسوب ألعاب مكتبي i5 الجيل العاشر 16 جيجا RTX 3050",
                    Price = 1699.00M,
                    ShortDescription = "Tour PC gamer avec RTX 3050.",
                    ShortDescriptionEn = "Gaming desktop tower with RTX 3050.",
                    ShortDescriptionAr = "حاسوب ألعاب مكتبي مع RTX 3050.",
                    LongDescription = "Configuration gamer solide avec processeur Intel i5 et carte graphique NVIDIA RTX 3050.",
                    TechnicalSpifications = "CPU: Intel i5 | GPU: RTX 3050 | RAM: 16Go | SSD: 512Go",
                    CategoryId = 2,
                    Category = new MockCategoryRepository().AllCategories.First(c => c.CategoryId == 2),
                    ImageFileName = "pc-de-bureau-gamer-mytek-i5-10e-gen-16go-rtx-3050-6g.jpg",
                    InStock = true,
                    IsProductOfTheWeek = false
                },
                new Product {
                    ProductId = 202,
                    Name = "PC Portable Gamer MSI Thin 15 B13UDX i5 13è Gén 24G RTX3050",
                    NameEn = "MSI Thin 15 B13UDX Gaming Laptop i5 13th Gen 24GB RTX 3050",
                    NameAr = "لابتوب ألعاب MSI Thin 15 B13UDX i5 الجيل 13 24 جيجا RTX 3050",
                    Price = 2999.00M,
                    ShortDescription = "PC Gamer haut de gamme avec Ryzen 7.",
                    ShortDescriptionEn = "High-end gaming laptop with Ryzen 7.",
                    ShortDescriptionAr = "لابتوب ألعاب قوي بمعالج Ryzen 7.",
                    LongDescription = "PC gamer puissant avec processeur AMD Ryzen 7 et GPU RTX 5060 pour jeux exigeants.",
                    TechnicalSpifications = "CPU: AMD Ryzen 7 | GPU: RTX 5060 | RAM: 32Go | SSD: 1To",
                    CategoryId = 2,
                    Category = new MockCategoryRepository().AllCategories.First(c => c.CategoryId == 2),
                    ImageFileName = "pc-portable-gamer-msi-thin-15-b13udx-i5-13e-gen-24g-rtx3050-b.jpg",
                    InStock = true,
                    IsProductOfTheWeek = false
                },

                // Accessoires
                new Product {
                    ProductId = 301,
                    Name = "Sac à dos 15.6\"",
                    NameEn = "15.6\" Backpack",
                    NameAr = "حقيبة ظهر 15.6 بوصة",
                    Price = 49.00M,
                    ShortDescription = "Sac à dos pour PC portable.",
                    ShortDescriptionEn = "Backpack for laptop.",
                    ShortDescriptionAr = "حقيبة ظهر للابتوب.",
                    LongDescription = "Sac à dos léger et pratique compatible avec PC portables jusqu'à 15.6\".",
                    TechnicalSpifications = "Capacité: 15.6\" | Design: Noir et Rouge",
                    CategoryId = 5,
                    Category = new MockCategoryRepository().AllCategories.First(c => c.CategoryId == 5),
                    ImageFileName = "sac-a-dos-pour-pc-portable-15-6-pl-power-marron-e.jpg",
                    InStock = true,
                    IsProductOfTheWeek = false
                },
                new Product {
                    ProductId = 302,
                    Name = "Sacoche PC 15.6\"",
                    NameEn = "15.6\" Laptop Bag",
                    NameAr = "حقيبة لابتوب 15.6 بوصة",
                    Price = 19.00M,
                    ShortDescription = "Sacoche simple pour PC.",
                    ShortDescriptionEn = "Simple laptop carrying bag.",
                    ShortDescriptionAr = "حقيبة بسيطة لحمل اللابتوب.",
                    LongDescription = "Sacoche légère pour transporter votre PC portable en toute sécurité.",
                    TechnicalSpifications = "Taille: 15.6\" | Couleur: Noir",
                    CategoryId = 5,
                    Category = new MockCategoryRepository().AllCategories.First(c => c.CategoryId == 5),
                    ImageFileName = "sacoche-pour-pc-portable-15-6-pouces-gris.jpg",
                    InStock = false,
                    IsProductOfTheWeek = false
                }
            };

        public IEnumerable<Product> AllProducts => _products;

        public IEnumerable<Product> ProductsOfTheWeek =>
            AllProducts.Where(p => p.IsProductOfTheWeek);

        public Product? GetProductById(int productId) =>
            AllProducts.FirstOrDefault(p => p.ProductId == productId);

        public IEnumerable<Product> SearchProducts(string searchQuery, string languageCode) =>
            AllProducts.Where(p => p.GetLocalizedName(languageCode).Contains(searchQuery, StringComparison.OrdinalIgnoreCase));

        public void UpdateProduct(Product product)
        {
            var existing = _products.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (existing is null)
            {
                return;
            }

            existing.Name = product.Name;
            existing.NameEn = product.NameEn;
            existing.NameAr = product.NameAr;
            existing.ShortDescription = product.ShortDescription;
            existing.ShortDescriptionEn = product.ShortDescriptionEn;
            existing.ShortDescriptionAr = product.ShortDescriptionAr;
            existing.LongDescription = product.LongDescription;
            existing.TechnicalSpifications = product.TechnicalSpifications;
            existing.Price = product.Price;
            existing.ImageUrl = product.ImageUrl;
            existing.ImageThumbnailUrl = product.ImageThumbnailUrl;
            existing.ImageFileName = product.ImageFileName;
            existing.IsProductOfTheWeek = product.IsProductOfTheWeek;
            existing.InStock = product.InStock;
            existing.CategoryId = product.CategoryId;
            existing.Category = _categoryRepository.AllCategories.FirstOrDefault(c => c.CategoryId == product.CategoryId) ?? existing.Category;
        }

        public void AddProduct(Product product)
        {
            var maxId = _products.Max(p => p.ProductId);
            product.ProductId = maxId + 1;
            product.Category = _categoryRepository.AllCategories.FirstOrDefault(c => c.CategoryId == product.CategoryId);
            _products.Add(product);
        }
    }
}
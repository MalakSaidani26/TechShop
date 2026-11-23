namespace TechShop.Models
{
    public class ImageService
    {
        private const string ProductImagesFolder = "images/products";
        private readonly string _webRootPath;

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webRootPath = webHostEnvironment.WebRootPath;
        }

        /// <summary>
        /// Génère les URLs de miniatures pour une image produit
        /// </summary>
        public (string thumbnail150, string thumbnail300) GenerateThumbnailUrls(string imageFileName)
        {
            if (string.IsNullOrEmpty(imageFileName))
                return (string.Empty, string.Empty);

            var nameWithoutExt = Path.GetFileNameWithoutExtension(imageFileName);
            var extension = Path.GetExtension(imageFileName);

            var thumbnail150 = $"/{ProductImagesFolder}/{nameWithoutExt}_thumb150{extension}";
            var thumbnail300 = $"/{ProductImagesFolder}/{nameWithoutExt}_thumb300{extension}";

            return (thumbnail150, thumbnail300);
        }

        /// <summary>
        /// Crée les miniatures si elles n'existent pas déjà
        /// </summary>
        public async Task<bool> CreateThumbnailsIfNeeded(string imageFileName)
        {
            if (string.IsNullOrEmpty(imageFileName))
                return false;

            try
            {
                var uploadPath = Path.Combine(_webRootPath, ProductImagesFolder);
                var originalPath = Path.Combine(uploadPath, imageFileName);

                if (!File.Exists(originalPath))
                    return false;

                var nameWithoutExt = Path.GetFileNameWithoutExtension(imageFileName);
                var extension = Path.GetExtension(imageFileName);

                // Vérifier si les miniatures existent déjà
                var thumb150Path = Path.Combine(uploadPath, $"{nameWithoutExt}_thumb150{extension}");
                var thumb300Path = Path.Combine(uploadPath, $"{nameWithoutExt}_thumb300{extension}");

                if (File.Exists(thumb150Path) && File.Exists(thumb300Path))
                    return true;

                // Pour cette version simple, nous copions juste l'image originale
                // En production, vous voudriez utiliser une librairie d'image pour redimensionner
                if (!File.Exists(thumb150Path))
                    File.Copy(originalPath, thumb150Path, true);

                if (!File.Exists(thumb300Path))
                    File.Copy(originalPath, thumb300Path, true);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Retourne l'URL complète de l'image
        /// </summary>
        public string GetImageUrl(string imageFileName)
        {
            if (string.IsNullOrEmpty(imageFileName))
                return "/images/products/placeholder.svg";

            return $"/{ProductImagesFolder}/{imageFileName}";
        }
    }
}

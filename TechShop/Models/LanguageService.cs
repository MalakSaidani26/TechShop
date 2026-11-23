using System.Globalization;

namespace TechShop.Models
{
    public class LanguageService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string LanguageCookieName = "TechShop_Language";
        private const int LanguageCookieExpirationDays = 30;

        public LanguageService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Obtient la langue actuelle du contexte HTTP
        /// </summary>
        public string GetCurrentLanguage()
        {
            var httpContext = _httpContextAccessor?.HttpContext;
            if (httpContext == null)
                return "fr";

            // 1. Vérifier les cookies
            if (httpContext.Request.Cookies.TryGetValue(LanguageCookieName, out var cookieLanguage))
            {
                if (IsValidLanguage(cookieLanguage))
                    return cookieLanguage;
            }

            // 2. Détecter la langue du navigateur
            var browserLanguage = GetBrowserLanguage(httpContext);
            if (!string.IsNullOrEmpty(browserLanguage) && IsValidLanguage(browserLanguage))
            {
                SetLanguageCookie(browserLanguage);
                return browserLanguage;
            }

            // 3. Utiliser la langue par défaut
            return "fr";
        }

        /// <summary>
        /// Définit la langue sélectionnée dans un cookie
        /// </summary>
        public void SetLanguageCookie(string languageCode)
        {
            if (!IsValidLanguage(languageCode))
                return;

            var httpContext = _httpContextAccessor?.HttpContext;
            if (httpContext == null)
                return;

            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(LanguageCookieExpirationDays),
                HttpOnly = true,
                Secure = httpContext.Request.IsHttps,
                SameSite = SameSiteMode.Lax
            };

            httpContext.Response.Cookies.Append(LanguageCookieName, languageCode, cookieOptions);
        }

        /// <summary>
        /// Détecte la langue du navigateur
        /// </summary>
        private string? GetBrowserLanguage(HttpContext httpContext)
        {
            var acceptLanguageHeader = httpContext.Request.Headers["Accept-Language"].ToString();
            if (string.IsNullOrEmpty(acceptLanguageHeader))
                return null;

            // Exemple: "en-US,en;q=0.9,fr;q=0.8"
            var languages = acceptLanguageHeader.Split(',')
                .Select(x => x.Trim().Split(';')[0].ToLowerInvariant())
                .ToList();

            foreach (var lang in languages)
            {
                // Essayer le code complet (ex: en-US)
                if (IsValidLanguage(lang))
                    return lang.Substring(0, 2);

                // Essayer le préfixe de langue
                var languagePrefix = lang.Substring(0, Math.Min(2, lang.Length));
                if (IsValidLanguage(languagePrefix))
                    return languagePrefix;
            }

            return null;
        }

        /// <summary>
        /// Valide si la langue est supportée
        /// </summary>
        private bool IsValidLanguage(string languageCode)
        {
            return languageCode switch
            {
                "fr" or "en" or "ar" => true,
                _ => false
            };
        }
    }
}

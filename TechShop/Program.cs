using TechShop.Models;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddLocalization();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

// Enregistrer les services métier
builder.Services.AddScoped<ICategoryRepository, MockCategoryRepository>();
builder.Services.AddScoped<IProductRepository, MockProductRepository>();
builder.Services.AddScoped<IShoppingCartService, MockShoppingCartService>();
builder.Services.AddScoped<IAuthService, MockAuthService>();
builder.Services.AddScoped<IOrderRepository, MockOrderRepository>();
builder.Services.AddScoped<LanguageService>();
builder.Services.AddScoped<IInvoicePdfService, InvoicePdfService>();
builder.Services.AddScoped<ImageService>();

// Enregistrer le service ChatBot
// Trois options disponibles:
// 1. MOCK_MODE: Mode Mock 100% gratuit (réponses pré-définies)
// 2. OpenAI: API OpenAI (nécessite crédits API)
// 3. Deepseek: API Deepseek via TeamoRouter (gratuit avec clé fournie)

if (builder.Environment.IsDevelopment())
{
    var apiKey = builder.Configuration["OpenAI:ApiKey"];
    var deepseekKey = builder.Configuration["Deepseek:ApiKey"];

    // Priorité 1: Mode Mock si clé OpenAI contient "MOCK"
    if (!string.IsNullOrEmpty(apiKey) && apiKey.Contains("MOCK"))
    {
        builder.Services.AddScoped<IChatBotService, MockChatBotService>();
        Console.WriteLine("✅ Mode Mock ChatBot Activé (100% Gratuit - Aucune API requise)");
    }
    // Priorité 2: Deepseek si clé fournie
    else if (!string.IsNullOrEmpty(deepseekKey) && !deepseekKey.StartsWith("YOUR_"))
    {
        builder.Services.AddHttpClient<IChatBotService, DeepseekChatBotService>();
        Console.WriteLine("✅ Mode Deepseek ChatBot Activé (Gratuit via TeamoRouter)");
    }
    // Priorité 3: OpenAI si clé valide
    else if (!string.IsNullOrEmpty(apiKey) && !apiKey.StartsWith("YOUR_"))
    {
        builder.Services.AddHttpClient<IChatBotService, ChatBotService>();
        Console.WriteLine("✅ Mode OpenAI ChatBot Activé (Crédits Gratuits)");
    }
    // Fallback: Mode Mock si rien n'est configuré
    else
    {
        builder.Services.AddScoped<IChatBotService, MockChatBotService>();
        Console.WriteLine("✅ Mode Mock ChatBot Activé (Aucune clé API trouvée)");
    }
}
else
{
    // En production, essayer d'utiliser Deepseek d'abord, puis OpenAI
    var deepseekKey = builder.Configuration["Deepseek:ApiKey"];
    if (!string.IsNullOrEmpty(deepseekKey))
    {
        builder.Services.AddHttpClient<IChatBotService, DeepseekChatBotService>();
    }
    else
    {
        builder.Services.AddHttpClient<IChatBotService, ChatBotService>();
    }
}

var app = builder.Build();

var supportedCultures = new[] { new CultureInfo("fr"), new CultureInfo("en"), new CultureInfo("ar") };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("fr"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

localizationOptions.RequestCultureProviders =
[
    new CookieRequestCultureProvider()
];

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRequestLocalization(localizationOptions);
app.UseStaticFiles();
app.UseSession();

app.MapRazorPages();
app.MapDefaultControllerRoute();

app.Run();

# 🛒 TechShop - E-Commerce

Une **boutique en ligne simple et moderne** avec ASP.NET Core 10, Razor Pages, panier, commandes, admin et chatbot IA.

[🔗 Voir la démo](#) | [📋 Fonctionnalités](#-fonctionnalités) | [🚀 Démarrage rapide](#-démarrage-rapide) | [📧 Support](#-support)

---

## ⭐ Fonctionnalités

### Pour les Clients
- 🛍️ **Catalogue** - Parcourir les produits
- 🛒 **Panier** - Ajouter/supprimer articles
- 📦 **Commandes** - Passer commande et voir l'historique
- 💬 **ChatBot IA** - Support client gratuit (réponses intelligentes)
- 🌍 **3 Langues** - Français, Anglais, Arabe
- 🎨 **Mode Sombre** - Toggle jour/nuit

### Pour l'Admin
- 📊 **Dashboard** - Vue d'ensemble
- ➕ **Gérer produits** - Ajouter/modifier/supprimer
- 👥 **Voir clients** - Consulter les inscriptions
- 📋 **Voir commandes** - Tous les détails
- 📄 **PDF** - Factures automatiques

---

## 🚀 Démarrage Rapide

### Installation (3 étapes)

```bash
# 1. Clonez le repo
git clone https://github.com/votreusername/TechShop.git
cd TechShop

# 2. Restaurez les packages
dotnet restore

# 3. Lancez l'app
dotnet run
```

Ouvrez: `https://localhost:7xxx` (port dans la console)

### Comptes de Test

```
Admin:    admin@techshop.com / Admin123!
Client:   customer@techshop.com / Customer123!
```

---

## 🏗️ Architecture

### Comment ça Marche?

```
Utilisateur (Browser)
	   ↓
	Razor Pages/Controllers
	   ↓
	Services (Dependency Injection)
	   ↓
	Repositories (Mock ou Base de données)
	   ↓
	Models/Database
```

### Structure Dossiers

```
TechShop/
├── Controllers/          → Logique (7 contrôleurs)
├── Models/              → Données (20+ classes)
├── ViewModels/          → Modèles pour affichage
├── Views/               → Pages HTML (15+ pages)
│   ├── Home/            → Accueil, produits
│   ├── Cart/            → Panier
│   ├── Account/         → Login, Register
│   ├── Admin/           → Gestion admin
│   └── Shared/          → Layout principal
├── wwwroot/             → Fichiers statiques
│   ├── css/             → Styles
│   ├── js/              → JavaScript
│   └── images/          → Photos produits
├── Program.cs           → Configuration app
└── appsettings.json     → Paramètres
```

### Controllers (Logique)

| Controller | Responsabilité |
|-----------|-----------------|
| **HomeController** | Accueil, produits |
| **ProductController** | Détail produit |
| **CartController** | Panier |
| **OrderController** | Commandes client |
| **AccountController** | Connexion/Inscription |
| **AdminController** | Dashboard admin |
| **ChatBotController** | API ChatBot |

### Modèles Importants

```csharp
Product          → Produit (nom, prix, description)
Order            → Commande (numéro, date, articles)
OrderItem        → Élément d'une commande
Category         → Catégorie (Téléphones, Ordinateurs, etc)
Customer         → Client (email, nom)
CartItem         → Article dans le panier
```

### Services (Dépendance Injection)

L'app utilise **Dependency Injection** pour tout:

```csharp
// Dans Program.cs:
builder.Services.AddScoped<IProductRepository, MockProductRepository>();
builder.Services.AddScoped<IOrderRepository, MockOrderRepository>();
builder.Services.AddScoped<IChatBotService, MockChatBotService>();
// etc...
```

**Avantage:** Facile à tester et remplacer!

---

## 💬 ChatBot IA

### Mode Par Défaut: Mock Gratuit ✅
- Réponses pré-définies
- Mots-clés: `bonjour`, `prix`, `livraison`, `retour`, `paiement`
- Zéro coût, fonctionne hors-ligne

### Mode Optional: OpenAI Payant
Pour utiliser ChatGPT au lieu du mock:

1. **Obtenez une clé API** → [platform.openai.com](https://platform.openai.com)
2. **Modifiez** `appsettings.Development.json`:

```json
{
  "OpenAI": {
	"ApiKey": "sk-proj-YOUR_KEY_HERE",
	"Model": "gpt-3.5-turbo"
  }
}
```

Le chatbot deviendra intelligent et pourra répondre à n'importe quelle question!

---

## 📊 Données Simulées (Mode Mock)

L'app fonctionne **100% sans base de données**. Voici ce qui est pré-chargé:

- **6 Produits** - iPhone, PC, Laptop, Accessoires
- **3 Catégories** - Téléphones, Ordinateurs, Accessoires
- **Panier** - Stocké en mémoire
- **Commandes** - Fictives pour démo

**Pour la production:** Connectez une vraie BD (SQL Server, PostgreSQL, etc)

---

## 🌐 Langues & Thèmes

### Langues (3 supportées)
Cliquez le drapeau en haut:
- 🇫🇷 Français (défaut)
- 🇬🇧 English
- 🇸🇦 العربية

### Thème
Toggle jour/nuit en haut à droite:
- ☀️ Clair
- 🌙 Sombre

---

## ⚙️ Configuration

### appsettings.Development.json

```json
{
  "Logging": {
	"LogLevel": {
	  "Default": "Information"
	}
  },
  "OpenAI": {
	"ApiKey": "MOCK_MODE",    // ChatBot: "MOCK_MODE" ou clé OpenAI
	"Model": "mock"           // "mock" ou "gpt-3.5-turbo"
  }
}
```

### Variables d'Environnement

Vous pouvez aussi utiliser des variables système:

```bash
# Windows (PowerShell)
$env:OpenAI__ApiKey = "sk-proj-YOUR_KEY"
dotnet run

# Linux/Mac
export OpenAI__ApiKey="sk-proj-YOUR_KEY"
dotnet run
```

---

## 📦 Dépendances

- **ASP.NET Core 10** - Framework web
- **Bootstrap 5** - Design responsive
- **jQuery** - Interactions DOM
- **QuestPDF** - Génération PDF
- **System.Text.Json** - Sérialisation JSON (OpenAI)

---

## 🧪 Tester l'App

1. **Accueil** → Voir les 6 produits
2. **Produit** → Cliquer sur un article
3. **Panier** → Ajouter 2-3 produits, modifier quantité
4. **Commande** → Passer une commande, PDF téléchargé
5. **ChatBot** → Dire "Bonjour" (bulle bas-droit)
6. **Admin** → Connectez-vous, gérer les produits
7. **Langues** → Changer de langue
8. **Thème** → Basculer sombre/clair

---

## 🐛 Dépannage

| Problème | Solution |
|---------|----------|
| **App ne démarre pas** | `dotnet clean && dotnet build` |
| **Port déjà utilisé** | Changer port dans `launchSettings.json` |
| **ChatBot erreur** | Vérifier `OpenAI:ApiKey = "MOCK_MODE"` dans config |
| **Images manquent** | Vérifier dossier `wwwroot/images/products/` |
| **Compilation échoue** | `dotnet restore` puis `dotnet build` |

---

## 🚀 Déploiement

### Local
```bash
dotnet run
```



---

## 📝 Code Style

Le code suit les **conventions Microsoft C#**:

```csharp
// ✅ Bon
public class ProductService
{
	private readonly IProductRepository _repository;

	public async Task<Product> GetProductAsync(int id)
	{
		return await _repository.GetAsync(id);
	}
}

// ❌ Mauvais
public class productService {
	private IProductRepository repository;
	public Product getProduct(int id) { return repository.Get(id); }
}
```

---



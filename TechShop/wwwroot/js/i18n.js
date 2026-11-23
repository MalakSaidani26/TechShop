// ========== TRANSLATIONS DICTIONARY ==========
window.translations = {
  fr: {
    products: 'Produits',
    our_products: 'Nos Produits',
    discover_tech: 'Découvrez nos derniers produits technologiques',
    search_product: 'Rechercher un produit...',
    search: 'Chercher',
    in_stock: '✓ En stock',
    out_of_stock: '✗ Hors stock',
    no_products: 'Aucun produit trouvé',
    footer_desc: 'Votre destination pour les meilleurs produits technologiques.',
    footer_copyright: '© {year} - TechShop. Tous droits réservés.',
    add_to_cart: 'Ajouter au panier',
    product_unavailable: 'Produit non disponible',
    toggle_theme: 'Basculer mode sombre/clair'
  },
  en: {
    products: 'Products',
    our_products: 'Our Products',
    discover_tech: 'Discover our latest technology products',
    search_product: 'Search for a product...',
    search: 'Search',
    in_stock: '✓ In stock',
    out_of_stock: '✗ Out of stock',
    no_products: 'No products found',
    footer_desc: 'Your destination for the best technology products.',
    footer_copyright: '© {year} - TechShop. All rights reserved.',
    add_to_cart: 'Add to cart',
    product_unavailable: 'Product unavailable',
    toggle_theme: 'Toggle dark/light mode'
  },
  ar: {
    products: 'المنتجات',
    our_products: 'منتجاتنا',
    discover_tech: 'اكتشف أحدث منتجاتنا التكنولوجية',
    search_product: 'ابحث عن منتج...',
    search: 'بحث',
    in_stock: '✓ متوفر',
    out_of_stock: '✗ غير متوفر',
    no_products: 'لم يتم العثور على منتجات',
    footer_desc: 'وجهتك للحصول على أفضل المنتجات التكنولوجية.',
    footer_copyright: '© {year} - TechShop. جميع الحقوق محفوظة.',
    add_to_cart: 'إضافة إلى السلة',
    product_unavailable: 'المنتج غير متوفر',
    toggle_theme: 'تبديل الوضع الداكن/الفاتح'
  }
};

window.productDescriptionTranslations = {
  en: {
    'Smartphone Apple avec écran Super Retina.': 'Apple smartphone with Super Retina display.',
    'Smartphone Samsung haut de gamme 5G.': 'Premium Samsung 5G smartphone.',
    'Tour PC gamer avec RTX 3050.': 'Gaming desktop PC with RTX 3050.',
    'PC Gamer haut de gamme avec Ryzen 7.': 'High-end gaming laptop with Ryzen 7.',
    'Sac à dos pour PC portable.': 'Backpack for laptop.',
    'Sacoche simple pour PC.': 'Simple laptop bag.'
  },
  ar: {
    'Smartphone Apple avec écran Super Retina.': 'هاتف آبل بشاشة سوبر ريتينا.',
    'Smartphone Samsung haut de gamme 5G.': 'هاتف سامسونج 5G من الفئة العليا.',
    'Tour PC gamer avec RTX 3050.': 'حاسوب مكتبي للألعاب مع RTX 3050.',
    'PC Gamer haut de gamme avec Ryzen 7.': 'حاسوب ألعاب عالي الأداء بمعالج Ryzen 7.',
    'Sac à dos pour PC portable.': 'حقيبة ظهر للابتوب.',
    'Sacoche simple pour PC.': 'حقيبة بسيطة للابتوب.'
  }
};

window.productNameTranslations = {
  en: {
    'Apple iPhone 15 128Go': 'Apple iPhone 15 128GB',
    'Samsung Galaxy S25 Ultra 5G': 'Samsung Galaxy S25 Ultra 5G',
    'Pc de Bureau Gamer MYTEK i5 10è Gén 16Go RTX 3050 6G': 'MYTEK Gaming Desktop PC i5 10th Gen 16GB RTX 3050 6GB',
    'PC Portable Gamer MSI Thin 15 B13UDX i5 13è Gén 24G RTX3050': 'MSI Thin 15 B13UDX Gaming Laptop i5 13th Gen 24GB RTX3050',
    'Sac à dos 15.6"': '15.6" Backpack',
    'Sacoche PC 15.6"': '15.6" Laptop Bag'
  },
  ar: {
    'Apple iPhone 15 128Go': 'آبل آيفون 15 سعة 128 جيجابايت',
    'Samsung Galaxy S25 Ultra 5G': 'سامسونج جالاكسي S25 ألترا 5G',
    'Pc de Bureau Gamer MYTEK i5 10è Gén 16Go RTX 3050 6G': 'حاسوب مكتبي للألعاب MYTEK بمعالج i5 الجيل العاشر وذاكرة 16 جيجابايت RTX 3050 6G',
    'PC Portable Gamer MSI Thin 15 B13UDX i5 13è Gén 24G RTX3050': 'حاسوب محمول للألعاب MSI Thin 15 B13UDX بمعالج i5 الجيل 13 وذاكرة 24 جيجابايت RTX3050',
    'Sac à dos 15.6"': 'حقيبة ظهر 15.6 بوصة',
    'Sacoche PC 15.6"': 'حقيبة لابتوب 15.6 بوصة'
  }
};

window.categoryTranslations = {
  en: {
    'Smartphones': 'Smartphones',
    'Ordinateurs portables': 'Laptops',
    'Audio': 'Audio',
    'Gaming': 'Gaming',
    'Accessoires': 'Accessories',
    'Tablettes': 'Tablets',
    'Montres connectées': 'Smartwatches',
    'Stockage': 'Storage'
  },
  ar: {
    'Smartphones': 'الهواتف الذكية',
    'Ordinateurs portables': 'الحواسيب المحمولة',
    'Audio': 'الصوتيات',
    'Gaming': 'الألعاب',
    'Accessoires': 'الملحقات',
    'Tablettes': 'الأجهزة اللوحية',
    'Montres connectées': 'الساعات الذكية',
    'Stockage': 'التخزين'
  }
};

// ========== GET CURRENT LANGUAGE ==========
function getCurrentLanguage() {
  var saved = localStorage.getItem('language');
  return saved || 'fr';
}

// ========== SET LANGUAGE ==========
function setLanguage(lang) {
  localStorage.setItem('language', lang);
  document.documentElement.lang = lang;
  document.documentElement.dir = (lang === 'ar') ? 'rtl' : 'ltr';
  if (document.body) {
    document.body.dir = (lang === 'ar') ? 'rtl' : 'ltr';
  }

  var selector = document.getElementById('languageSelect');
  if (selector) {
    selector.value = lang;
  }

  var currentText = document.getElementById('languageCurrentText');
  var currentFlag = document.querySelector('.language-current-flag');
  var languageItems = document.querySelectorAll('.language-option');
  var selectedItem = document.querySelector('.language-option[data-lang="' + lang + '"]');

  languageItems.forEach(function(item) {
    item.classList.remove('active');
  });

  if (selectedItem) {
    selectedItem.classList.add('active');

    if (currentText) {
      currentText.textContent = selectedItem.getAttribute('data-label') || selectedItem.textContent.trim();
    }

    if (currentFlag) {
      var flagCode = selectedItem.getAttribute('data-flag') || 'fr';
      currentFlag.className = 'language-current-flag flag flag-' + flagCode;
    }
  }

  updateTranslations(lang);
}

// ========== UPDATE TRANSLATIONS ==========
function updateTranslations(lang) {
  var year = new Date().getFullYear().toString();
  var elements = document.querySelectorAll('[data-i18n]');

  elements.forEach(function(element) {
    var key = element.getAttribute('data-i18n');
    var text = window.translations[lang] && window.translations[lang][key];
    if (text) {
      element.textContent = text.replace('{year}', year);
    }
  });

  var placeholders = document.querySelectorAll('[data-i18n-placeholder]');
  placeholders.forEach(function(element) {
    var key = element.getAttribute('data-i18n-placeholder');
    var text = window.translations[lang] && window.translations[lang][key];
    if (text) {
      element.placeholder = text;
    }
  });

  var titleElements = document.querySelectorAll('[data-i18n-title]');
  titleElements.forEach(function(element) {
    var key = element.getAttribute('data-i18n-title');
    var text = window.translations[lang] && window.translations[lang][key];
    if (text) {
      element.setAttribute('title', text.replace('{year}', year));
      element.setAttribute('aria-label', text.replace('{year}', year));
    }
  });

  var productNameElements = document.querySelectorAll('[data-i18n-product-name]');
  productNameElements.forEach(function(element) {
    var sourceValue = element.getAttribute('data-i18n-product-name');
    if (!sourceValue) {
      return;
    }

    if (lang === 'fr') {
      element.textContent = sourceValue;
      return;
    }

    var translated = window.productNameTranslations[lang] && window.productNameTranslations[lang][sourceValue];
    element.textContent = translated || sourceValue;
  });

  var categoryElements = document.querySelectorAll('[data-i18n-category]');
  categoryElements.forEach(function(element) {
    var sourceValue = element.getAttribute('data-i18n-category');
    if (!sourceValue) {
      return;
    }

    if (lang === 'fr') {
      element.textContent = sourceValue;
      return;
    }

    var translated = window.categoryTranslations[lang] && window.categoryTranslations[lang][sourceValue];
    element.textContent = translated || sourceValue;
  });

  var descriptionElements = document.querySelectorAll('[data-i18n-product-description]');
  descriptionElements.forEach(function(element) {
    var sourceValue = element.getAttribute('data-i18n-product-description');
    if (!sourceValue) {
      return;
    }

    if (lang === 'fr') {
      element.textContent = sourceValue;
      return;
    }

    var translated = window.productDescriptionTranslations[lang] && window.productDescriptionTranslations[lang][sourceValue];
    element.textContent = translated || sourceValue;
  });

  applyLocalizedSearch();
}

function normalizeText(value) {
  if (!value) {
    return '';
  }

  return value
    .toString()
    .toLowerCase()
    .normalize('NFD')
    .replace(/[\u0300-\u036f]/g, '');
}

function applyLocalizedSearch() {
  var form = document.querySelector('.search-form');
  if (!form) {
    return;
  }

  var input = form.querySelector('input[name="searchQuery"]');
  if (!input) {
    return;
  }

  var query = normalizeText(input.value.trim());
  var cards = document.querySelectorAll('.product-card');
  if (!cards.length) {
    return;
  }

  var visibleCount = 0;
  cards.forEach(function(card) {
    var searchableText = normalizeText(card.innerText);
    var isVisible = !query || searchableText.indexOf(query) !== -1;
    card.closest('.col').style.display = isVisible ? '' : 'none';
    if (isVisible) {
      visibleCount++;
    }
  });

  var noResults = document.getElementById('clientNoResults');
  if (noResults) {
    noResults.style.display = visibleCount === 0 ? '' : 'none';
  }
}

function initLocalizedSearch() {
  var form = document.querySelector('.search-form');
  if (!form) {
    return;
  }

  var input = form.querySelector('input[name="searchQuery"]');
  if (!input) {
    return;
  }

  form.addEventListener('submit', function(event) {
    event.preventDefault();
    applyLocalizedSearch();
  });

  input.addEventListener('input', applyLocalizedSearch);
}

// ========== CHANGE LANGUAGE FUNCTION ==========
window.changeLanguage = function(lang) {
  setLanguage(lang);
};

// ========== LANGUAGE DROPDOWN TOGGLE ==========
function initLanguageDropdown() {
  var languageDropdown = document.getElementById('languageDropdown');
  var languageMenu = document.getElementById('languageMenu');

  if (!languageDropdown || !languageMenu) {
    return;
  }

  // Toggle menu on button click
  languageDropdown.addEventListener('click', function(event) {
    event.stopPropagation();
    var isOpen = languageMenu.classList.contains('show');
    languageMenu.classList.toggle('show');
    languageDropdown.setAttribute('aria-expanded', !isOpen);
    languageMenu.setAttribute('aria-hidden', isOpen);
  });

  // Close menu when clicking on an option
  var languageItems = document.querySelectorAll('.language-option');
  languageItems.forEach(function(item) {
    item.addEventListener('click', function(event) {
      event.stopPropagation();
      var selectedLang = item.getAttribute('data-lang');
      if (selectedLang) {
        setLanguage(selectedLang);
      }
      languageMenu.classList.remove('show');
      languageDropdown.setAttribute('aria-expanded', false);
      languageMenu.setAttribute('aria-hidden', true);
    });
  });

  // Close menu when clicking outside
  document.addEventListener('click', function(event) {
    if (!languageDropdown.contains(event.target) && !languageMenu.contains(event.target)) {
      languageMenu.classList.remove('show');
      languageDropdown.setAttribute('aria-expanded', false);
      languageMenu.setAttribute('aria-hidden', true);
    }
  });

  // Close menu on Escape key
  document.addEventListener('keydown', function(event) {
    if (event.key === 'Escape') {
      languageMenu.classList.remove('show');
      languageDropdown.setAttribute('aria-expanded', false);
      languageMenu.setAttribute('aria-hidden', true);
    }
  });
}

// ========== INITIALIZE ON LOAD ==========
function initLanguage() {
  var currentLang = getCurrentLanguage();

  initLanguageDropdown();
  initLocalizedSearch();
  setLanguage(currentLang);
}

// Initialize when DOM is ready
if (document.readyState === 'loading') {
  document.addEventListener('DOMContentLoaded', initLanguage);
} else {
  initLanguage();
}

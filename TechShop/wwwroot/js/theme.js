// ========== THEME MANAGEMENT ==========
var THEME_KEY = 'theme-preference';

// Get current theme from localStorage or system preference
function getThemePreference() {
  var saved = localStorage.getItem(THEME_KEY);
  if (saved) {
    return saved;
  }
  // Check system preference
  return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
}

// Apply theme
function applyTheme(theme) {
  var root = document.documentElement;
  root.setAttribute('data-theme', theme);
  localStorage.setItem(THEME_KEY, theme);

  // Update icon
  var themeToggle = document.getElementById('themeToggle');
  if (themeToggle) {
    var icon = themeToggle.querySelector('i');
    if (icon) {
      icon.className = (theme === 'dark') ? 'fas fa-moon' : 'fas fa-sun';
    }
  }

  // Update meta theme-color
  var metaTheme = document.querySelector('meta[name="theme-color"]');
  if (metaTheme) {
    metaTheme.setAttribute('content', theme === 'dark' ? '#0f172a' : '#ffffff');
  }
}

// Toggle theme
window.toggleTheme = function() {
  var current = document.documentElement.getAttribute('data-theme') || 'light';
  var newTheme = (current === 'light') ? 'dark' : 'light';
  applyTheme(newTheme);
};

// Initialize theme on page load
function initTheme() {
  var theme = getThemePreference();
  applyTheme(theme);
}

// Initialize when DOM is ready
if (document.readyState === 'loading') {
  document.addEventListener('DOMContentLoaded', initTheme);
} else {
  initTheme();
}

// Listen for system theme changes
var mediaQuery = window.matchMedia('(prefers-color-scheme: dark)');
if (mediaQuery.addEventListener) {
  mediaQuery.addEventListener('change', function(e) {
    if (!localStorage.getItem(THEME_KEY)) {
      applyTheme(e.matches ? 'dark' : 'light');
    }
  });
} else if (mediaQuery.addListener) {
  mediaQuery.addListener(function(e) {
    if (!localStorage.getItem(THEME_KEY)) {
      applyTheme(e.matches ? 'dark' : 'light');
    }
  });
}

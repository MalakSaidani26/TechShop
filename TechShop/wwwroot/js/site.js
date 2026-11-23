(() => {
    const savedTheme = localStorage.getItem("theme") || "light";
    document.body.setAttribute("data-theme", savedTheme);

    const themeToggle = document.getElementById("themeToggle");
    if (themeToggle) {
        const updateIcon = () => {
            const isDark = document.body.getAttribute("data-theme") === "dark";
            themeToggle.innerHTML = isDark
                ? '<i class="fas fa-sun"></i>'
                : '<i class="fas fa-moon"></i>';
        };

        updateIcon();

        themeToggle.addEventListener("click", () => {
            const current = document.body.getAttribute("data-theme") || "light";
            const next = current === "dark" ? "light" : "dark";
            document.body.setAttribute("data-theme", next);
            localStorage.setItem("theme", next);
            updateIcon();
        });
    }
})();

window.setLanguage = (lang) => {
    const url = new URL(window.location.href);
    url.searchParams.set("lang", lang);
    window.location.href = url.toString();
};

(() => {
    const categoryButtons = document.querySelectorAll(".category-option-btn");
    if (categoryButtons.length > 0) {
        categoryButtons.forEach((btn) => {
            btn.addEventListener("click", () => {
                const value = btn.getAttribute("data-value") ?? "";
                const label = btn.getAttribute("data-label") ?? "";
                const input = document.getElementById("categoryIdInput");
                const selectedLabel = document.getElementById("categorySelectedLabel");

                if (input) {
                    input.value = value;
                }

                if (selectedLabel) {
                    selectedLabel.textContent = label;
                }

                const form = btn.closest("form");
                if (form) {
                    form.submit();
                }
            });
        });
    }

    const passwordToggles = document.querySelectorAll(".password-toggle");
    if (passwordToggles.length > 0) {
        passwordToggles.forEach((toggle) => {
            toggle.addEventListener("click", () => {
                const targetId = toggle.getAttribute("data-target");
                if (!targetId) {
                    return;
                }

                const input = document.getElementById(targetId);
                if (!input) {
                    return;
                }

                const icon = toggle.querySelector("i");
                const isPassword = input.getAttribute("type") === "password";
                input.setAttribute("type", isPassword ? "text" : "password");

                if (icon) {
                    icon.classList.toggle("fa-eye", !isPassword);
                    icon.classList.toggle("fa-eye-slash", isPassword);
                }

                const showText = toggle.getAttribute("data-show-text") ?? "Show";
                const hideText = toggle.getAttribute("data-hide-text") ?? "Hide";
                toggle.setAttribute("aria-label", isPassword ? hideText : showText);
            });
        });
    }
})();

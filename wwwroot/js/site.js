const checkbox = document.getElementById("themeToggle");
const body = document.body;

let isDark = localStorage.getItem("isDark") === "true";

function updateTheme() {
    isDark ? body.classList.add("dark") : body.classList.remove("dark");
    checkbox.checked = isDark;
}

updateTheme();

checkbox.addEventListener("change", () => {
    isDark = checkbox.checked;
    localStorage.setItem("isDark", isDark);
    updateTheme();
});

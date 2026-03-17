document.addEventListener("DOMContentLoaded", function () {
    const signupForm = document.querySelector("#signupForm");
    if (signupForm) {
        signupForm.addEventListener("submit", function (e) {
            const password = document.querySelector("#Password");
            const confirmPassword = document.querySelector("#ConfirmPassword");

            if (password && confirmPassword && password.value !== confirmPassword.value) {
                e.preventDefault();
                alert("Passwords do not match.");
            }
        });
    }

    const searchInput = document.querySelector("#productSearch");
    const productCards = document.querySelectorAll(".product-card");
    const noProductsMessage = document.querySelector("#noProductsMessage");
    const filterButtons = document.querySelectorAll(".btn-filter");

    let currentFilter = "all";

    function applyProductFilters() {
        const searchValue = searchInput ? searchInput.value.toLowerCase().trim() : "";
        let visibleCount = 0;

        productCards.forEach(card => {
            const text = card.innerText.toLowerCase();
            const category = card.dataset.category || "";
            const matchesSearch = text.includes(searchValue);
            const matchesCategory = currentFilter === "all" || category === currentFilter;

            const shouldShow = matchesSearch && matchesCategory;
            card.style.display = shouldShow ? "" : "none";

            if (shouldShow) {
                visibleCount++;
            }
        });

        if (noProductsMessage) {
            noProductsMessage.classList.toggle("d-none", visibleCount !== 0);
        }
    }

    if (searchInput) {
        searchInput.addEventListener("keyup", applyProductFilters);
    }

    if (filterButtons.length > 0) {
        filterButtons.forEach(button => {
            button.addEventListener("click", function () {
                filterButtons.forEach(btn => btn.classList.remove("active"));
                this.classList.add("active");
                currentFilter = this.dataset.filter;
                applyProductFilters();
            });
        });
    }

    const modal = document.querySelector("#productModal");
    const closeModalBtn = document.querySelector("#closeProductModal");
    const openButtons = document.querySelectorAll(".open-product-modal");

    const modalName = document.querySelector("#modalProductName");
    const modalDescription = document.querySelector("#modalProductDescription");
    const modalPrice = document.querySelector("#modalProductPrice");
    const modalImage = document.querySelector("#modalProductImage");

    openButtons.forEach(button => {
        button.addEventListener("click", function () {
            if (modalName) modalName.textContent = this.dataset.name;
            if (modalDescription) modalDescription.textContent = this.dataset.description;
            if (modalPrice) modalPrice.textContent = "$" + this.dataset.price;
            if (modalImage) {
                modalImage.src = this.dataset.image;
                modalImage.alt = this.dataset.name;
            }

            if (modal) {
                modal.classList.add("show");
            }
        });
    });

    if (closeModalBtn) {
        closeModalBtn.addEventListener("click", function () {
            if (modal) {
                modal.classList.remove("show");
            }
        });
    }

    if (modal) {
        modal.addEventListener("click", function (e) {
            if (e.target === modal) {
                modal.classList.remove("show");
            }
        });
    }
});
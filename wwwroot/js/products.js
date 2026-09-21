document.addEventListener("DOMContentLoaded", function () {

    const searchForm = document.getElementById("productSearchForm");

    if (searchForm) {
        searchForm.addEventListener("submit", function (event) {
            event.preventDefault();

            const params = new URLSearchParams(
                new FormData(searchForm)
            );

            params.set("page", 1);

            fetch("/Product/Index?" + params.toString(), {
                headers: {
                    "X-Requested-With": "XMLHttpRequest"
                }
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error(
                            "Search failed: " + response.status
                        );
                    }

                    return response.text();
                })
                .then(html => {
                    document.getElementById("productList").innerHTML = html;
                })
                .catch(error => {
                    console.error(error);
                });
        });
    }

    const resetButton =
        document.getElementById("resetProductFilters");

    if (resetButton) {
        resetButton.addEventListener("click", function () {

            searchForm.querySelector(
                'input[name="search"]'
            ).value = "";

            searchForm.querySelector(
                'select[name="sortOrder"]'
            ).value = "";

            searchForm.querySelector(
                'select[name="categoryId"]'
            ).value = "";

            searchForm.querySelector(
                'select[name="brandId"]'
            ).value = "";

            searchForm.querySelector(
                'select[name="supplierId"]'
            ).value = "";

            searchForm.querySelector(
                'input[name="minPrice"]'
            ).value = "";

            searchForm.querySelector(
                'input[name="maxPrice"]'
            ).value = "";

            searchForm.querySelector(
                'input[name="minQuantity"]'
            ).value = "";

            searchForm.querySelector(
                'input[name="maxQuantity"]'
            ).value = "";

            loadProductPage(1);
        });
    }

    const addProductButton =
        document.getElementById("addProductBtn");

    if (addProductButton) {
        addProductButton.addEventListener("click", function () {

            fetch("/Product/Create")
                .then(response => response.text())
                .then(html => {

                    document.getElementById("modal").innerHTML = html;

                    document.getElementById("productForm")
                        .addEventListener(
                            "submit",
                            createProduct
                        );
                });
        });
    }

});

function createProduct(event) {
    event.preventDefault();

    const form = document.getElementById("productForm");

    fetch("/Product/Create", {
        method: "POST",
        body: new FormData(form)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(
                    "Create failed: " + response.status
                );
            }

            return response.text();
        })
        .then(html => {
            document.getElementById(
                "productList"
            ).innerHTML = html;

            document.getElementById("modal").innerHTML = "";
        })
        .catch(error => {
            console.error(error);
        });
}

function showDetails(id) {
    fetch("/Product/Details?id=" + id)
        .then(response => {
            if (!response.ok) {
                throw new Error(
                    "Failed to load details: " + response.status
                );
            }

            return response.text();
        })
        .then(html => {
            document.getElementById("modal").innerHTML = html;
        })
        .catch(error => {
            console.error(error);
        });
}

function editProduct(id) {
    fetch("/Product/Edit?id=" + id)
        .then(response => {
            if (!response.ok) {
                throw new Error(
                    "Failed to load edit form: " +
                    response.status
                );
            }

            return response.text();
        })
        .then(html => {

            document.getElementById("modal").innerHTML = html;

            document.getElementById("productForm")
                .addEventListener(
                    "submit",
                    updateProduct
                );
        })
        .catch(error => {
            console.error(error);
        });
}

function updateProduct(event) {
    event.preventDefault();

    const form = document.getElementById("productForm");

    fetch("/Product/Edit", {
        method: "POST",
        body: new FormData(form)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(
                    "Update failed: " + response.status
                );
            }

            return response.text();
        })
        .then(html => {

            document.getElementById(
                "productList"
            ).innerHTML = html;

            document.getElementById("modal").innerHTML = "";
        })
        .catch(error => {
            console.error(error);
        });
}

function deleteProduct(id) {

    if (!confirm(
        "Are you sure you want to delete this product?"
    )) {
        return;
    }

    const token = document.querySelector(
        'input[name="__RequestVerificationToken"]'
    );

    if (!token) {
        console.error(
            "Anti-forgery token not found."
        );
        return;
    }

    fetch("/Product/Delete", {
        method: "POST",
        headers: {
            "Content-Type":
                "application/x-www-form-urlencoded"
        },
        body:
            "id=" +
            encodeURIComponent(id) +
            "&__RequestVerificationToken=" +
            encodeURIComponent(token.value)
    })
        .then(response => {

            console.log(
                "Delete status:",
                response.status
            );

            if (!response.ok) {
                throw new Error(
                    "Delete failed: " +
                    response.status
                );
            }

            return response.text();
        })
        .then(html => {
            document.getElementById(
                "productList"
            ).innerHTML = html;
        })
        .catch(error => {
            console.error(error);
            alert(
                "Delete failed. Check the browser console."
            );
        });
}

function closeModal() {
    const modal = document.getElementById("modal");

    if (modal) {
        modal.innerHTML = "";
    }
}

function deleteProductImage(id) {

    if (!confirm("Are you sure you want to delete this image?")) {
        return;
    }

    const token = document.querySelector(
        'input[name="__RequestVerificationToken"]'
    );

    if (!token) {
        console.error("Anti-forgery token not found.");
        return;
    }

    fetch("/Product/DeleteImage", {
        method: "POST",
        headers: {
            "Content-Type":
                "application/x-www-form-urlencoded"
        },
        body:
            "id=" +
            encodeURIComponent(id) +
            "&__RequestVerificationToken=" +
            encodeURIComponent(token.value)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(
                    "Image deletion failed: " +
                    response.status
                );
            }

            return response.json();
        })
        .then(result => {

            if (result.success) {
                showDetails(
                    document.querySelector(
                        ".product-details"
                    ).querySelector(
                        ".retro-panel-header p"
                    ).textContent
                        .replace("PRODUCT #", "")
                );
            }
        })
        .catch(error => {
            console.error(error);
        });
}

function loadProductPage(page) {

    const searchForm =
        document.getElementById("productSearchForm");

    if (!searchForm) {
        return;
    }

    const params = new URLSearchParams(
        new FormData(searchForm)
    );

    params.set("page", page);

    fetch("/Product/Index?" + params.toString(), {
        headers: {
            "X-Requested-With": "XMLHttpRequest"
        }
    })
        .then(response => {

            if (!response.ok) {
                throw new Error(
                    "Failed to load page: " +
                    response.status
                );
            }

            return response.text();
        })
        .then(html => {

            document.getElementById(
                "productList"
            ).innerHTML = html;
        })
        .catch(error => {
            console.error(error);
        });
}
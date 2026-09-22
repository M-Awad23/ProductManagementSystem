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
    const validationError = validateProductImages(form);

    if (validationError) {
        alert(validationError);
        return;
    }

    fetch("/Product/Create", {
        method: "POST",
        body: new FormData(form)
    })
        .then(async response => {
            const text = await response.text();

            if (!response.ok) {
                let message = "Unable to create the product.";

                try {
                    const error = JSON.parse(text);
                    message = error.message || message;
                } catch {
                    if (text) {
                        message = text;
                    }
                }

                console.error(
                    "Create failed:",
                    response.status,
                    text
                );

                throw new Error(message);
            }

            return text;
        })
        .then(html => {
            document.getElementById(
                "productList"
            ).innerHTML = html;

            document.getElementById("modal").innerHTML = "";
        })
        .catch(error => {
            console.error(error);
            alert(error.message);
        });
}


function validateProductImages(form) {

    const input = form.querySelector(
        'input[name="images"]'
    );

    if (!input || !input.files) {
        return null;
    }

    const allowedExtensions = [
        ".jpg",
        ".jpeg",
        ".png",
        ".gif",
        ".webp"
    ];

    const maxSize = 5 * 1024 * 1024;

    for (const file of input.files) {

        const extension = file.name
            .substring(
                file.name.lastIndexOf(".")
            )
            .toLowerCase();

        if (!allowedExtensions.includes(extension)) {
            return "Only JPG, JPEG, PNG, WEBP, and GIF images are allowed.";
        }

        if (file.size > maxSize) {
            return "Product images must be 5 MB or smaller.";
        }
    }

    return null;
}


function showDetails(id) {

    fetch("/Product/Details?id=" + id)
        .then(response => {

            if (!response.ok) {
                throw new Error(
                    "Failed to load details: " +
                    response.status
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
    const validationError = validateProductImages(form);

    if (validationError) {
        alert(validationError);
        return;
    }

    const formData = new FormData(form);

    const idInput = form.querySelector(
        'input[name="Id"]'
    );

    if (idInput) {
        formData.set(
            "id",
            idInput.value
        );
    }

    fetch(
        "/Product/Edit?id=" +
        encodeURIComponent(
            idInput ? idInput.value : ""
        ),
        {
            method: "POST",
            body: formData
        }
    )
        .then(async response => {

            const text = await response.text();

            if (!response.ok) {

                let message =
                    "Unable to update the product.";

                try {
                    const error = JSON.parse(text);
                    message =
                        error.message ||
                        message;
                }
                catch {
                    if (text) {
                        message = text;
                    }
                }

                throw new Error(message);
            }

            return text;
        })
        .then(html => {

            document.getElementById(
                "productList"
            ).innerHTML = html;

            document.getElementById(
                "modal"
            ).innerHTML = "";
        })
        .catch(error => {
            console.error(error);
            alert(error.message);
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

    const modal =
        document.getElementById("modal");

    if (modal) {
        modal.innerHTML = "";
    }
}


function setPrimaryImage(id) {

    const token = document.querySelector(
        'input[name="__RequestVerificationToken"]'
    );

    if (!token) {
        console.error(
            "Anti-forgery token not found."
        );
        return;
    }

    fetch("/Product/SetPrimaryImage", {
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
                    "Failed to set primary image: " +
                    response.status
                );
            }

            return response.json();
        })
        .then(result => {

            if (result.success) {

                const productDetails =
                    document.querySelector(
                        ".product-details"
                    );

                const productId =
                    productDetails
                        .querySelector(
                            ".retro-panel-header p"
                        )
                        .textContent
                        .replace(
                            "PRODUCT #",
                            ""
                        );

                showDetails(productId);
            }
        })
        .catch(error => {
            console.error(error);
        });
}


function replaceProductImage(id, input) {

    if (
        !input.files ||
        input.files.length === 0
    ) {
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

    const formData = new FormData();

    formData.append(
        "id",
        id
    );

    formData.append(
        "image",
        input.files[0]
    );

    formData.append(
        "__RequestVerificationToken",
        token.value
    );

    fetch("/Product/ReplaceImage", {
        method: "POST",
        body: formData
    })
        .then(async response => {

            const text = await response.text();

            if (!response.ok) {

                let message =
                    "Image replacement failed.";

                try {
                    const error =
                        JSON.parse(text);

                    message =
                        error.message ||
                        message;
                }
                catch {
                    if (text) {
                        message = text;
                    }
                }

                throw new Error(message);
            }

            return JSON.parse(text);
        })
        .then(result => {

            if (result.success) {

                const productId =
                    document
                        .querySelector(
                            ".product-details"
                        )
                        .querySelector(
                            ".retro-panel-header p"
                        )
                        .textContent
                        .replace(
                            "PRODUCT #",
                            ""
                        );

                showDetails(productId);
            }
        })
        .catch(error => {
            console.error(error);
            alert(error.message);
        });
}


function deleteProductImage(id) {

    if (!confirm(
        "Are you sure you want to delete this image?"
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
        .then(async response => {

            const text =
                await response.text();

            if (!response.ok) {

                let message =
                    "Image deletion failed.";

                try {
                    const error =
                        JSON.parse(text);

                    message =
                        error.message ||
                        message;
                }
                catch {
                    if (text) {
                        message = text;
                    }
                }

                throw new Error(message);
            }

            return JSON.parse(text);
        })
        .then(result => {

            if (result.success) {

                const productId =
                    document
                        .querySelector(
                            ".product-details"
                        )
                        .querySelector(
                            ".retro-panel-header p"
                        )
                        .textContent
                        .replace(
                            "PRODUCT #",
                            ""
                        );

                showDetails(productId);
            }
        })
        .catch(error => {

            console.error(error);
            alert(error.message);
        });
}


function loadProductPage(page) {

    const searchForm =
        document.getElementById(
            "productSearchForm"
        );

    if (!searchForm) {
        return;
    }

    const params =
        new URLSearchParams(
            new FormData(searchForm)
        );

    params.set(
        "page",
        page
    );

    fetch(
        "/Product/Index?" +
        params.toString(),
        {
            headers: {
                "X-Requested-With":
                    "XMLHttpRequest"
            }
        }
    )
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
document.addEventListener("DOMContentLoaded", function () {
    const resetButton = document.getElementById("resetProductFilters");

if (resetButton) {
    resetButton.addEventListener("click", function () {
        searchForm.reset();

        loadProductPage(1);
    });
}


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
                .then(response => response.text())
                .then(html => {
                    document.getElementById("productList").innerHTML = html;
                });
        });
    }

});




document.getElementById("addProductBtn").addEventListener("click", function () {

    fetch("/Product/Create")
        .then(response => response.text())
        .then(html => {
            document.getElementById("modal").innerHTML = html;

            document.getElementById("productForm")
                .addEventListener("submit", createProduct);
        });
});


function createProduct(event) {
    event.preventDefault();
    const form = document.getElementById("productForm");

    fetch("/Product/Create",
        {
            method: "POST",
            body: new FormData(form)
        }
    ).then(response => response.text()).then(html => {
        document.getElementById("productList").innerHTML = html;
        document.getElementById("modal").innerHTML = "";
    });

}



function showDetails(id) {

    fetch("/Product/Details?id=" + id)
        .then(response => response.text())
        .then(html => {

            document.getElementById("modal").innerHTML = html;
        });
}



function editProduct(id) {

    fetch("/Product/Edit?id=" + id)
        .then(response => response.text())
        .then(html => {

            document.getElementById("modal").innerHTML = html;

            document.getElementById("productForm")
                .addEventListener("submit", updateProduct);
        });
}




function updateProduct(event) {

    event.preventDefault();

    const form = document.getElementById("productForm");

    fetch("/Product/Edit", {
        method: "POST",
        body: new FormData(form)
    })
        .then(response => response.text())
        .then(html => {

            document.getElementById("productList").innerHTML = html;

            document.getElementById("modal").innerHTML = "";
        });
}




function deleteProduct(id) {

    if (!confirm("Are you sure you want to delete this product?")) {
        return;
    }

    const token = document.querySelector(
        'input[name="__RequestVerificationToken"]'
    ).value;

    fetch("/Product/Delete", {
        method: "POST",
        headers: {
            "Content-Type": "application/x-www-form-urlencoded"
        },
        body:
            "id=" + encodeURIComponent(id) +
            "&__RequestVerificationToken=" + encodeURIComponent(token)
    })
        .then(response => {

            console.log("Status:", response.status);

            if (!response.ok) {
                throw new Error("Delete failed: " + response.status);
            }

            return response.text();
        })
        .then(html => {
            document.getElementById("productList").innerHTML = html;
        })
        .catch(error => {
            console.error(error);
            alert("Delete failed. Check the browser console.");
        });
}

        function loadProductPage(page) {
    const searchForm = document.getElementById("productSearchForm");

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
                throw new Error("Failed to load page: " + response.status);
            }

            return response.text();
        })
        .then(html => {
            document.getElementById("productList").innerHTML = html;
        })
        .catch(error => {
            console.error(error);
        });
}
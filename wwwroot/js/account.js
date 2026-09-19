document.addEventListener("DOMContentLoaded", function () {

    const changeEmailButton =
        document.getElementById("changeEmailButton");

    if (changeEmailButton) {
        changeEmailButton.addEventListener("click", async function () {

            const response =
                await fetch("/Account/ChangeEmail");

            if (response.ok) {

                const html =
                    await response.text();

                document.getElementById(
                    "changeEmailContainer"
                ).innerHTML = html;

                setupChangeEmailForm();
            }
        });
    }


    const changePasswordButton =
        document.getElementById("changePasswordButton");

    if (changePasswordButton) {
        changePasswordButton.addEventListener("click", async function () {

            const response =
                await fetch("/Account/ChangePassword");

            if (response.ok) {

                const html =
                    await response.text();

                document.getElementById(
                    "changePasswordContainer"
                ).innerHTML = html;

                setupChangePasswordForm();
            }
        });
    }


    function setupChangeEmailForm() {

        const form =
            document.getElementById("changeEmailForm");

        if (!form) {
            return;
        }

        form.addEventListener("submit", async function (event) {

            event.preventDefault();

            const formData =
                new FormData(form);

            const response =
                await fetch("/Account/ChangeEmail", {
                    method: "POST",
                    body: formData
                });

            const result =
                await response.json();

            const message =
                document.getElementById(
                    "changeEmailMessage"
                );

            message.textContent = result.message;

            if (result.success) {

                document.querySelector(
                    "[data-user-email]"
                ).textContent = result.email;

            }
        });
    }


    function setupChangePasswordForm() {

        const form =
            document.getElementById("changePasswordForm");

        if (!form) {
            return;
        }

        form.addEventListener("submit", async function (event) {

            event.preventDefault();

            const formData =
                new FormData(form);

            const response =
                await fetch("/Account/ChangePassword", {
                    method: "POST",
                    body: formData
                });

            const result =
                await response.json();

            const message =
                document.getElementById(
                    "changePasswordMessage"
                );

            message.textContent = result.message;

            if (result.success) {
                form.reset();
            }
        });
    }

});
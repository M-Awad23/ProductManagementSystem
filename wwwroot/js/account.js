document.addEventListener("DOMContentLoaded", function () {
    const profilePhotoForm =
    document.getElementById("profilePhotoForm");

if (profilePhotoForm) {
    profilePhotoForm.addEventListener("submit", async function (event) {
        event.preventDefault();

        const formData = new FormData(profilePhotoForm);

        const response = await fetch(
            "/Account/UploadProfilePhoto",
            {
                method: "POST",
                body: formData
            }
        );

        const result = await response.json();

        document.getElementById(
            "profilePhotoMessage"
        ).textContent = result.message || "";

        if (result.success) {
            let image = document.querySelector(
                "[data-profile-photo]"
            );

            if (!image) {
                image = document.createElement("img");
                image.setAttribute("data-profile-photo", "");
                image.alt = "Profile Photo";
                image.style.width = "150px";
                image.style.height = "150px";
                image.style.objectFit = "cover";

                profilePhotoForm.before(image);
            }

            image.src = result.photoUrl;
            profilePhotoForm.reset();
        }
    });
}
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
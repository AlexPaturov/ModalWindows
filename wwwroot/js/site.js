// wwwroot/js/site.js

// Ждем, пока вся страница загрузится
document.addEventListener("DOMContentLoaded", function () {

    // Ищем на странице скрытые поля, которые наш контроллер будет заполнять
    const successMessage = document.getElementById("modalSuccessMessage")?.value;
    const errorMessage = document.getElementById("modalErrorMessage")?.value;
    const warningMessage = document.getElementById("modalWarningMessage")?.value;

    if (successMessage) {
        // Находим элементы модального окна и заполняем их
        const successModal = new bootstrap.Modal(document.getElementById('successModal'));
        document.getElementById('successModalMessage').innerText = successMessage;
        successModal.show();
    }

    if (errorMessage) {
        const errorModal = new bootstrap.Modal(document.getElementById('errorModal'));
        document.getElementById('errorModalMessage').innerText = errorMessage;
        errorModal.show();
    }

    if (warningMessage) {
        const warningModal = new bootstrap.Modal(document.getElementById('warningModal'));
        document.getElementById('warningModalMessage').innerText = warningMessage;
        warningModal.show();
    }
});
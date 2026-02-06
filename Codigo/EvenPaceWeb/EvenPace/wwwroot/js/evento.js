document.addEventListener("DOMContentLoaded", function () {

    // 1. Lógica do Alerta (Toast)
    var alerta = document.querySelector(".alert-custom");
    if (alerta) {
        setTimeout(function () {
            alerta.classList.add("alert-hide");
            setTimeout(function () {
                alerta.remove();
            }, 500);
        }, 3000);
    }

    // 2. Upload de Imagem (Display do nome)
    var fileInput = document.getElementById("ImagemUpload"); // Certifique-se que o ID no HTML é este ou use a classe
    var fileInputClass = document.querySelector(".file-hidden");
    var fileNameDisplay = document.getElementById("fileNameDisplay");

    if (fileInputClass) {
        fileInputClass.addEventListener("change", function () {
            if (this.files && this.files[0]) {
                fileNameDisplay.value = this.files[0].name;
            }
        });
    }
});

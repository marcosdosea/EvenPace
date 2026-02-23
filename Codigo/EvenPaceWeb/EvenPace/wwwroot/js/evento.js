document.addEventListener("DOMContentLoaded", function () {

    var alerta = document.querySelector(".alert-custom");
    if (alerta) {
        setTimeout(function () {
            alerta.classList.add("alert-hide");
            setTimeout(function () {
                alerta.remove();
            }, 500);
        }, 3000);
    }

    var fileInput = document.getElementById("ImagemUpload"); 
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

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

    // 2. Upload de Imagem do Kit
    var fileInputClass = document.querySelector(".file-hidden");
    var textInput = document.querySelector(".input-wrapper input[type='text']");

    if (fileInputClass && textInput) {
        fileInputClass.addEventListener("change", function (e) {
            if (e.target.files && e.target.files[0]) {
                textInput.value = e.target.files[0].name;
            }
        });
    }

    // 3. Limpeza de nome ao editar (Remove o ID Guid se existir)
    var hiddenInput = document.getElementById("Imagem");
    if (hiddenInput && textInput && hiddenInput.value) {
        var nomeCompleto = hiddenInput.value;
        if (nomeCompleto.includes('_')) {
            textInput.value = nomeCompleto.substring(nomeCompleto.indexOf('_') + 1);
        } else {
            textInput.value = nomeCompleto;
        }
    }
});
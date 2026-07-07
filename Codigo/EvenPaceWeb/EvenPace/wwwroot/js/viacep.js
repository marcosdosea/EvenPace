document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll("[data-viacep-form]").forEach(function (form) {
        const cepInput = form.querySelector("[data-viacep-cep]");
        const ruaInput = form.querySelector("[data-viacep-rua]");
        const bairroInput = form.querySelector("[data-viacep-bairro]");
        const cidadeInput = form.querySelector("[data-viacep-cidade]");
        const estadoInput = form.querySelector("[data-viacep-estado]");

        if (!cepInput || !ruaInput || !bairroInput || !cidadeInput || !estadoInput) {
            return;
        }

        const limparCep = function (cep) {
            return cep.replace(/\D/g, "");
        };

        const aplicarMascaraCep = function () {
            cepInput.setCustomValidity("");
            const cep = limparCep(cepInput.value).slice(0, 8);
            cepInput.value = cep.length > 5 ? `${cep.slice(0, 5)}-${cep.slice(5)}` : cep;
        };

        const preencherEndereco = async function () {
            const cep = limparCep(cepInput.value);

            if (cep.length !== 8) {
                return;
            }

            cidadeInput.value = "";
            estadoInput.value = "";

            try {
                const response = await fetch(`https://viacep.com.br/ws/${cep}/json/`);
                const endereco = await response.json();

                if (!response.ok || endereco.erro) {
                    cepInput.setCustomValidity("CEP não encontrado.");
                    cepInput.reportValidity();
                    return;
                }

                cepInput.setCustomValidity("");
                ruaInput.value = endereco.logradouro || ruaInput.value;
                bairroInput.value = endereco.bairro || bairroInput.value;
                cidadeInput.value = endereco.localidade || "";
                estadoInput.value = endereco.uf || "";
            } catch {
                cepInput.setCustomValidity("Não foi possível consultar o CEP.");
                cepInput.reportValidity();
            }
        };

        cepInput.addEventListener("input", aplicarMascaraCep);
        cepInput.addEventListener("blur", preencherEndereco);
    });
});

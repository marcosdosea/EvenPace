function selecionarDistancia(botao, valor) {
    document.getElementById("distanciaSelecionada").value = valor;

    document.querySelectorAll(".btn-distance")
        .forEach(b => b.classList.remove("active"));

    botao.classList.add("active");
}

function selecionarKit(botao, id) {
    document.getElementById("kitSelecionado").value = id;

    document.querySelectorAll(".btn-kit")
        .forEach(b => b.classList.remove("selected"));

    botao.classList.add("selected");
}

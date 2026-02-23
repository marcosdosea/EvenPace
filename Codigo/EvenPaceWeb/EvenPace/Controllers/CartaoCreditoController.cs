using AutoMapper;
using Core.Service;
using Models;
using Microsoft.AspNetCore.Mvc;

namespace EvenPace.Controllers;

public class CartaoCreditoControler : Controller
{
    private ICartaoCreditoService _cartaoCredito;
    private IMapper _mapper;

    public CartaoCreditoControler(ICartaoCreditoService cartaoCredito, IMapper mapper)
    {
        _cartaoCredito = cartaoCredito;
        _mapper = mapper;
    }

    /// <summary>
    /// Fornece o resumo e listagem gerencial pertinente a todos os métodos de transação vinculados a cartões armazenados para processamento de faturas de inscrição.
    /// </summary>
    /// <returns>Catálogo relacional iterativo dos cartões ativos no sistema.</returns>
    public ActionResult Index()
    {
        var cartaoCredito = _cartaoCredito.GetAll();
        var cartaoCreditoViewModels = _mapper.Map<List<CartaoCreditoViewModel>>(cartaoCredito);
        return View(cartaoCreditoViewModels);
    }

    /// <summary>
    /// Executa a exibição concentrada e sigilosa de um instrumento de cobrança estipulado pela correspondente ID submetida.
    /// </summary>
    /// <param name="id">Chave atrelativa ao cartão no banco de validação de pagamentos.</param>
    /// <returns>A janela particular exibindo as partes descritivas requeridas contidas.</returns>
    public ActionResult Details(int id)
    {
        var cartaoCredito = _cartaoCredito.Get((int)id);
        var cartaoCreditoViewModel = _mapper.Map<CartaoCreditoViewModel>(cartaoCredito);
        return View(cartaoCreditoViewModel);
    }

    /// <summary>
    /// Elabora a página receptora focada no arquivamento seguro dos números e componentes operantes essenciais à aceitação de um método financeiro inovador do cliente no momento da aprovação.
    /// </summary>
    /// <returns>Modelo visual em branco propício ao cadastro contendo inputs requeridos do cartão.</returns>
    public ActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// Efetiva as inclusões procedimentais dos registros captados no preenchimento convertendo a visualização temporal em dado de pagamento atrelado e persistente no banco ativo de transações de corredor ou gestor.
    /// </summary>
    /// <param name="cartaoCreditoViewModel">Classe agrupando as métricas e chaves providenciadas pelo comprador.</param>
    /// <returns>Promove o regresso instantâneo da visualização para os índices consolidados de cartões cadastrados.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(CartaoCreditoViewModel cartaoCreditoViewModel)
    {
        if (ModelState.IsValid)
        {
            var cartaoCredito = _mapper.Map<Core.CartaoCredito>(cartaoCreditoViewModel);
            _cartaoCredito.Create(cartaoCredito);
        }
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Dispõe e invoca a tela formatada resgatando os blocos de informações gravados anteriormente voltados a aceitar reparos, aditivos limitadores ou troca de validade para viabilizar as transações contínuas de um cartão.
    /// </summary>
    /// <param name="id">Identificador base fornecido atrelado à ferramenta de cobrança requerida.</param>
    /// <returns>Ambiente gráfico dotado de preenchimento antecedente para edição assertiva.</returns>
    public ActionResult Edit(int id)
    {
        var cartaoCredito = _cartaoCredito.Get((int)id);
        var cartaoCreditoViewModel = _mapper.Map<CartaoCreditoViewModel>(cartaoCredito);
        return View(cartaoCreditoViewModel);
    }

    /// <summary>
    /// Procede com a alocação e gravação integral das informações consertadas ou substituídas provenientes de formulário não-estrito, efetivando novas regras a um cartão específico atuante sem destruir relações anteriores.
    /// </summary>
    /// <param name="id">Chave referencial designada temporalmente com o propósito atrelativo no ato da identificação processual.</param>
    /// <param name="collection">Pacote abstrato de dados de coleção HTTP formulário aglomerando propriedades atreladas enviadas na edição.</param>
    /// <returns>Sinaliza sucesso reescrevendo a janela central principal com retorno automático.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
    {
        if (ModelState.IsValid)
        {
            var cartaoCredito = _mapper.Map<Core.CartaoCredito>(collection);
            _cartaoCredito.Edit(cartaoCredito);
        }
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Constrói o obstáculo protetivo em interface exibindo métricas do cartão com o intuito focado em questionar o autor sobre as consequências envolvidas numa eminente exclusão das chaves em repouso.
    /// </summary>
    /// <param name="id">Código identificador exclusivo do recurso transacional focado.</param>
    /// <returns>Disponibiliza vista descritiva requerendo aval confirmatório in-loco.</returns>
    public ActionResult Delete(int id)
    {
        var cartaoCredito = _cartaoCredito.Get((int)id);
        var cartaoCreditoViewModel = _mapper.Map<CartaoCreditoViewModel>(cartaoCredito);
        return View(cartaoCreditoViewModel);
    }

    /// <summary>
    /// Aborta e expulsa de maneira incontornável e decisiva as relações preexistentes referentes a esta metodologia de pagamento alocada nos arquivos centrais das transações correntes da aplicação relacional via ORM em vigor.
    /// </summary>
    /// <param name="id">A indexação que valida de ponta a ponta as propriedades exclusivas do cartão no sistema.</param>
    /// <param name="cartaoCreditoViewModel">Instrumento referenciador gerado pelas submissões do formulário no ambiente view.</param>
    /// <returns>Realocações automáticas com trânsito repassado ao quadro resumo da classe manipuladora limpa.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, CartaoCreditoViewModel cartaoCreditoViewModel)
    {
        _cartaoCredito.Delete((int)id);
        return RedirectToAction(nameof(Index));
    }
}
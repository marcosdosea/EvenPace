using AutoMapper;
using Core.Service;
using Models;
using Microsoft.AspNetCore.Mvc;

namespace EvenPace.Controllers;

public class CartaoCreditoController : Controller
{
    private ICartaoCreditoService _cartaoCredito;
    private IInscricaoService _inscricaoService;
    private IMapper _mapper;

    public CartaoCreditoController(ICartaoCreditoService cartaoCredito, IInscricaoService inscricaoService,IMapper mapper)
    {
        _cartaoCredito = cartaoCredito;
        _inscricaoService = inscricaoService;
        _mapper = mapper;
    }

    /// <summary>
    /// Fornece o resumo e listagem gerencial pertinente a todos os m�todos de transa��o vinculados a cart�es armazenados para processamento de faturas de inscri��o.
    /// </summary>
    /// <returns>Cat�logo relacional iterativo dos cart�es ativos no sistema.</returns>
    public ActionResult Index()
    {
        var idCorredorClaim = User.FindFirst("IdCorredor");

        if (idCorredorClaim == null)
            return RedirectToAction("Login", "Account");

        int idCorredor = int.Parse(idCorredorClaim.Value);

        var cartoes = _cartaoCredito.GetByCorredor(idCorredor);
        var viewModels = _mapper.Map<List<CartaoCreditoViewModel>>(cartoes);

        return View(viewModels);
    }

    /// <summary>
    /// Executa a exibi��o concentrada e sigilosa de um instrumento de cobran�a estipulado pela correspondente ID submetida.
    /// </summary>
    /// <param name="id">Chave atrelativa ao cart�o no banco de valida��o de pagamentos.</param>
    /// <returns>A janela particular exibindo as partes descritivas requeridas contidas.</returns>
    public ActionResult Details(int id)
    {
        var cartaoCredito = _cartaoCredito.Get((int)id);
        var cartaoCreditoViewModel = _mapper.Map<CartaoCreditoViewModel>(cartaoCredito);
        return View(cartaoCreditoViewModel);
    }

    /// <summary>
    /// Elabora a p�gina receptora focada no arquivamento seguro dos n�meros e componentes operantes essenciais � aceita��o de um m�todo financeiro inovador do cliente no momento da aprova��o.
    /// </summary>
    /// <returns>Modelo visual em branco prop�cio ao cadastro contendo inputs requeridos do cart�o.</returns>
    public ActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// Efetiva as inclus�es procedimentais dos registros captados no preenchimento convertendo a visualiza��o temporal em dado de pagamento atrelado e persistente no banco ativo de transa��es de corredor ou gestor.
    /// </summary>
    /// <param name="cartaoCreditoViewModel">Classe agrupando as m�tricas e chaves providenciadas pelo comprador.</param>
    /// <returns>Promove o regresso instant�neo da visualiza��o para os �ndices consolidados de cart�es cadastrados.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(CartaoCreditoViewModel cartaoCreditoViewModel)
    {
        var idCorredorClaim = User.FindFirst("IdCorredor");
        
        if (!ModelState.IsValid)
            return View(cartaoCreditoViewModel);

        var cartaoCredito = _mapper.Map<Core.CartaoCredito>(cartaoCreditoViewModel);
        cartaoCredito.IdCorredor = int.Parse(idCorredorClaim.Value);
        _cartaoCredito.Create(cartaoCredito);

        return RedirectToAction("Index", "Home");
    }

    /// <summary>
    /// Disp�e e invoca a tela formatada resgatando os blocos de informa��es gravados anteriormente voltados a aceitar reparos, aditivos limitadores ou troca de validade para viabilizar as transa��es cont�nuas de um cart�o.
    /// </summary>
    /// <param name="id">Identificador base fornecido atrelado � ferramenta de cobran�a requerida.</param>
    /// <returns>Ambiente gr�fico dotado de preenchimento antecedente para edi��o assertiva.</returns>
    public ActionResult Edit(int id)
    {
        var cartaoCredito = _cartaoCredito.Get((int)id);
        var cartaoCreditoViewModel = _mapper.Map<CartaoCreditoViewModel>(cartaoCredito);
        return View(cartaoCreditoViewModel);
    }

    /// <summary>
    /// Procede com a aloca��o e grava��o integral das informa��es consertadas ou substitu�das provenientes de formul�rio n�o-estrito, efetivando novas regras a um cart�o espec�fico atuante sem destruir rela��es anteriores.
    /// </summary>
    /// <param name="id">Chave referencial designada temporalmente com o prop�sito atrelativo no ato da identifica��o processual.</param>
    /// <param name="collection">Pacote abstrato de dados de cole��o HTTP formul�rio aglomerando propriedades atreladas enviadas na edi��o.</param>
    /// <returns>Sinaliza sucesso reescrevendo a janela central principal com retorno autom�tico.</returns>
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
    /// Constr�i o obst�culo protetivo em interface exibindo m�tricas do cart�o com o intuito focado em questionar o autor sobre as consequ�ncias envolvidas numa eminente exclus�o das chaves em repouso.
    /// </summary>
    /// <param name="id">C�digo identificador exclusivo do recurso transacional focado.</param>
    /// <returns>Disponibiliza vista descritiva requerendo aval confirmat�rio in-loco.</returns>
    public ActionResult Delete(int id)
    {
        var cartaoCredito = _cartaoCredito.Get((int)id);
        var cartaoCreditoViewModel = _mapper.Map<CartaoCreditoViewModel>(cartaoCredito);
        return View(cartaoCreditoViewModel);
    }

    /// <summary>
    /// Aborta e expulsa de maneira incontorn�vel e decisiva as rela��es preexistentes referentes a esta metodologia de pagamento alocada nos arquivos centrais das transa��es correntes da aplica��o relacional via ORM em vigor.
    /// </summary>
    /// <param name="id">A indexa��o que valida de ponta a ponta as propriedades exclusivas do cart�o no sistema.</param>
    /// <param name="cartaoCreditoViewModel">Instrumento referenciador gerado pelas submiss�es do formul�rio no ambiente view.</param>
    /// <returns>Realoca��es autom�ticas com tr�nsito repassado ao quadro resumo da classe manipuladora limpa.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, CartaoCreditoViewModel cartaoCreditoViewModel)
    {
        _cartaoCredito.Delete((int)id);
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult RealizarPagamento(int IdCartao, int IdInscricao)
    {
        var inscricao = _inscricaoService.Get(IdInscricao);

        inscricao.Status = "Confirmada";
        _inscricaoService.Edit(inscricao);

        return RedirectToAction("Details", "Inscricao", new { id = IdInscricao });
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult CancelarPagamento(int IdInscricao)
    {
        _inscricaoService.Delete(IdInscricao);
        return RedirectToAction("Index", "Evento");
    }
}
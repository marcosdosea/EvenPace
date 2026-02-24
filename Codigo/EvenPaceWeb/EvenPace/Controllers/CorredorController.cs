using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Threading.Tasks;

namespace EvenPace.Controllers;

public class CorredorController : Controller
{
    private readonly ICorredorService _corredorService;
    private readonly IMapper _mapper;
    private readonly IAuthService _authService;

    public CorredorController(
        ICorredorService corredor,
        IMapper mapper,
        IAuthService authService)
    {
        _corredorService = corredor;
        _mapper = mapper;
        _authService = authService;
    }

    /// <summary>
    /// Fornece a interface visual para a autenticação do perfil do corredor.
    /// </summary>
    /// <returns>View contendo o formulário de login.</returns>
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    /// <summary>
    /// Valida as credenciais inseridas pelo corredor e delega a autenticação ao serviço responsável.
    /// </summary>
    /// <param name="email">Endereço de e-mail do corredor.</param>
    /// <param name="senha">Senha vinculada ao e-mail.</param>
    /// <returns>Redireciona para o painel de eventos caso aprovado, ou retorna a View com erros de validação.</returns>
    [HttpPost]
    public async Task<IActionResult> Login(string email, string senha)
    {
        var isAutenticado = await _authService.LoginAsync(email, senha);

        if (isAutenticado)
            return RedirectToAction("IndexUsuario", "Evento");

        ModelState.AddModelError("", "Login inválido");
        return View();
    }

    /// <summary>
    /// Apresenta o perfil de um corredor específico com base na sua chave de identificação.
    /// </summary>
    /// <param name="id">Chave primária do corredor solicitada.</param>
    /// <returns>View associada aos dados mapeados do corredor.</returns>
    public ActionResult Get(int id)
    {
        Corredor corredor = _corredorService.Get(id);
        CorredorViewModel corredorModel = _mapper.Map<CorredorViewModel>(corredor);
        return View(corredorModel);
    }

    /// <summary>
    /// Disponibiliza o formulário em branco destinado ao registo de um novo participante/corredor.
    /// </summary>
    /// <returns>View de cadastro.</returns>
    public ActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// Efetiva a gravação das informações de um novo corredor provenientes do formulário de submissão.
    /// </summary>
    /// <param name="corredorModel">O objeto model preenchido que será convertido e salvo na base de dados.</param>
    /// <returns>A mesma View de cadastro para continuidade ou redirecionamento de acordo com o fluxo do sistema.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(CorredorViewModel corredorModel)
    {
        if (ModelState.IsValid)
        {
            var corredor = _mapper.Map<Corredor>(corredorModel);
            _corredorService.Create(corredor);
        }
        return View(corredorModel);
    }

    /// <summary>
    /// Carrega as informações atuais de um corredor e as expõe para alteração do utilizador através de um formulário.
    /// </summary>
    /// <param name="id">Identificador do corredor a ser editado.</param>
    /// <returns>View preenchida com o ViewModel correspondente ao perfil ou um modelo vazio se não encontrado.</returns>
    public ActionResult Edit(int id)
    {
        Corredor corredor = _corredorService.Get(id);
        if (corredor == null)
        {
            return View(new CorredorViewModel());
        }
        CorredorViewModel corredorModel = _mapper.Map<CorredorViewModel>(corredor);
        return View(corredorModel);
    }

    /// <summary>
    /// Aplica e guarda na base de dados as eventuais mudanças feitas aos dados do corredor na interface visual.
    /// </summary>
    /// <param name="corredorModel">Model de visualização já modificado pelo utilizador na interface.</param>
    /// <returns>Redireciona para a visualização dos dados atualizados em caso de êxito.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(CorredorViewModel corredorModel)
    {
        if (ModelState.IsValid)
        {
            var corredor = _mapper.Map<Corredor>(corredorModel);
            _corredorService.Edit(corredor);
            return RedirectToAction(nameof(Get), new { id = corredorModel.Id });
        }

        return View(corredorModel);
    }

    /// <summary>
    /// Fornece a página de confirmação de segurança antes de submeter o registo de um corredor à exclusão definitiva.
    /// </summary>
    /// <param name="id">Identificador da entidade alvo de exclusão.</param>
    /// <returns>View contendo o resumo dos dados que serão removidos.</returns>
    public ActionResult Delete(int id)
    {
        Corredor corredor = _corredorService.Get(id);
        CorredorViewModel corredorModel = _mapper.Map<CorredorViewModel>(corredor);
        return View(corredorModel);
    }

    /// <summary>
    /// Processa e conclui a remoção dos dados de um corredor da base de dados do sistema.
    /// </summary>
    /// <param name="id">A chave de identificação do corredor a ser apagado.</param>
    /// <param name="corredorModel">Modelo representando a entidade na interface (usado para validações do formulário).</param>
    /// <returns>Redireciona a navegação de volta ao ponto de entrada primário ou ao índice da listagem.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, CorredorViewModel corredorModel)
    {
        _corredorService.Delete(id);
        return RedirectToAction(nameof(Index));
    }
}
using AutoMapper;
using Core;
using Core.Service;
using EvenPaceWeb.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace EvenPace.Controllers;

public class CorredorController : Controller
{
    private ICorredorService _corredorService;
    private IMapper _mapper;
    private readonly UserManager<UsuarioIdentity>  _userManager;
    private readonly SignInManager<UsuarioIdentity> _signInManager;

    public CorredorController(
        ICorredorService corredor,
        IMapper mapper,
        UserManager<UsuarioIdentity> userManager,
        SignInManager<UsuarioIdentity> signInManager)
    {
        _corredorService = corredor;
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
    }
    /// <summary>
    /// Fornece a interface visual para autenticação do perfil do corredor.
    /// </summary>
    /// <returns>View contendo o formulário de login.</returns>
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    /// <summary>
    /// Valida as credenciais inseridas pelo corredor e executa a autenticação no sistema via Identity.
    /// </summary>
    /// <param name="email">Endereço de e-mail do corredor.</param>
    /// <param name="senha">Senha secreta vinculada ao e-mail.</param>
    /// <returns>Redireciona para o painel de eventos caso aprovado, ou retorna a View com alertas de erro de validação.</returns>
    [HttpPost]
    public async Task<IActionResult> Login(string email, string senha)
    {
        var result = await _signInManager.PasswordSignInAsync(
            email,
            senha,
            false,
            false
        );

        if (result.Succeeded)
            return RedirectToAction("IndexUsuario", "Evento");

        ModelState.AddModelError("", "Login inválido");
        return View();
    }
    /// <summary>
    /// Busca e exibe o perfil do corredor correspondente ao usuário que está ativamente autenticado na sessão.
    /// </summary>
    /// <returns>View contendo os detalhes cadastrais do corredor, ou redirecionamento em caso de ausência de sessão/cadastro.</returns>
    public async Task<IActionResult> GetByEmail()
    {
        var usuario = await _userManager.GetUserAsync(User);

        if (usuario == null)
            return RedirectToAction("Login");

        var corredor = _corredorService.GetByEmail(usuario.Email);

        if (corredor == null)
            return RedirectToAction("Create"); // ou alguma tela de completar cadastro

        var model = _mapper.Map<CorredorViewModel>(corredor);

        return View("Get", model);
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
    /// Disponibiliza o formulário em branco destinado ao registro de um novo participante/corredor.
    /// </summary>
    /// <returns>View de cadastro.</returns>
    public ActionResult Create()
    {
        return View();
    }
    /// <summary>
    /// Efetiva a gravação das informações de um novo corredor proveniente do formulário de submissão.
    /// </summary>
    /// <param name="corredorModel">O objeto model preenchido que será convertido e salvo.</param>
    /// <returns>A mesma View de cadastro para continuidade ou redirecionamento dependendo da regra negocial acoplada.</returns>
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
    /// Carrega as informações atuais de um corredor e as expõe para alteração do usuário em formulário.
    /// </summary>
    /// <param name="id">Identificador do corredor a ser editado.</param>
    /// <returns>View preenchida com o ViewModel correspondente ao perfil.</returns>
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
    /// Aplica e salva em banco as eventuais mudanças feitas aos dados do corredor.
    /// </summary>
    /// <param name="corredorModel">Model de visualização já modificado pelo usuário na interface.</param>
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
    /// Fornece a página de confirmação antes de submeter o registro de um corredor à exclusão.
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
    /// Processa e conclui a remoção dos dados de um corredor do banco de dados do sistema.
    /// </summary>
    /// <param name="id">A chave de identificação do corredor a ser apagado.</param>
    /// <param name="corredorModel">Modelo representando a entidade na interface (não utilizado na lógica de exclusão direta).</param>
    /// <returns>Redireciona a navegação de volta ao ponto de entrada primário ou índice.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, CorredorViewModel corredorModel)
    {
        _corredorService.Delete(id);
        return RedirectToAction(nameof(Index));
    }
}
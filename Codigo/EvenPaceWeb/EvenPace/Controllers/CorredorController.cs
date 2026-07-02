using System.Security.Claims;
using AutoMapper;
using Core;
using Core.Service;
using EvenPaceWeb.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
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
    
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string cpf, string senha)
    {
        cpf = cpf.Replace(".", "").Replace("-", "");
        
        var result = await _signInManager.PasswordSignInAsync(
            cpf,
            senha,
            false,
            false
        );

        if (result.Succeeded)
            return RedirectToAction("IndexUsuario", "Evento");
        
        ModelState.AddModelError("", "CPF ou Senha inálidos");
        return View();
    }

    public ActionResult Get(int id)
    {
        Corredor corredor = _corredorService.Get(id);
        CorredorViewModel corredorModel = _mapper.Map<CorredorViewModel>(corredor);
        return View(corredorModel);
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CorredorViewModel corredorModel)
    {
        if (!ModelState.IsValid)
            return View(corredorModel);

        var corredor = _mapper.Map<Corredor>(corredorModel);

        // Salva no banco do sistema
        _corredorService.Create(corredor);

        // Cria o usuário do Identity
        var identityUser = new UsuarioIdentity
        {
            UserName = corredor.Cpf.Replace(".", "").Replace("-", ""),
            Email = corredor.Email
        };

        var result = await _userManager.CreateAsync(identityUser, corredorModel.Senha);

        if (!result.Succeeded)
        {
            // Remove o corredor caso o Identity falhe
            _corredorService.Delete(corredor.Id);

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(corredorModel);
        }

        return RedirectToAction("Login", "Corredor");
    }

    [HttpGet]
    public IActionResult DefinirAcesso(string cpf)
    {
        if (string.IsNullOrEmpty(cpf)) return RedirectToAction("Create");

        // Passamos o CPF para a View para sabermos qual usuário estamos finalizando
        ViewBag.Cpf = cpf;
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    
    public async Task<IActionResult> FinalizarCadastro(string cpf, string email, string senha)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
        {
            ModelState.AddModelError("", "E-mail e Senha são obrigatórios.");
            ViewBag.Cpf = cpf;
            return View("DefinirAcesso");
        }

        // Procura o corredor cadastrado anteriormente
        var corredor = _corredorService.GetByCpf(cpf);

        if (corredor == null)
        {
            ModelState.AddModelError("", "Corredor não encontrado.");
            ViewBag.Cpf = cpf;
            return View("DefinirAcesso");
        }

        // Atualiza os dados do corredor
        corredor.Email = email;
        corredor.Senha = senha; // Se estiver usando Identity, o ideal é remover esta linha futuramente

        // Cria o usuário no Identity
        var identityUser = new UsuarioIdentity
        {
            UserName = cpf.Replace(".", "").Replace("-", ""),
            Email = email
        };

        var result = await _userManager.CreateAsync(identityUser, senha);

        if (result.Succeeded)
        {
            // Salva Email e Senha na tabela Corredor
            _corredorService.Edit(corredor);

            // Redireciona para o login
            return RedirectToAction("Login", "Corredor");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        ViewBag.Cpf = cpf;
        return View("DefinirAcesso");
    }
    /*[HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CorredorViewModel corredorModel)
    {
        // Remova a validação estrita se estiver enviando apenas parte dos dados
        // Ou certifique-se de que Nome, CPF e Data de Nascimento são os únicos obrigatórios no ViewModel

        if (ModelState.IsValid)
        {
            // 1. Mapeia para a entidade de banco de dados
            var corredor = _mapper.Map<Corredor>(corredorModel);

            // 2. Tenta criar no banco de dados de negócio (EvenPaceContext)
            try
            {
                _corredorService.Create(corredor);

                // IMPORTANTE: Para o Identity funcionar, você precisaria criar o UsuarioIdentity aqui também
                // com o CPF como UserName e uma senha padrão ou vinda do formulário.

                return RedirectToAction("Login"); // Redireciona para não ficar na mesma página
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao salvar no banco: " + ex.Message);
            }
        }
        return View(corredorModel);
    }*/
    /*[HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CorredorViewModel corredorModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var corredor = _mapper.Map<Corredor>(corredorModel);
                _corredorService.Create(corredor);
                return RedirectToAction("ProximaEtapa"); // Ou onde for o fluxo
            }
            catch (Exception ex)
            {
                // Isso vai mostrar o erro REAL do MySQL na tela (ex: "Column 'Email' cannot be null")
                var mensagemReal = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ModelState.AddModelError("", "Erro detalhado: " + mensagemReal);
            }
        }
        return View(corredorModel);
    }*/

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

    public ActionResult Delete(int id)
    {
        Corredor corredor = _corredorService.Get(id);
        CorredorViewModel corredorModel = _mapper.Map<CorredorViewModel>(corredor);
        return View(corredorModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, CorredorViewModel corredorModel)
    {
        _corredorService.Delete(id);
        return RedirectToAction(nameof(Index));
    }
}
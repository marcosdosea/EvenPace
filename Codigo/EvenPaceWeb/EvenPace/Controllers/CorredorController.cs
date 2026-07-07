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
    private readonly UserManager<UsuarioIdentity> _userManager;
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
        cpf = cpf.Replace(".", "").Replace("-", "").Trim();

        var result = await _signInManager.PasswordSignInAsync(
            cpf,
            senha,
            false,
            false
        );

        if (result.Succeeded)
            return RedirectToAction("IndexUsuario", "Evento");

        ModelState.AddModelError("", "CPF ou senha inválidos.");
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
        if (!string.IsNullOrWhiteSpace(corredorModel.CPF))
        {
            corredorModel.CPF = corredorModel.CPF
                .Replace(".", "")
                .Replace("-", "")
                .Trim();
        }

        // Como o CPF foi alterado depois do binding,
        // removemos a validação anterior e validamos novamente.
        ModelState.Remove(nameof(corredorModel.CPF));

        if (string.IsNullOrWhiteSpace(corredorModel.CPF) ||
            corredorModel.CPF.Length != 11)
        {
            ModelState.AddModelError(
                nameof(corredorModel.CPF),
                "O CPF deve conter 11 números."
            );
        }

        if (!ModelState.IsValid)
            return View(corredorModel);

        var identityUser = new UsuarioIdentity
        {
            UserName = corredorModel.CPF,
            Email = corredorModel.Email,
            EmailConfirmed = false
        };

        var result = await _userManager.CreateAsync(
            identityUser,
            corredorModel.Senha
        );

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(corredorModel);
        }

        var roleResult = await _userManager.AddToRoleAsync(identityUser, "Corredor");

        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(identityUser);

            foreach (var error in roleResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(corredorModel);
        }

        try
        {
            var corredor = _mapper.Map<Corredor>(corredorModel);

            // Garantia extra caso o Profile do AutoMapper esteja errado.
            corredor.Cpf = corredorModel.CPF;

            _corredorService.Create(corredor);

            return RedirectToAction(nameof(Login));
        }
        catch (Exception ex)
        {
            // Evita deixar usuário no Identity sem um corredor correspondente.
            await _userManager.DeleteAsync(identityUser);

            var erro = ex.InnerException?.Message ?? ex.Message;
            ModelState.AddModelError("", $"Erro ao cadastrar corredor: {erro}");

            return View(corredorModel);
        }
    }

    [HttpGet]
    public ActionResult Edit(int id)
    {
        Corredor corredor = _corredorService.Get(id);

        if (corredor == null)
        {
            return RedirectToAction(nameof(Login));
        }

        CorredorViewModel corredorModel = _mapper.Map<CorredorViewModel>(corredor);

        return View(corredorModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(CorredorViewModel corredorModel)
    {
        // E-mail e senha pertencem ao Identity e não são alterados nesta tela.
        ModelState.Remove(nameof(corredorModel.Email));
        ModelState.Remove(nameof(corredorModel.Senha));

        // Foto é opcional.
        ModelState.Remove(nameof(corredorModel.FotoPerfilUpload));

        if (!ModelState.IsValid)
        {
            return View(corredorModel);
        }

        var corredorExistente = _corredorService.Get(corredorModel.Id);

        if (corredorExistente == null)
        {
            return RedirectToAction(nameof(Login));
        }

        try
        {
            corredorExistente.Nome = corredorModel.Nome;
            corredorExistente.DataNascimento = corredorModel.DataNascimento;

            if (corredorModel.FotoPerfilUpload != null &&
                corredorModel.FotoPerfilUpload.Length > 0)
            {
                string novaFoto = SalvarFotoPerfil(corredorModel.FotoPerfilUpload);

                DeletarFotoPerfil(corredorExistente.FotoPerfil);

                corredorExistente.FotoPerfil = novaFoto;
            }

            _corredorService.Edit(corredorExistente);

            TempData["MensagemSucesso"] = "Perfil atualizado com sucesso!";
            return RedirectToAction(nameof(Get), new { id = corredorExistente.Id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(
                "",
                "Erro ao atualizar perfil: " +
                (ex.InnerException?.Message ?? ex.Message)
            );

            return View(corredorModel);
        }
    }

    [HttpGet]
    [Authorize]
    public IActionResult Delete(int id)
    {
        var cpfUsuarioLogado = User.Identity?.Name;

        if (string.IsNullOrWhiteSpace(cpfUsuarioLogado))
        {
            return RedirectToAction(nameof(Login));
        }

        var corredor = _corredorService.Get(id);

        if (corredor == null || corredor.Cpf != cpfUsuarioLogado)
        {
            return Forbid();
        }

        var corredorModel = _mapper.Map<CorredorViewModel>(corredor);

        return View(corredorModel);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var cpfUsuarioLogado = User.Identity?.Name;

        if (string.IsNullOrWhiteSpace(cpfUsuarioLogado))
        {
            return RedirectToAction(nameof(Login));
        }

        var corredor = _corredorService.Get(id);

        if (corredor == null || corredor.Cpf != cpfUsuarioLogado)
        {
            return Forbid();
        }

        try
        {
            var usuarioIdentity = await _userManager.FindByNameAsync(corredor.Cpf);

            if (usuarioIdentity == null)
            {
                TempData["MensagemErro"] =
                    "Usuário de autenticação não encontrado.";

                return RedirectToAction(nameof(Get), new { id });
            }

            var nomeFoto = corredor.FotoPerfil;

            _corredorService.Delete(corredor.Id);

            var resultadoIdentity = await _userManager.DeleteAsync(usuarioIdentity);

            if (!resultadoIdentity.Succeeded)
            {
                TempData["MensagemErro"] =
                    "O perfil foi removido, mas ocorreu um erro ao excluir o acesso de login.";

                return RedirectToAction(nameof(Login));
            }

            DeletarFotoPerfil(nomeFoto);

            await HttpContext.SignOutAsync();

            TempData["MensagemSucesso"] = "Sua conta foi excluída com sucesso.";

            return RedirectToAction(nameof(Login));
        }
        catch (Exception ex)
        {
            TempData["MensagemErro"] =
                "Erro ao excluir conta: " +
                (ex.InnerException?.Message ?? ex.Message);

            return RedirectToAction(nameof(Get), new { id });
        }
    }

    private string SalvarFotoPerfil(IFormFile foto)
    {
        string pasta = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            "imagens",
            "fotos-perfil"
        );

        if (!Directory.Exists(pasta))
        {
            Directory.CreateDirectory(pasta);
        }

        string extensao = Path.GetExtension(foto.FileName).ToLowerInvariant();
        string nomeArquivo = $"{Guid.NewGuid()}{extensao}";
        string caminhoCompleto = Path.Combine(pasta, nomeArquivo);

        using var stream = new FileStream(caminhoCompleto, FileMode.Create);
        foto.CopyTo(stream);

        return nomeArquivo;
    }

    private void DeletarFotoPerfil(string? nomeArquivo)
    {
        if (string.IsNullOrWhiteSpace(nomeArquivo))
        {
            return;
        }

        string caminho = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            "imagens",
            "fotos-perfil",
            nomeArquivo
        );

        if (System.IO.File.Exists(caminho))
        {
            System.IO.File.Delete(caminho);
        }
    }

}

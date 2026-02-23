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
    
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

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
    public ActionResult Create(CorredorViewModel corredorModel)
    {
        if (ModelState.IsValid)
        {
            var corredor = _mapper.Map<Corredor>(corredorModel);
            _corredorService.Create(corredor);
        }
        return View(corredorModel);
    }

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
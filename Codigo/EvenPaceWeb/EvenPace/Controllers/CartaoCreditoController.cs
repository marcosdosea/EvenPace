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

    public ActionResult Index()
    {
        var cartaoCredito = _cartaoCredito.GetAll();
        var cartaoCreditoViewModels = _mapper.Map<List<CartaoCreditoViewModel>>(cartaoCredito);
        return View(cartaoCreditoViewModels);
    }

    public ActionResult Details(int id)
    {
        var cartaoCredito = _cartaoCredito.Get((int)id);
        var cartaoCreditoViewModel = _mapper.Map<CartaoCreditoViewModel>(cartaoCredito);
        return View(cartaoCreditoViewModel);
    }

    public ActionResult Create()
    {
        return View();
    }

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

    public ActionResult Edit(int id)
    {
        var cartaoCredito = _cartaoCredito.Get((int)id);
        var cartaoCreditoViewModel = _mapper.Map<CartaoCreditoViewModel>(cartaoCredito);
        return View(cartaoCreditoViewModel);
    }

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

    public ActionResult Delete(int id)
    {
        var cartaoCredito = _cartaoCredito.Get((int)id);
        var cartaoCreditoViewModel = _mapper.Map<CartaoCreditoViewModel>(cartaoCredito);
        return View(cartaoCreditoViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, CartaoCreditoViewModel cartaoCreditoViewModel)
    {
        _cartaoCredito.Delete((int)id);
        return RedirectToAction(nameof(Index));
    }
}
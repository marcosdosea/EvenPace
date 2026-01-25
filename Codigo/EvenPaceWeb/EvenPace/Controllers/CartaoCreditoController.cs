using AutoMapper;
using Core;
using Core.Service;
using EvenPaceWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace EvenPace.Controllers;

public class CartaoCreditoControler : Controller
{
    private ICartaoCredito _cartaoCredito;
    private IMapper _mapper;

    public CartaoCreditoControler(ICartaoCredito cartaoCredito, IMapper mapper)
    {
        _cartaoCredito = cartaoCredito;
        _mapper = mapper;
    }

    // GET: CartaoCreditoControler
    public ActionResult Index()
    {
        var cartaoCredito = _cartaoCredito.GetAll();
        var cartaoCreditoViewModels = _mapper.Map<List<CartaoCreditoViewModel>>(cartaoCredito);
        return View(cartaoCreditoViewModels);
    }

    // GET: CartaoCreditoControler/Details/5
    public ActionResult Details(int id)
    {
        var cartaoCredito = _cartaoCredito.Get((int)id);
        var cartaoCreditoViewModel = _mapper.Map<CartaoCreditoViewModel>(cartaoCredito);
        return View(cartaoCreditoViewModel);
    }

    // GET: CartaoCreditoControler/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: CartaoCreditoControler/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(CartaoCreditoViewModel cartaoCreditoViewModel)
    {
        if (ModelState.IsValid)
        {
            var cartaoCredito = _mapper.Map<Core.Cartaocredito>(cartaoCreditoViewModel);
            _cartaoCredito.Create(cartaoCredito);
        }
        return RedirectToAction(nameof(Index));
    }

    // GET: CartaoCreditoControler/Edit/5
    public ActionResult Edit(int id)
    {
        var cartaoCredito = _cartaoCredito.Get((int)id);
        var cartaoCreditoViewModel = _mapper.Map<CartaoCreditoViewModel>(cartaoCredito);
        return View(cartaoCreditoViewModel);
    }

    // POST: CartaoCreditoControler/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
    {
        if (ModelState.IsValid)
        {
            var cartaoCredito = _mapper.Map<Core.Cartaocredito>(collection);
            _cartaoCredito.Edit(cartaoCredito);
        }
        return RedirectToAction(nameof(Index));
    }

    // GET: CartaoCreditoControler/Delete/5
    public ActionResult Delete(int id)
    {
        var cartaoCredito = _cartaoCredito.Get((int)id);
        var cartaoCreditoViewModel = _mapper.Map<CartaoCreditoViewModel>(cartaoCredito);
        return View(cartaoCreditoViewModel);
    }

    // POST: CartaoCreditoControler/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, CartaoCreditoViewModel cartaoCreditoViewModel)
    {
        _cartaoCredito.Delete((int)id);
        return RedirectToAction(nameof(Index));
    }
}
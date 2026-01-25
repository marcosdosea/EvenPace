using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core.Service;
using AutoMapper;
using EvenPaceWeb.Models;
namespace EvenPaceWeb.Controllers
{
    public class CartaoCreditoController : Controller
    {
        private readonly ICartaoCreditoService _cartaoCreditoService;
        private readonly IMapper _mapper;

        public CartaoCreditoController(ICartaoCreditoService cartaoCreditoService, IMapper mapper)
        {
            _cartaoCreditoService = cartaoCreditoService;
            _mapper = mapper;
        }
        // GET: CartaoCreditoController
        public ActionResult Index()
        {
            var cartoes = _cartaoCreditoService.GetAll();
            var cartaoViewModels = _mapper.Map<List<CartaoCreditoViewModel>>(cartoes);
            return View(cartaoViewModels);
        }

        // GET: CartaoCreditoController/Details/5
        public ActionResult Details(int id)
        {
            var cartao = _cartaoCreditoService.Get((int)id);
            var cartaoViewModel = _mapper.Map<CartaoCreditoViewModel>(cartao);
            return View(cartaoViewModel);
        }

        // GET: CartaoCreditoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CartaoCreditoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CartaoCreditoViewModel cartaoCreditoViewModel)
        {
            if (ModelState.IsValid)
            {
                var cartao = _mapper.Map<Core.Cartaocredito>(cartaoCreditoViewModel);
                _cartaoCreditoService.Create(cartao);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: CartaoCreditoController/Edit/5
        public ActionResult Edit(int id)
        {
            var cartao = _cartaoCreditoService.Get((int)id);
            var cartaoViewModel = _mapper.Map<CartaoCreditoViewModel>(cartao);
            return View(cartaoViewModel);
        }

        // POST: CartaoCreditoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CartaoCreditoViewModel cartaoCreditoViewModel)
        {
            if (ModelState.IsValid)
            {
                var cartao = _mapper.Map<Core.Cartaocredito>(cartaoCreditoViewModel);
                _cartaoCreditoService.Edit(cartao);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: CartaoCreditoController/Delete/5
        public ActionResult Delete(int id)
        {
            var cartao = _cartaoCreditoService.Get((int)id);
            var cartaoViewModel = _mapper.Map<CartaoCreditoViewModel>(cartao);
            return View(cartaoViewModel);
        }

        // POST: CartaoCreditoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            _cartaoCreditoService.Delete((int)id);
            return RedirectToAction(nameof(Index));
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core.Service;
using AutoMapper;
using EvenPaceWeb.Models;

namespace EvenPaceWeb.Controllers
{
    public class AdministradorController : Controller
    {
        private readonly IAdministradorService _administradorService;
        private readonly IMapper _mapper;

        public AdministradorController(IAdministradorService administradorService, IMapper mapper)
        {
            _administradorService = administradorService;
            _mapper = mapper;
        }
        // GET: AdministradorController
        public ActionResult Index()
        {
            var administradores = _administradorService.GetAll();
            var administradorViewModels = _mapper.Map<List<AdministradorViewModel>>(administradores);
            return View(administradorViewModels);
        }

        // GET: AdministradorController/Details/5
        public ActionResult Details(int id)
        {
            var administrador = _administradorService.Get((int)id);
            var administradorViewModel = _mapper.Map<AdministradorViewModel>(administrador);
            return View(administradorViewModel);
        }

        // GET: AdministradorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdministadorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AdministradorViewModel administradorViewModel)
        {
            if (ModelState.IsValid)
            {
                var administrador = _mapper.Map<Core.Admistrador>(administradorViewModel);
                _administradorService.Create(administrador);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: AdministadorController/Edit/5
        public ActionResult Edit(int id)
        {
            var administrador = _administradorService.Get((int)id);
            var administradorViewModel = _mapper.Map<AdministradorViewModel>(administrador);
            return View(administradorViewModel);
        }

        // POST: AdministadorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                var administrador = _mapper.Map<Core.Admistrador>(collection);
                _administradorService.Edit(administrador);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: AdministadorController/Delete/5
        public ActionResult Delete(int id)
        {
            var administrador = _administradorService.Get((int)id);
            var administradorViewModel = _mapper.Map<AdministradorViewModel>(administrador);
            return View(administradorViewModel);
        }

        // POST: AdministadorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, AdministradorViewModel administradorViewModel)
        {
            _administradorService.Delete((int)id);
            return RedirectToAction(nameof(Index));
        }
    }
}

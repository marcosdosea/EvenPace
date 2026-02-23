using Microsoft.AspNetCore.Mvc;
using Core.Service;
using AutoMapper;
using Models;

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

        public ActionResult Index()
        {
            var administradores = _administradorService.GetAll();
            var administradorViewModels = _mapper.Map<List<AdministradorViewModel>>(administradores);
            return View(administradorViewModels);
        }

        public ActionResult Details(int id)
        {
            var administrador = _administradorService.Get((int)id);
            var administradorViewModel = _mapper.Map<AdministradorViewModel>(administrador);
            return View(administradorViewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AdministradorViewModel administradorViewModel)
        {
            if (ModelState.IsValid)
            {
                var administrador = _mapper.Map<Core.Administrador>(administradorViewModel);
                _administradorService.Create(administrador);
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Edit(int id)
        {
            var administrador = _administradorService.Get((int)id);
            var administradorViewModel = _mapper.Map<AdministradorViewModel>(administrador);
            return View(administradorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                var administrador = _mapper.Map<Core.Administrador>(collection);
                _administradorService.Edit(administrador);
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int id)
        {
            var administrador = _administradorService.Get((int)id);
            var administradorViewModel = _mapper.Map<AdministradorViewModel>(administrador);
            return View(administradorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, AdministradorViewModel administradorViewModel)
        {
            _administradorService.Delete((int)id);
            return RedirectToAction(nameof(Index));
        }
    }
}
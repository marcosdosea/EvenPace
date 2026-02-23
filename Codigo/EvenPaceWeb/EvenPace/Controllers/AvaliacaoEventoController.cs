using AutoMapper;
using Core;
using Microsoft.AspNetCore.Mvc;
using Core.Service;
using Models;

namespace EvenPaceWeb.Controllers
{
    public class AvaliacaoEventoController : Controller
    {
        private readonly IAvaliacaoEventoService _avaliacaoEventoService;
        private readonly IMapper _mapper;

        public AvaliacaoEventoController(IAvaliacaoEventoService avaliacaoEventoService, IMapper mapper)
        {
            _avaliacaoEventoService = avaliacaoEventoService;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            var avaliacoes = _avaliacaoEventoService.GetAll();
            var viewModels = _mapper.Map<List<AvaliacaoEventoViewModel>>(avaliacoes);
            return View(viewModels);
        }

        public ActionResult Details(string nome)
        {
            var avaliacao = _avaliacaoEventoService.GetByName((string)nome);
            var viewModel = _mapper.Map<AvaliacaoEventoViewModel>(avaliacao);
            return View(viewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AvaliacaoEventoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var avaliacao = _mapper.Map<Core.AvaliacaoEvento>(viewModel);
                _avaliacaoEventoService.Create(avaliacao);
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        public ActionResult Edit(int id)
        {
            var avaliacao = _avaliacaoEventoService.Get((int)id);
            var viewModel = _mapper.Map<AvaliacaoEventoViewModel>(avaliacao);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AvaliacaoEventoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var avaliacao = _mapper.Map<Core.AvaliacaoEvento>(viewModel);
                _avaliacaoEventoService.Edit(avaliacao);
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        public ActionResult Delete(int id)
        {
            var avaliacao = _avaliacaoEventoService.Get((int)id);
            var viewModel = _mapper.Map<AvaliacaoEventoViewModel>(avaliacao);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, AvaliacaoEventoViewModel viewModel)
        {
            _avaliacaoEventoService.Delete((int)id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AvaliarEvento(AvaliacaoEventoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var avaliacao = _mapper.Map<AvaliacaoEvento>(model);
                _avaliacaoEventoService.Create(avaliacao);

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
    }
}
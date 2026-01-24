using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Core.Service;
using EvenPaceWeb.Models;

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

        /// <summary>
        /// Lista todas as avaliações de eventos
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var avaliacoes = _avaliacaoEventoService.GetAll();
            var viewModels = _mapper.Map<List<AvaliacaoEventoViewModel>>(avaliacoes);
            return View(viewModels);
        }

        /// <summary>
        /// Retorna os detalhes de uma avaliação de evento específicada pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            var avaliacao = _avaliacaoEventoService.Get((int)id);
            var viewModel = _mapper.Map<AvaliacaoEventoViewModel>(avaliacao);
            return View(viewModel);
        }

        /// <summary>
        /// Retorna o formulário de criação de uma nova avaliação de evento
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Cria uma nova avaliação de evento a partir do viewModel
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AvaliacaoEventoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var avaliacao = _mapper.Map<Core.Avaliacaoevento>(viewModel);
                _avaliacaoEventoService.Create(avaliacao);
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        /// <summary>
        /// Retorna o formulário de edição de uma avaliação de evento pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            var avaliacao = _avaliacaoEventoService.Get((int)id);
            var viewModel = _mapper.Map<AvaliacaoEventoViewModel>(avaliacao);
            return View(viewModel);
        }

        /// <summary>
        /// Edita uma avaliação de evento a partir do viewModel
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AvaliacaoEventoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var avaliacao = _mapper.Map<Core.Avaliacaoevento>(viewModel);
                _avaliacaoEventoService.Edit(avaliacao);
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        /// <summary>
        /// Retorna o formulário de confirmação de exclusão de uma avaliação de evento pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            var avaliacao = _avaliacaoEventoService.Get((int)id);
            var viewModel = _mapper.Map<AvaliacaoEventoViewModel>(avaliacao);
            return View(viewModel);
        }

        /// <summary>
        /// Exclui uma avaliação de evento pelo id e viewModel
        /// </summary>
        /// <param name="id"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, AvaliacaoEventoViewModel viewModel)
        {
            _avaliacaoEventoService.Delete((int)id);
            return RedirectToAction(nameof(Index));
        }
    }
}

using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace EvenPace.Controllers
{
    public class InscricaoController : Controller
    {
        private readonly IInscricaoService _inscricaoService;
        private readonly IEventoService _eventoService;
        private readonly IKitService _kitService;
        private readonly IMapper _mapper;

        public InscricaoController(
            IInscricaoService inscricaoService,
            IEventoService eventoService,
            IKitService kitService,
            IMapper mapper)
        {
            _inscricaoService = inscricaoService;
            _eventoService = eventoService;
            _kitService = kitService;
            _mapper = mapper;
        }

        // GET: Inscricao/Index
        public ActionResult Index()
        {
            var listaInscricao = _inscricaoService.GetAll();
            var listaInscricaoModel = _mapper.Map<List<InscricaoViewModel>>(listaInscricao);
            return View(listaInscricaoModel);
        }

        // GET: Inscricao/Get/2
        public ActionResult Get(int id)
        {
            var inscricao = _inscricaoService.Get(id);
            var inscricaoModel = _mapper.Map<InscricaoViewModel>(inscricao);
            return View(inscricaoModel);
        }

        // GET: Inscricao/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inscricao/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InscricaoViewModel inscricaoModel)
        {
            if (!ModelState.IsValid)
                return View(inscricaoModel);

            var inscricao = _mapper.Map<Inscricao>(inscricaoModel);
            _inscricaoService.Create(inscricao);

            return RedirectToAction(nameof(Index));
        }

        // GET: Inscricao/Edit/4
        public ActionResult Edit(int id)
        {
            var inscricao = _inscricaoService.Get(id);
            var inscricaoModel = _mapper.Map<InscricaoViewModel>(inscricao);
            return View(inscricaoModel);
        }

        // POST: Inscricao/Edit/4
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, InscricaoViewModel inscricaoModel)
        {
            if (!ModelState.IsValid)
                return View(inscricaoModel);

            var inscricao = _mapper.Map<Inscricao>(inscricaoModel);
            _inscricaoService.Edit(inscricao);

            return RedirectToAction(nameof(Index));
        }

        // GET: Inscricao/Delete/1
        public ActionResult Delete(int id)
        {
            var inscricao = _inscricaoService.Get(id);
            var inscricaoModel = _mapper.Map<InscricaoViewModel>(inscricao);
            return View(inscricaoModel);
        }

        // POST: Inscricao/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            _inscricaoService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

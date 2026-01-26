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

        public ActionResult Index()
        {
            var listaInscricao = _inscricaoService.GetAll();
            var listaInscricaoModel = _mapper.Map<List<InscricaoViewModel>>(listaInscricao);
            return View(listaInscricaoModel);
        }

        public ActionResult Get(int id)
        {
            var inscricao = _inscricaoService.Get(id);
            var inscricaoModel = _mapper.Map<InscricaoViewModel>(inscricao);
            return View(inscricaoModel);
        }

        public ActionResult Create()
        {
            return View();
        }

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

        public ActionResult Edit(int id)
        {
            var inscricao = _inscricaoService.Get(id);
            var inscricaoModel = _mapper.Map<InscricaoViewModel>(inscricao);
            return View(inscricaoModel);
        }

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

        public ActionResult Delete(int id)
        {
            var inscricao = _inscricaoService.Get(id);
            var inscricaoModel = _mapper.Map<InscricaoViewModel>(inscricao);
            return View(inscricaoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            _inscricaoService.Delete(id);
            return RedirectToAction(nameof(Index));
        }


        // GET
        public IActionResult Tela14_InscricaoNaCorrida1(int idEvento)
        {
            var evento = _eventoService.Get(idEvento);
            var kits = _kitService.GetByEvento(idEvento);

            var vm = new TelaInscricaoViewModel
            {
                IdEvento = evento.Id,
                NomeEvento = evento.Nome,
                ImagemEvento = evento.Imagem,
                Local = evento.Local,
                DataEvento = evento.DataEvento,
                Descricao = evento.Descricao,
                Percursos = new List<string> { "3km", "5km", "10km" },
                Kits = _mapper.Map<List<KitViewModel>>(kits),
                Inscricao = new InscricaoViewModel
                {
                    IdEvento = evento.Id,
                    DataInscricao = DateTime.Now
                }
            };

            return View(vm);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SalvarInscricao(TelaInscricaoViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("Tela14_InscricaoNaCorrida1", vm);

            var inscricao = _mapper.Map<Inscricao>(vm.Inscricao);
            _inscricaoService.Create(inscricao);

            TempData["MensagemSucesso"] = "Inscrição realizada com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}

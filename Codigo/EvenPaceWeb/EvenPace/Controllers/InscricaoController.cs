using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Models;
using ;

namespace EvenPace.Controllers
{
    public class InscricaoController : Controller
    {
        private readonly IInscricaoService _inscricaoService;
        private readonly IEventosService _eventoService;
        private readonly IKitService _kitService;
        private readonly IMapper _mapper;

        public InscricaoController(
            IInscricaoService inscricaoService,
            IEventosService eventoService,
            IKitService kitService,
            IMapper mapper)
        {
            _inscricaoService = inscricaoService;
            _eventoService = eventoService;
            _kitService = kitService;
            _mapper = mapper;
        }

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
                    DataInscricao = DateTime.Now,
                    IdCorredor = 1 
                }
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SalvarInscricao(TelaInscricaoViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var evento = _eventoService.Get(vm.IdEvento);
                var kits = _kitService.GetByEvento(vm.IdEvento);

                vm.NomeEvento = evento.Nome;
                vm.ImagemEvento = evento.Imagem;
                vm.Local = evento.Local;
                vm.DataEvento = evento.DataEvento;
                vm.Descricao = evento.Descricao;
                vm.Percursos = new List<string> { "3km", "5km", "10km" };
                vm.Kits = _mapper.Map<List<KitViewModel>>(kits);

                return View("Tela14_InscricaoNaCorrida1", vm);
            }

            var inscricao = _mapper.Map<Inscricao>(vm.Inscricao);
            _inscricaoService.Create(inscricao);

            TempData["MensagemSucesso"] = "Inscrição realizada com sucesso!";
            return RedirectToAction("Index", "Home");
        }
    }
}

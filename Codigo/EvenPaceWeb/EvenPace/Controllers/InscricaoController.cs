using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using EvenPace.Models;
using Models;
using EvenPaceWeb.Models;
using Service;

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

        public IActionResult Inscricao(int id)
        {
            var evento = _eventoService.Get(id); // Ou o método que você usa para buscar

            if (evento == null)
            {
                return NotFound("Evento não encontrado no banco de dados.");
            }

            var kitsFromDb = _kitService.GetKitsPorEvento(id).ToList();

            var vm = new TelaInscricaoViewModel
            {
                IdEvento = evento.Id,
                NomeEvento = evento.Nome,
                Local = evento.Rua,
                DataEvento = evento.Data,
                Descricao = evento.Descricao,
                Percursos = new List<string> { "3km", "5km", "10km" },
                Kits = _mapper.Map<List<KitViewModel>>(kitsFromDb),
                Inscricao = new InscricaoViewModel
                {
                    IdEvento = (int)evento.Id,
                    DataInscricao = DateTime.Now,
                    IdCorredor = 1
                }
            };

            return View("TelaInscricao", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SalvarInscricao(TelaInscricaoViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var evento = _eventoService.Get((int)vm.IdEvento);
                var kits = _kitService.Get((int)vm.IdEvento);

                vm.NomeEvento = evento.Nome;
                //vm.ImagemEvento = evento.Imagem;
                vm.Local = evento.Cidade;
                vm.DataEvento = evento.Data;
                vm.Descricao = evento.Descricao;
                vm.Percursos = new List<string> { "3km", "5km", "10km" };
                vm.Kits = _mapper.Map<List<KitViewModel>>(kits);

                return View("TelaInscricao", vm);
            }

            var inscricao = _mapper.Map<Inscricao>(vm.Inscricao);
            _inscricaoService.Create(inscricao);

            TempData["MensagemSucesso"] = "Inscrição realizada com sucesso!";
            return RedirectToAction("Index", "Home");
        }
    }
}

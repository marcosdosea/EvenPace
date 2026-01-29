using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using EvenPaceWeb.Models;
using Service;
using Models;

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

    
        [HttpGet]
        public IActionResult TelaInscricao(uint id)
        {
            if (id == 0)
                return BadRequest("https://localhost:7131/Inscricao/TelaInscricao/1");

            var vm = new TelaInscricaoViewModel
            {
                IdEvento = id,
                Inscricao = new InscricaoViewModel
                {

                    IdEvento = id
                }
            };

            PopularTelaInscricao(vm);
            return View(vm);
        }

        [HttpGet]
        public IActionResult Tela1(uint id)
        {
            if (id == 0)
                return Content("ID RECEBIDO = 0");

            var vm = new TelaInscricaoViewModel
            {
                IdEvento = id,
                Inscricao = new InscricaoViewModel
                {
                    IdEvento = id
                }
            };

            PopularTelaInscricao(vm);

            return View("Tela1", vm); // 👈 Tela1.cshtml
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TelaInscricao(TelaInscricaoViewModel vm)
        {
            if (vm?.Inscricao == null)
                throw new Exception("Inscrição veio null");

            if (vm.Inscricao.IdEvento == 0)
                throw new Exception("IdEvento não veio do formulário");

            var idCorredorClaim = User.FindFirst("IdCorredor");
            if (idCorredorClaim == null)
            {
                TempData["Erro"] = "Faça login para continuar";
                return RedirectToAction(
                    "TelaInscricao",
                    new { id = vm.Inscricao.IdEvento }
                );
            }

            var inscricao = new Inscricao
            {
                Status = "Pendente",
                DataInscricao = DateTime.Now,
                Distancia = vm.Inscricao.Distancia,
                TamanhoCamisa = vm.Inscricao.TamanhoCamisa,
                IdEvento = (uint)vm.Inscricao.IdEvento,
                IdKit = (uint)vm.Inscricao.IdKit,
                IdCorredor = (uint)int.Parse(idCorredorClaim.Value)
            };

            _inscricaoService.Create(inscricao);

            TempData["Sucesso"] = "Inscrição realizada com sucesso!";

            return RedirectToAction(
                "TelaInscricao",
                new { id = vm.Inscricao.IdEvento }
            );
        }

      
        private void PopularTelaInscricao(TelaInscricaoViewModel vm)
        {
            if (vm == null)
                throw new Exception("ViewModel está null");

            if (vm.IdEvento == 0)
                throw new Exception("IdEvento não foi informado");

            var evento = _eventoService.Get((int)vm.IdEvento);
            if (evento == null)
                throw new Exception($"Evento {vm.IdEvento} não existe no banco");

            var kits = _kitService.GetKitsPorEvento((int)vm.IdEvento);

            vm.NomeEvento = evento.Nome;
            vm.Local = evento.Cidade;
            vm.DataEvento = evento.Data;
            vm.Descricao = evento.Descricao;
            vm.InfoRetiradaKit = evento.InfoRetiradaKit;

            vm.Percursos = new List<string> { "3km", "5km", "10km" };
            vm.Kits = _mapper.Map<List<KitViewModel>>(kits);
        }
    }
}

using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using EvenPaceWeb.Models;
using Service;
using Models;

//https://localhost:7131/Inscricao/TelaInscricao/1 para rodar 
namespace EvenPace.Controllers
{
    public class InscricaoController : Controller
    {
        private readonly IInscricaoService _inscricaoService;
        private readonly IEventosService _eventoService;
        private readonly IKitService _kitService;
        private readonly IMapper _mapper;
        private readonly ICorredorService _corredorService;


        public InscricaoController(
            IInscricaoService inscricaoService,
            IEventosService eventoService,
            IKitService kitService,
            ICorredorService corredorService,
            IMapper mapper)
        {
            _inscricaoService = inscricaoService;
            _eventoService = eventoService;
            _kitService = kitService;
            _corredorService = corredorService;
            _mapper = mapper;
        }
       
        [HttpGet]
        public IActionResult Cancelar(int id)
        {
            var inscricao = _inscricaoService.Get(id);

            if (inscricao == null)
                return NotFound("Inscrição não encontrada");

            Kit? kit = null;
            if (inscricao.IdKit.HasValue)
                kit = _kitService.Get(inscricao.IdKit.Value);

            var evento = _eventoService.Get(inscricao.IdEvento);

            if (evento.Data < DateTime.Now)
            {
                TempData["Erro"] = "Não é possível cancelar após a data do evento.";
                return RedirectToAction("Index", "Home");
            }

            var vm = new TelaInscricaoViewModel
            {
                NomeEvento = evento.Nome,
                DataEvento = evento.Data,
                Local = evento.Cidade,
                NomeKit = kit?.Nome ?? "Sem kit",
                Inscricao = new InscricaoViewModel
                {
                    Id = (uint)inscricao.Id,
                    Distancia = inscricao.Distancia,
                    TamanhoCamisa = inscricao.TamanhoCamisa,
                    DataInscricao = inscricao.DataInscricao
                }
            };

            return View("CancelarInscricao", vm);
        }


        public IActionResult TelaInscricao(int id)
        {
            if (id == 0)
                return BadRequest("https://localhost:5157/Inscricao/TelaInscricao/1");

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
        public IActionResult Tela1(int id)
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

            return View("Tela1", vm); 
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancelar(int idInscricao, int idEvento)
        {
            var idCorredorClaim = User.FindFirst("IdCorredor");
            if (idCorredorClaim == null)
            {
                TempData["Erro"] = "Faça login para cancelar a inscrição.";
                return RedirectToAction("TelaInscricao", new { id = idEvento });
            }

            try
            {
                _inscricaoService.Cancelar(
                    idInscricao,
                    int.Parse(idCorredorClaim.Value)
                );

                TempData["Sucesso"] = "Inscrição cancelada com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = ex.Message;
            }

            return RedirectToAction("TelaInscricao", new { id = idEvento });
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
                IdEvento = (int)vm.Inscricao.IdEvento,
                IdKit = (int)vm.Inscricao.IdKit,
                IdCorredor = int.Parse(idCorredorClaim.Value)
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

            var evento = _eventoService.Get(vm.IdEvento);
            if (evento == null)
                throw new Exception($"Evento {vm.IdEvento} não existe no banco");

            var kits = _kitService.GetKitsPorEvento((int)vm.IdEvento);

            vm.NomeEvento = evento.Nome;
            vm.Local = evento.Cidade;
            vm.DataEvento = evento.Data;
            vm.Descricao = evento.Descricao;
           // vm.InfoRetiradaKit = evento.InfoRetiradaKit;

            vm.Percursos = new List<string> { "3km", "5km", "10km" };
            vm.Kits = _mapper.Map<List<KitViewModel>>(kits);
        }

        public ActionResult GetAllByEvento(int  idEvento)
        {
            var inscricao = _inscricaoService.GetAllByEvento(idEvento);
            var inscricaoViewModel = _mapper.Map<List<InscricaoViewModel>>(inscricao);
            return View(inscricaoViewModel);
        }
    }
}

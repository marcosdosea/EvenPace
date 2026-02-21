using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
//using EvenPaceWeb.Models;
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
        public IActionResult Delete(int id)
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

            var vm = new InscricaoViewModel
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

            return View("Delete", vm);
        }


        public IActionResult Index(int id)
        {
            var vm = new InscricaoViewModel
            {
                IdEvento = id,
                Inscricao = new InscricaoViewModel
                {
                    IdEvento = id
                }
            };

            ConfigurarInscricao(vm);
            return View(vm);
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            var vm = new InscricaoViewModel
            {
                IdEvento = id,
                Inscricao = new InscricaoViewModel
                {
                    IdEvento = id
                }
            };

            ConfigurarInscricao(vm);

            return View("Create", vm); 
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int idInscricao, int idEvento)
        {
            var idCorredorClaim = User.FindFirst("IdCorredor");
            if (idCorredorClaim == null)
            {
                TempData["Erro"] = "Faça login para cancelar a inscrição.";
                return RedirectToAction("Index", new { id = idEvento });
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

            return RedirectToAction("Index", new { id = idEvento });
        }

        public IActionResult salve()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(InscricaoViewModel vm)
        {
            return Content("POST EXECUTOU");
            var idCorredorClaim = User.FindFirst("IdCorredor");

            int idCorredor;

            if (idCorredorClaim != null)
            {
                idCorredor = int.Parse(idCorredorClaim.Value);
            }
            else
            {
                // Verificar se já existe corredor com esse email
                var corredorExistente = _corredorService.GetByEmail(vm.Inscricao.Email);

                if (corredorExistente != null)
                {
                    idCorredor = corredorExistente.Id;
                }
                else
                {
                    var novoCorredor = new Corredor
                    {
                        Nome = vm.Inscricao.Nome,
                        Email = vm.Inscricao.Email,
                        Senha = "temporaria", // ou gerar hash depois
                        Cpf = "00000000000",
                        DataNascimento = DateTime.Now
                    };

                    idCorredor = _corredorService.Create(novoCorredor);
                }
            }

            var inscricao = new Inscricao
            {
                Status = "Pendente",
                DataInscricao = DateTime.Now,
                Distancia = vm.Inscricao.Distancia,
                TamanhoCamisa = vm.Inscricao.TamanhoCamisa,
                IdEvento = (int)vm.Inscricao.IdEvento,
                IdKit = vm.Inscricao.IdKit,
                IdCorredor = idCorredor
            };

            _inscricaoService.Create(inscricao);

            TempData["Sucesso"] = "Inscrição criada com sucesso!";

            return RedirectToAction("salve");
        }

        private void ConfigurarInscricao(InscricaoViewModel vm)
        {
            var evento = _eventoService.Get(vm.IdEvento);
            if (evento == null)
                throw new Exception($"Evento {vm.IdEvento} não existe no banco");

            var kits = _kitService.GetKitsPorEvento((int)vm.IdEvento);

            vm.NomeEvento = evento.Nome;
            vm.Local = evento.Cidade;
            vm.DataEvento = evento.Data;
            vm.Descricao = evento.Descricao;
            vm.ImagemEvento = evento.Imagem;
            vm.InfoRetiradaKit = evento.InfoRetiradaKit;

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

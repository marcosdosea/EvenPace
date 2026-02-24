using AutoMapper;
using Core;
using Core.Service;
using EvenPaceWeb.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service;
using Models;

namespace EvenPace.Controllers
{
    public class InscricaoController : Controller
    {
        private readonly IInscricaoService _inscricaoService;
        private readonly IEventosService _eventoService;
        private readonly IKitService _kitService;
        private readonly UserManager<UsuarioIdentity> _userManager;
        private readonly ICorredorService _corredorService;
        private readonly IMapper _mapper;

        public InscricaoController(
            IInscricaoService inscricaoService,
            IEventosService eventoService,
            IKitService kitService,
            ICorredorService corredorService,
            UserManager<UsuarioIdentity> userManager,
            IMapper mapper)
        {
            _inscricaoService = inscricaoService;
            _eventoService = eventoService;
            _kitService = kitService;
            _corredorService = corredorService;
            _userManager = userManager;
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
        public IActionResult Create(int idEvento, int idKit)
        {
            var vm = new InscricaoViewModel
            {
                IdEvento = idEvento,
                IdKit = idKit,
                Inscricao = new InscricaoViewModel
                {
                    IdEvento = idEvento,
                    IdKit = idKit
                }
            };

            ConfigurarInscricao(vm);
            
            Console.WriteLine("=== DEBUG INSCRIÇÃO ===");
            Console.WriteLine($"IdEvento: {vm.IdEvento}");
            Console.WriteLine($"IdKit: {vm.IdKit}");

            return View("Create", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InscricaoViewModel model)
        {
            var usuarioIdentity = await _userManager.GetUserAsync(User);
            
            var corredor = _corredorService.GetByCpf(usuarioIdentity.UserName);
                
            Console.WriteLine($"Distancia: {model.Distancia}");
            Console.WriteLine($"TamanhoCamisa: {model.TamanhoCamisa}");
            Console.WriteLine($"IdCorredor: {corredor?.Id}");
            var inscricao = new Inscricao()
            {
                IdEvento = model.IdEvento,
                IdCorredor = corredor.Id,
                IdKit = model.IdKit,
                DataInscricao = DateTime.Now,
                Status = "Pendente",
                Distancia = model.Distancia,
                TamanhoCamisa = model.TamanhoCamisa,
                StatusRetiradaKit = false,
                Tempo = null,
                Posicao = null,
                IdAvaliacaoEvento = null
            };
            await _inscricaoService.CreateAsync(inscricao);

            TempData["MensagemSucesso"] = "Inscrição salva com sucesso!";
            return RedirectToAction("IndexUsuario", "Evento");   
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(InscricaoViewModel vm)
        {
            try
            {
                var idCorredorClaim = User.FindFirst("IdCorredor");

                var inscricao = new Inscricao
                {
                    IdEvento = vm.Inscricao.IdEvento,
                    IdCorredor = int.Parse(idCorredorClaim.Value),
                    Distancia = vm.Inscricao.Distancia,
                    TamanhoCamisa = vm.Inscricao.TamanhoCamisa,
                    DataInscricao = DateTime.Now,
                    Status = "Ativa",
                    StatusRetiradaKit = false
                };

                _inscricaoService.Create(inscricao);

                TempData["Sucesso"] = "Inscrição realizada com sucesso!";
                return Content("SALVOU NO BANCO");
            }
            catch (Exception ex)
            {
                TempData["Erro"] = ex.Message;
                ConfigurarInscricao(vm);
                return View(vm);
            }
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

        private void ConfigurarInscricao(InscricaoViewModel vm)
        {
            var evento = _eventoService.Get(vm.IdEvento);
            if (evento == null)
                throw new Exception($"Evento {vm.IdEvento} não existe");

            var kits = _kitService.GetKitsPorEvento(vm.IdEvento);

            vm.NomeEvento = evento.Nome;
            vm.Local = evento.Cidade;
            vm.DataEvento = evento.Data;
            vm.Descricao = evento.Descricao;
            vm.ImagemEvento = evento.Imagem;
            vm.InfoRetiradaKit = evento.InfoRetiradaKit;

            vm.Percursos = new List<string> { "3km", "5km", "10km" };
            vm.Kits = _mapper.Map<List<KitViewModel>>(kits);
        }

        public ActionResult GetAllByEvento(int idEvento)
        {
            var inscricao = _inscricaoService.GetAllByEvento(idEvento);
            var inscricaoViewModel = _mapper.Map<List<InscricaoViewModel>>(inscricao);
            return View(inscricaoViewModel);
        }
    }
}
using AutoMapper;
using Core;
using Core.Service;
using Core.Service.Dtos;
using Microsoft.AspNetCore.Mvc;
using EvenPaceWeb.Models;
using Models;

namespace EvenPace.Controllers
{
    public class InscricaoController : Controller
    {
        private readonly IInscricaoService _inscricaoService;
        private readonly IMapper _mapper;

        public InscricaoController(IInscricaoService inscricaoService, IMapper mapper)
        {
            _inscricaoService = inscricaoService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var result = _inscricaoService.GetDadosTelaDelete(id);

            if (!result.Success)
            {
                if (result.ErrorType == "NotFound")
                    return NotFound("Inscrição não encontrada");
                TempData["Erro"] = "Não é possível cancelar após a data do evento.";
                return RedirectToAction("Index", "Home");
            }
            
            var dadosTelaDelete = result.Data!;
            var telaInscricaoViewModel = new TelaInscricaoViewModel
            {
                NomeEvento = dadosTelaDelete.NomeEvento,
                DataEvento = dadosTelaDelete.DataEvento,
                Local = dadosTelaDelete.Local,
                NomeKit = dadosTelaDelete.NomeKit,
                Inscricao = new InscricaoViewModel
                {
                    Id = (uint)dadosTelaDelete.IdInscricao,
                    Distancia = dadosTelaDelete.Distancia,
                    TamanhoCamisa = dadosTelaDelete.TamanhoCamisa,
                    DataInscricao = dadosTelaDelete.DataInscricao
                }
            };

            return View("Delete", telaInscricaoViewModel);
        }

        public IActionResult Index(int id)
        {
            var dto = _inscricaoService.GetDadosTelaInscricao(id);
            var vm = MontarTelaInscricaoViewModel(dto, id);
            return View(vm);
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            var dto = _inscricaoService.GetDadosTelaInscricao(id);
            var vm = MontarTelaInscricaoViewModel(dto, id);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(TelaInscricaoViewModel vm)
        {
            var idCorredorClaim = User.FindFirst("IdCorredor");
            if (idCorredorClaim == null)
            {
                TempData["Erro"] = "Faça login para continuar";
                return RedirectToAction("Index", new { id = vm.Inscricao.IdEvento });
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
            return RedirectToAction("Index", new { id = vm.Inscricao.IdEvento });
        }

        private TelaInscricaoViewModel MontarTelaInscricaoViewModel(DadosTelaInscricaoDto dto, int idEvento)
        {
            return new TelaInscricaoViewModel
            {
                IdEvento = idEvento,
                NomeEvento = dto.NomeEvento,
                Local = dto.Local,
                DataEvento = dto.DataEvento,
                Descricao = dto.Descricao,
                ImagemEvento = dto.ImagemEvento,
                Percursos = new List<string> { "3km", "5km", "10km" },
                Kits = _mapper.Map<List<KitViewModel>>(dto.Kits),
                Inscricao = new InscricaoViewModel { IdEvento = idEvento }
            };
        }

        public ActionResult GetAllByEvento(int idEvento)
        {
            var inscricao = _inscricaoService.GetAllByEvento(idEvento);
            var inscricaoViewModel = _mapper.Map<List<InscricaoViewModel>>(inscricao);
            return View(inscricaoViewModel);
        }
    }
}

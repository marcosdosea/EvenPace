using AutoMapper;
using Core;
using Core.Service;
using Core.Service.Dtos;
using EvenPaceWeb.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace EvenPace.Controllers
{
    [Authorize]
    public class InscricaoController : Controller
    {
        private readonly IInscricaoService _inscricaoService;
        private readonly ICorredorService _corredorService;
        private readonly UserManager<UsuarioIdentity> _userManager;
        private readonly IMapper _mapper;
        private readonly IEventosService _eventosService;

        public InscricaoController(
            IInscricaoService inscricaoService,
            ICorredorService corredorService,
            UserManager<UsuarioIdentity> userManager,
            IEventosService eventosService,
            IMapper mapper)
        {
            _inscricaoService = inscricaoService;
            _corredorService = corredorService;
            _userManager = userManager;
            _eventosService = eventosService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            InscricaoViewModel model;
            try
            {
                model = await CarregarTelaInscricaoAsync(id, 0);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }

            if (model.DataEvento < DateTime.Now)
            {
                ViewBag.PopupMessage = "Não é possível se inscrever porque a data desta corrida já expirou.";
            }

            var corredor = await ObterCorredorLogadoAsync();
            if (corredor is not null &&
                await _inscricaoService.PossuiInscricaoAtivaAsync(corredor.Id, model.IdEvento))
            {
                ViewBag.InscricaoExistenteMessage = "Você já está inscrito nesta corrida.";
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int idEvento, int idKit)
        {
            InscricaoViewModel model;
            try
            {
                model = await CarregarTelaInscricaoAsync(idEvento, idKit);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }

            if (model.DataEvento < DateTime.Now)
            {
                ViewBag.PopupMessage = "Não é possível se inscrever porque a data desta corrida já expirou.";
                return View("Index", model);
            }

            return View("Create", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InscricaoViewModel model)
        {
            RemoverValidacoesCamposDeTela(model);

            if (!ModelState.IsValid)
            {
                try
                {
                    await RecarregarTelaInscricaoAsync(model);
                }
                catch (InvalidOperationException ex)
                {
                    return NotFound(ex.Message);
                }

                return View("Create", model);
            }

            var corredor = await ObterCorredorLogadoAsync();
            if (corredor is null)
            {
                TempData["Erro"] = "Faça login para concluir a inscrição.";
                return RedirectToAction("Login", "Corredor");
            }

            if (await _inscricaoService.PossuiInscricaoAtivaAsync(corredor.Id, model.IdEvento))
            {
                ModelState.AddModelError(string.Empty, "Você já possui uma inscrição para este evento.");
                await RecarregarTelaInscricaoAsync(model);
                return View("Create", model);
            }

            return RedirectToAction("AguardarCheckoutInscricao", "Pagamento", new
            {
                idEvento = model.IdEvento,
                idKit = model.IdKit,
                distancia = model.Distancia,
                tamanhoCamisa = model.TamanhoCamisa
            });
        }

        private void RemoverValidacoesCamposDeTela(InscricaoViewModel model)
        {
            ModelState.Remove(nameof(model.DataInscricao));
            ModelState.Remove(nameof(model.IdCorredor));
            ModelState.Remove(nameof(model.NomeEvento));
            ModelState.Remove(nameof(model.ImagemEvento));
            ModelState.Remove(nameof(model.Local));
            ModelState.Remove(nameof(model.DataEvento));
            ModelState.Remove(nameof(model.Descricao));
            ModelState.Remove(nameof(model.InfoRetiradaKit));
            ModelState.Remove(nameof(model.Percursos));
            ModelState.Remove(nameof(model.Kits));
            ModelState.Remove(nameof(model.Inscricao));
            ModelState.Remove(nameof(model.NomeCorredor));
            ModelState.Remove(nameof(model.NomeKit));
            ModelState.Remove(nameof(model.IdAvaliacaoEventoNavigation));
            ModelState.Remove(nameof(model.IdCorredorNavigation));
            ModelState.Remove(nameof(model.IdEventoNavigation));
            ModelState.Remove(nameof(model.IdKitNavigation));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var inscricao = await _inscricaoService.GetAsync(id);
            if (inscricao is null)
                return NotFound("Inscrição não encontrada");

            var corredor = await ObterCorredorLogadoAsync();
            if (corredor is null || inscricao.IdCorredor != corredor.Id)
                return NotFound("Inscrição não encontrada");

            var dados = await _inscricaoService.GetDadosTelaDeleteAsync(id);
            if (!dados.Success)
            {
                if (dados.ErrorType == "EventoExpirado")
                {
                    TempData["Erro"] = "Não é possível cancelar após a data do evento.";
                    return RedirectToAction(nameof(Index), new { id = inscricao.IdEvento });
                }

                return NotFound("Inscrição não encontrada");
            }

            var vm = new InscricaoViewModel
            {
                IdEvento = inscricao.IdEvento,
                NomeEvento = dados.Data!.NomeEvento,
                ImagemEvento = dados.Data.ImagemEvento,
                DataEvento = dados.Data.DataEvento,
                Local = dados.Data.Local,
                NomeKit = dados.Data.NomeKit,
                NomeCorredor = dados.Data.NomeCorredor,
                Inscricao = new InscricaoViewModel
                {
                    Id = (uint)dados.Data.IdInscricao,
                    IdEvento = inscricao.IdEvento,
                    IdKit = inscricao.IdKit ?? 0,
                    Distancia = dados.Data.Distancia,
                    TamanhoCamisa = dados.Data.TamanhoCamisa,
                    DataInscricao = dados.Data.DataInscricao
                }
            };

            return View("Delete", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(InscricaoViewModel model)
        {
            var idEvento = model?.IdEvento ?? model?.Inscricao?.IdEvento ?? 0;
            if (model?.Inscricao is null || model.Inscricao.Id == 0)
            {
                TempData["Erro"] = "Não foi possível identificar a inscrição para cancelamento.";
                return RedirectToAction(nameof(Index), new { id = idEvento });
            }

            var corredor = await ObterCorredorLogadoAsync();
            if (corredor is null)
            {
                TempData["Erro"] = "Faça login para cancelar a inscrição.";
                return RedirectToAction(nameof(Index), new { id = idEvento });
            }

            try
            {
                await _inscricaoService.CancelarAsync((int)model.Inscricao.Id, corredor.Id);
                TempData["Sucesso"] = "Inscrição cancelada com sucesso!";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erro"] = ex.Message;
            }

            return RedirectToAction(nameof(Index), new { id = idEvento });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByEvento(int idEvento)
        {
            var inscricoes = await _inscricaoService.GetAllByEventoAsync(idEvento);

            var inscricaoViewModel = _mapper.Map<List<InscricaoViewModel>>(inscricoes);

            // Busca o evento
            var evento = _eventosService.Get((int)idEvento);

            // Envia informações para a View
            ViewBag.NomeEvento = evento.Nome;
            ViewBag.IdEventoAtual = idEvento;

            return View(inscricaoViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> IndexCorridasInscritas()
        {
            var corredor = await ObterCorredorLogadoAsync();
            if (corredor is null)
            {
                TempData["Erro"] = "Faça login para ver suas corridas.";
                return RedirectToAction("Login", "Corredor");
            }

            ViewBag.IdCorredor = corredor.Id;
            ViewBag.NomeCorredor = corredor.Nome;

            var inscricoes = await _inscricaoService.GetAllByCorredorAsync(corredor.Id);
            var model = _mapper.Map<List<InscricaoViewModel>>(inscricoes);

            return View(model);
        }

        private async Task<InscricaoViewModel> CarregarTelaInscricaoAsync(int idEvento, int idKit)
        {
            var dados = await _inscricaoService.GetDadosTelaInscricaoAsync(idEvento);

            return new InscricaoViewModel
            {
                IdEvento = dados.IdEvento,
                IdKit = idKit,
                NomeEvento = dados.NomeEvento,
                Local = dados.Local,
                DataEvento = dados.DataEvento,
                Descricao = dados.Descricao,
                ImagemEvento = dados.ImagemEvento,
                InfoRetiradaKit = dados.InfoRetiradaKit ?? string.Empty,
                Percursos = dados.Percursos,
                Kits = _mapper.Map<List<KitViewModel>>(dados.Kits.ToList()),
                Inscricao = new InscricaoViewModel
                {
                    IdEvento = dados.IdEvento,
                    IdKit = idKit
                }
            };
        }

        private async Task RecarregarTelaInscricaoAsync(InscricaoViewModel model)
        {
            var tela = await CarregarTelaInscricaoAsync(model.IdEvento, model.IdKit);

            model.NomeEvento = tela.NomeEvento;
            model.Local = tela.Local;
            model.DataEvento = tela.DataEvento;
            model.Descricao = tela.Descricao;
            model.ImagemEvento = tela.ImagemEvento;
            model.InfoRetiradaKit = tela.InfoRetiradaKit;
            model.Percursos = tela.Percursos;
            model.Kits = tela.Kits;
            model.Inscricao = tela.Inscricao;
        }

        private async Task<Corredor?> ObterCorredorLogadoAsync()
        {
            var usuarioIdentity = await _userManager.GetUserAsync(User);
            if (usuarioIdentity?.UserName is null)
                return null;

            var cpf = NormalizarDocumento(usuarioIdentity.UserName);
            if (string.IsNullOrWhiteSpace(cpf))
                return null;

            return await _corredorService.GetByCpfAsync(cpf);
        }

        private static string NormalizarDocumento(string documento)
        {
            var apenasDigitos = documento.Where(char.IsDigit).ToArray();
            return new string(apenasDigitos);
        }
    }
}

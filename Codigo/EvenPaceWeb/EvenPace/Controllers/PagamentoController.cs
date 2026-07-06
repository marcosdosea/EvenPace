using Core;
using Core.Service;
using Core.Service.Dtos;
using EvenPaceWeb.Areas.Identity.Data;
using EvenPaceWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace EvenPace.Controllers
{
    [Authorize]
    public class PagamentoController : Controller
    {
        private readonly IPagamentoService _pagamentoService;
        private readonly IInscricaoService _inscricaoService;
        private readonly ICorredorService _corredorService;
        private readonly IEventosService _eventoService;
        private readonly IKitService _kitService;
        private readonly UserManager<UsuarioIdentity> _userManager;
        private readonly IConfiguration _configuration;

        public PagamentoController(
            IPagamentoService pagamentoService,
            IInscricaoService inscricaoService,
            ICorredorService corredorService,
            IEventosService eventoService,
            IKitService kitService,
            UserManager<UsuarioIdentity> userManager,
            IConfiguration configuration)
        {
            _pagamentoService = pagamentoService;
            _inscricaoService = inscricaoService;
            _corredorService = corredorService;
            _eventoService = eventoService;
            _kitService = kitService;
            _userManager = userManager;
            _configuration = configuration;
        }

        // ── GET /Pagamento/Pagar?idInscricao=X ─────────────────────────────
        [HttpGet]

        public async Task<IActionResult> Pagar(int idInscricao)
        {
            var inscricao = await _inscricaoService.GetAsync(idInscricao);
            if (inscricao is null)
                return NotFound("Inscrição não encontrada.");

            var corredor = await ObterCorredorLogadoAsync();
            if (corredor is null || inscricao.IdCorredor != corredor.Id)
                return NotFound("Inscrição não encontrada.");

            if (string.Equals(inscricao.Status, "Confirmada", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Sucesso"] = "Inscrição já confirmada!";
                return RedirectToAction("IndexUsuario", "Evento");
            }

            if (string.Equals(inscricao.Status, "Cancelada", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Erro"] = "Não é possível pagar uma inscrição cancelada.";
                return RedirectToAction("IndexUsuario", "Evento");
            }

            // Carrega evento e kit diretamente (igual ao InscricaoController)
            var evento = _eventoService.Get(inscricao.IdEvento);
            Kit? kit = inscricao.IdKit.HasValue ? _kitService.Get(inscricao.IdKit.Value) : null;

            var vm = new PagamentoViewModel
            {
                IdInscricao = idInscricao,
                NomeEvento = evento?.Nome ?? string.Empty,
                NomeCorredor = corredor.Nome,
                NomeKit = kit?.Nome ?? "Sem kit",
                Distancia = inscricao.Distancia,
                TamanhoCamisa = inscricao.TamanhoCamisa,
                ValorKit = kit?.Valor ?? 0m,
                MercadoPagoPublicKey = _configuration["MercadoPago:PublicKey"] ?? string.Empty
            };

            return View(vm);
        }

        // ── POST /Pagamento/Processar  (chamado pelo fetch do Brick JS) ─────
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Processar([FromBody] ProcessarPagamentoDto dto)
        {
            if (dto is null || dto.IdInscricao <= 0)
                return BadRequest(new { success = false, mensagemErro = "Dados inválidos." });

            var inscricao = await _inscricaoService.GetAsync(dto.IdInscricao);
            var corredor = await ObterCorredorLogadoAsync();

            if (inscricao is null || corredor is null || inscricao.IdCorredor != corredor.Id)
                return Unauthorized(new { success = false, mensagemErro = "Acesso negado." });

            try
            {
                var resultado = await _pagamentoService.ProcessarAsync(dto);
                return Ok(new
                {
                    success = resultado.Success,
                    status = resultado.Status,
                    idPagamento = resultado.IdPagamento,
                    idTransacaoMP = resultado.IdTransacaoMP
                });
            }
            catch (InvalidOperationException ex)
            {
                return Ok(new { success = false, mensagemErro = ex.Message });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    mensagemErro = ex.InnerException?.Message ?? ex.ToString()
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Cartao(int idInscricao)
        {
            var inscricao = await _inscricaoService.GetAsync(idInscricao);
            if (inscricao == null)
                return NotFound();

            var corredor = await ObterCorredorLogadoAsync();
            if (corredor == null || inscricao.IdCorredor != corredor.Id)
                return Unauthorized();

            var evento = _eventoService.Get(inscricao.IdEvento);
            Kit? kit = inscricao.IdKit.HasValue
                ? _kitService.Get(inscricao.IdKit.Value)
                : null;

            var vm = new PagamentoViewModel
            {
                IdInscricao = idInscricao,
                NomeEvento = evento?.Nome ?? "",
                NomeCorredor = corredor.Nome,
                NomeKit = kit?.Nome ?? "Sem kit",
                Distancia = inscricao.Distancia,
                TamanhoCamisa = inscricao.TamanhoCamisa,
                ValorKit = kit?.Valor ?? 0,
                MercadoPagoPublicKey = _configuration["MercadoPago:PublicKey"] ?? ""
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Pix(int idInscricao)
        {
            var inscricao = await _inscricaoService.GetAsync(idInscricao);
            if (inscricao == null)
                return NotFound();

            var corredor = await ObterCorredorLogadoAsync();
            if (corredor == null || inscricao.IdCorredor != corredor.Id)
                return Unauthorized();

            var evento = _eventoService.Get(inscricao.IdEvento);
            Kit? kit = inscricao.IdKit.HasValue
                ? _kitService.Get(inscricao.IdKit.Value)
                : null;

            var vm = new PagamentoViewModel
            {
                IdInscricao = idInscricao,
                NomeEvento = evento?.Nome ?? "",
                NomeCorredor = corredor.Nome,
                NomeKit = kit?.Nome ?? "Sem kit",
                Distancia = inscricao.Distancia,
                TamanhoCamisa = inscricao.TamanhoCamisa,
                ValorKit = kit?.Valor ?? 0,
                MercadoPagoPublicKey = _configuration["MercadoPago:PublicKey"] ?? ""
            };

            return View(vm);
        }


        // ── GET /Pagamento/Resultado?status=approved&idTransacaoMP=xxx ──────
        [HttpGet]
        public IActionResult Resultado(string status, string? idTransacaoMP)
        {
            var vm = new PagamentoViewModel
            {
                StatusPagamento = status,
                IdTransacaoMP = idTransacaoMP
            };
            return View(vm);
        }

        // ── Helper ──────────────────────────────────────────────────────────
        private async Task<Corredor?> ObterCorredorLogadoAsync()
        {
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario?.UserName is null) return null;

            // Igual ao InscricaoController do projeto (usa GetByCpf síncrono)
            return _corredorService.GetByCpf(usuario.UserName);
        }
    }
}
using Core;
using Core.Service;
using Core.Service.Dtos;
using EvenPaceWeb.Areas.Identity.Data;
using EvenPaceWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace EvenPace.Controllers
{
    [Authorize]
    public class PagamentoController : Controller
    {
        private const int MinutosValidadeCheckoutSessao = 30;
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

        [HttpGet]
        public async Task<IActionResult> CheckoutPro(int idInscricao)
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

            var retorno = CriarRetornoMercadoPago();

            try
            {
                var checkoutUrl = await _pagamentoService.CriarCheckoutProAsync(
                    idInscricao,
                    retorno.SuccessUrl,
                    retorno.PendingUrl,
                    retorno.FailureUrl,
                    retorno.HabilitarRetornoAutomatico,
                    retorno.NotificationUrl);

                return Redirect(checkoutUrl);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erro"] = ex.Message;
                return RedirectToAction("Pagar", new { idInscricao });
            }
        }

        [HttpGet]
        public async Task<IActionResult> AguardarCheckoutInscricao(
            int idEvento,
            int idKit,
            string distancia,
            string tamanhoCamisa)
        {
            var corredor = await ObterCorredorLogadoAsync();
            if (corredor is null)
            {
                TempData["Erro"] = "Faça login para concluir a inscrição.";
                return RedirectToAction("Login", "Corredor");
            }

            var retorno = CriarRetornoMercadoPago();

            var chaveCheckout = MontarChaveCheckoutSessao(
                corredor.Id,
                idEvento,
                idKit,
                distancia,
                tamanhoCamisa);

            CheckoutProDto? checkout = ObterCheckoutDaSessao(chaveCheckout);
            try
            {
                if (checkout is null)
                {
                    checkout = await _pagamentoService.CriarCheckoutProInscricaoAsync(
                        corredor.Id,
                        idEvento,
                        idKit,
                        distancia,
                        tamanhoCamisa,
                        retorno.SuccessUrl,
                        retorno.PendingUrl,
                        retorno.FailureUrl,
                        retorno.HabilitarRetornoAutomatico,
                        retorno.NotificationUrl);

                    SalvarCheckoutNaSessao(chaveCheckout, checkout);
                }
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erro"] = ex.Message;
                return RedirectToAction("Create", "Inscricao", new { idEvento, idKit });
            }

            var statusUrl = Url.Action(nameof(StatusCheckout), "Pagamento", new
            {
                idEvento,
                externalReference = checkout.ExternalReference
            }) ?? throw new InvalidOperationException("Não foi possível montar a URL de status.");

            var resultadoUrl = Url.Action(nameof(Resultado), "Pagamento", new { status = "approved" })
                ?? throw new InvalidOperationException("Não foi possível montar a URL de resultado.");

            return View(new CheckoutInscricaoViewModel
            {
                IdEvento = idEvento,
                IdKit = idKit,
                Distancia = distancia,
                TamanhoCamisa = tamanhoCamisa,
                CheckoutUrl = checkout.CheckoutUrl,
                ExternalReference = checkout.ExternalReference,
                StatusUrl = statusUrl,
                ResultadoUrl = resultadoUrl
            });
        }

        [HttpGet]
        public async Task<IActionResult> CheckoutInscricao(
            int idEvento,
            int idKit,
            string distancia,
            string tamanhoCamisa)
        {
            var corredor = await ObterCorredorLogadoAsync();
            if (corredor is null)
            {
                TempData["Erro"] = "Faça login para concluir a inscrição.";
                return RedirectToAction("Login", "Corredor");
            }

            var retorno = CriarRetornoMercadoPago();

            try
            {
                var checkout = await _pagamentoService.CriarCheckoutProInscricaoAsync(
                    corredor.Id,
                    idEvento,
                    idKit,
                    distancia,
                    tamanhoCamisa,
                    retorno.SuccessUrl,
                    retorno.PendingUrl,
                    retorno.FailureUrl,
                    retorno.HabilitarRetornoAutomatico,
                    retorno.NotificationUrl);

                return Redirect(checkout.CheckoutUrl);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erro"] = ex.Message;
                return RedirectToAction("Create", "Inscricao", new { idEvento, idKit });
            }
        }

        [HttpGet]
        public async Task<IActionResult> StatusInscricao(int idEvento)
        {
            var corredor = await ObterCorredorLogadoAsync();
            if (corredor is null)
                return Unauthorized(new { confirmada = false });

            var confirmada = await _inscricaoService.PossuiInscricaoAtivaAsync(corredor.Id, idEvento);
            return Ok(new { confirmada });
        }

        [HttpGet]
        public async Task<IActionResult> StatusCheckout(string? externalReference, int idEvento)
        {
            var corredor = await ObterCorredorLogadoAsync();
            if (corredor is null)
                return Unauthorized(new { confirmada = false });

            if (!ReferenciaPertenceAoCorredor(externalReference, corredor.Id, idEvento))
                return BadRequest(new { confirmada = false });

            try
            {
                var resultado = await _pagamentoService.VerificarCheckoutProPorReferenciaAsync(externalReference!);
                var confirmada =
                    string.Equals(resultado.Status, "approved", StringComparison.OrdinalIgnoreCase) ||
                    await _inscricaoService.PossuiInscricaoAtivaAsync(corredor.Id, idEvento);

                if (confirmada)
                    RemoverCheckoutsDaSessao(corredor.Id, idEvento);

                return Ok(new
                {
                    confirmada,
                    status = resultado.Status
                });
            }
            catch (InvalidOperationException ex)
            {
                return Ok(new
                {
                    confirmada = false,
                    mensagemErro = ex.Message
                });
            }
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
        public async Task<IActionResult> Resultado(
            string? status,
            string? collection_status,
            string? payment_id,
            string? external_reference,
            string? idTransacaoMP)
        {
            var statusPagamento = status ?? collection_status ?? "unknown";
            var idTransacao = idTransacaoMP ?? payment_id;

            if (!string.IsNullOrWhiteSpace(payment_id))
            {
                try
                {
                    var resultado = await _pagamentoService.RegistrarRetornoCheckoutProAsync(
                        payment_id,
                        external_reference,
                        statusPagamento);

                    statusPagamento = resultado.Status ?? statusPagamento;
                    idTransacao = resultado.IdTransacaoMP ?? idTransacao;
                }
                catch (InvalidOperationException ex)
                {
                    TempData["Erro"] = ex.Message;
                }
            }

            var vm = new PagamentoViewModel
            {
                StatusPagamento = statusPagamento,
                IdTransacaoMP = idTransacao
            };
            return View(vm);
        }

        [AllowAnonymous]
        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> Webhook()
        {
            var paymentId = ObterPaymentIdDoWebhook();
            if (string.IsNullOrWhiteSpace(paymentId))
                return Ok();

            try
            {
                await _pagamentoService.RegistrarRetornoCheckoutProAsync(paymentId, null, null);
            }
            catch
            {
                return Ok();
            }

            return Ok();
        }

        // ── Helper ──────────────────────────────────────────────────────────
        private async Task<Corredor?> ObterCorredorLogadoAsync()
        {
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario?.UserName is null) return null;

            // Igual ao InscricaoController do projeto (usa GetByCpf síncrono)
            return _corredorService.GetByCpf(usuario.UserName);
        }

        private RetornoMercadoPago CriarRetornoMercadoPago()
        {
            var resultadoPath = Url.Action(nameof(Resultado), "Pagamento")
                ?? throw new InvalidOperationException("Não foi possível montar a URL de retorno.");

            var configuredBaseUrl = _configuration["MercadoPago:ReturnBaseUrl"]?.Trim().TrimEnd('/');
            var baseUrl = !string.IsNullOrWhiteSpace(configuredBaseUrl)
                ? configuredBaseUrl
                : $"{Request.Scheme}://{Request.Host}";

            var successUrl = $"{baseUrl}{resultadoPath}";
            var habilitarRetornoAutomatico =
                Uri.TryCreate(baseUrl, UriKind.Absolute, out var uri) &&
                uri.Scheme == Uri.UriSchemeHttps &&
                !uri.IsLoopback;

            var notificationUrl = habilitarRetornoAutomatico
                ? $"{baseUrl}{Url.Action(nameof(Webhook), "Pagamento")}"
                : null;

            return new RetornoMercadoPago(
                successUrl,
                successUrl,
                successUrl,
                habilitarRetornoAutomatico,
                notificationUrl);
        }

        private string? ObterPaymentIdDoWebhook()
        {
            var query = Request.Query;
            var paymentId =
                query["data.id"].FirstOrDefault() ??
                query["id"].FirstOrDefault() ??
                query["payment_id"].FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(paymentId))
                return paymentId;

            if (!Request.HasJsonContentType())
                return null;

            try
            {
                using var document = JsonDocument.Parse(Request.Body);
                var root = document.RootElement;

                if (root.TryGetProperty("data", out var data) &&
                    data.TryGetProperty("id", out var dataId))
                {
                    return dataId.GetString();
                }

                if (root.TryGetProperty("id", out var id))
                    return id.GetString();
            }
            catch
            {
                return null;
            }

            return null;
        }

        private static bool ReferenciaPertenceAoCorredor(string? externalReference, int idCorredor, int idEvento)
        {
            if (string.IsNullOrWhiteSpace(externalReference))
                return false;

            var partes = externalReference.Split('|');

            return partes.Length == 6 &&
                string.Equals(partes[0], "preinscricao", StringComparison.OrdinalIgnoreCase) &&
                int.TryParse(partes[1], out var idCorredorReferencia) &&
                int.TryParse(partes[2], out var idEventoReferencia) &&
                idCorredorReferencia == idCorredor &&
                idEventoReferencia == idEvento;
        }

        private CheckoutProDto? ObterCheckoutDaSessao(string chave)
        {
            var json = HttpContext.Session.GetString(chave);
            if (string.IsNullOrWhiteSpace(json))
                return null;

            try
            {
                var checkout = JsonSerializer.Deserialize<CheckoutSessaoDto>(json);
                if (checkout is null ||
                    string.IsNullOrWhiteSpace(checkout.CheckoutUrl) ||
                    string.IsNullOrWhiteSpace(checkout.ExternalReference) ||
                    checkout.CriadoEmUtc.AddMinutes(MinutosValidadeCheckoutSessao) < DateTimeOffset.UtcNow)
                {
                    HttpContext.Session.Remove(chave);
                    return null;
                }

                return new CheckoutProDto
                {
                    CheckoutUrl = checkout.CheckoutUrl,
                    ExternalReference = checkout.ExternalReference
                };
            }
            catch
            {
                HttpContext.Session.Remove(chave);
                return null;
            }
        }

        private void SalvarCheckoutNaSessao(string chave, CheckoutProDto checkout)
        {
            var dados = new CheckoutSessaoDto(
                checkout.CheckoutUrl,
                checkout.ExternalReference,
                DateTimeOffset.UtcNow);

            HttpContext.Session.SetString(chave, JsonSerializer.Serialize(dados));
        }

        private void RemoverCheckoutsDaSessao(int idCorredor, int idEvento)
        {
            var prefixo = $"checkout-inscricao:{idCorredor}:{idEvento}:";
            var chaves = HttpContext.Session.Keys
                .Where(chave => chave.StartsWith(prefixo, StringComparison.Ordinal))
                .ToList();

            foreach (var chave in chaves)
                HttpContext.Session.Remove(chave);
        }

        private static string MontarChaveCheckoutSessao(
            int idCorredor,
            int idEvento,
            int idKit,
            string distancia,
            string tamanhoCamisa)
        {
            var dados = $"{idCorredor}|{idEvento}|{idKit}|{distancia}|{tamanhoCamisa}";
            var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(dados)));

            return $"checkout-inscricao:{idCorredor}:{idEvento}:{hash}";
        }

        private sealed record RetornoMercadoPago(
            string SuccessUrl,
            string PendingUrl,
            string FailureUrl,
            bool HabilitarRetornoAutomatico,
            string? NotificationUrl);

        private sealed record CheckoutSessaoDto(
            string CheckoutUrl,
            string ExternalReference,
            DateTimeOffset CriadoEmUtc);
    }
}

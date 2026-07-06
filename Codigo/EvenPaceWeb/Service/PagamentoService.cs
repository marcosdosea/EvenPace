using Core;
using Core.Service;
using Core.Service.Dtos;
using MercadoPago.Client.Common;
using MercadoPago.Client.Payment;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;


namespace Service
{
    public class PagamentoService : IPagamentoService
    {
        private readonly EvenPaceContext _context;
        private readonly string _accessToken;

        public PagamentoService(EvenPaceContext context, IConfiguration configuration)
        {
            _context = context;
            _accessToken = configuration["MercadoPago:AccessToken"]
                ?? throw new InvalidOperationException(
                    "Chave 'MercadoPago:AccessToken' não encontrada no appsettings.");
        }

        public async Task<ResultadoPagamentoDto> ProcessarAsync(ProcessarPagamentoDto dto)
        {
            // ── 1. Valida inscrição ──────────────────────────────────────────
            var inscricao = await _context.Inscricao
                .Include(i => i.IdKitNavigation)
                .Include(i => i.IdEventoNavigation)
                .Include(i => i.IdCorredorNavigation)
                .FirstOrDefaultAsync(i => i.Id == dto.IdInscricao);

            if (inscricao is null)
                throw new InvalidOperationException("Inscrição não encontrada.");

            if (string.Equals(inscricao.Status, "Confirmada", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Esta inscrição já foi paga.");

            if (string.Equals(inscricao.Status, "Cancelada", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Não é possível pagar uma inscrição cancelada.");

            // ── 2. Valor real do kit (NUNCA confia no valor enviado pelo cliente) ──
            var valorKit = inscricao.IdKitNavigation?.Valor
                ?? throw new InvalidOperationException("Kit da inscrição não encontrado.");

            // ── 3. Configura o SDK ───────────────────────────────────────────
            MercadoPagoConfig.AccessToken = _accessToken;

            // ── 4. Monta e envia o pagamento para o MP ────────────────────────
            var paymentRequest = new PaymentCreateRequest
            {
                TransactionAmount = valorKit,
                Token = dto.Token,
                Description = $"EvenPace – {inscricao.IdEventoNavigation?.Nome} " +
                                    $"| Kit: {inscricao.IdKitNavigation?.Nome} " +
                                    $"| Inscrição #{inscricao.Id}",
                Installments = dto.Installments > 0 ? dto.Installments : 1,
                PaymentMethodId = dto.PaymentMethodId,
                Payer = new PaymentPayerRequest
                {
                    Email = dto.Payer?.Email,
                    Identification = dto.Payer?.Identification is not null
                        ? new IdentificationRequest
                        {
                            Type = dto.Payer.Identification.Type,
                            Number = dto.Payer.Identification.Number
                        }
                        : null
                }
            };

            MercadoPago.Resource.Payment.Payment mpPayment;
            try
            {
                var client = new PaymentClient();
                mpPayment = await client.CreateAsync(paymentRequest);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Erro na comunicação com o Mercado Pago: {ex.Message}");
            }

            // ── 5. Salva registro de Pagamento no banco ───────────────────────
            var pagamento = new Pagamento
            {
                IdInscricao = dto.IdInscricao,
                ValorPago = valorKit,
                Status = mpPayment.Status ?? "unknown",
                FormaPagamento = dto.PaymentMethodId,
                IdTransacaoMP = mpPayment.Id?.ToString(),
                DataPagamento = DateTime.Now,
                Parcelas = dto.Installments > 0 ? dto.Installments : 1
            };
            await _context.Pagamentos.AddAsync(pagamento);

            // ── 6. Atualiza Status da Inscrição ───────────────────────────────
            inscricao.Status = mpPayment.Status switch
            {
                "approved" => "Confirmada",
                "in_process" => "Pendente",
                "pending" => "Pendente",
                _ => "Pendente"   // rejected → fica Pendente para nova tentativa
            };

            await _context.SaveChangesAsync();

            return new ResultadoPagamentoDto
            {
                Success = true,
                Status = mpPayment.Status,
                IdPagamento = pagamento.Id,
                IdTransacaoMP = mpPayment.Id?.ToString()
            };
        }

        public async Task<string> CriarCheckoutProAsync(
            int idInscricao,
            string successUrl,
            string pendingUrl,
            string failureUrl,
            bool habilitarRetornoAutomatico,
            string? notificationUrl)
        {
            var inscricao = await _context.Inscricao
                .Include(i => i.IdKitNavigation)
                .Include(i => i.IdEventoNavigation)
                .Include(i => i.IdCorredorNavigation)
                .FirstOrDefaultAsync(i => i.Id == idInscricao);

            if (inscricao is null)
                throw new InvalidOperationException("Inscrição não encontrada.");

            if (string.Equals(inscricao.Status, "Confirmada", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Esta inscrição já foi paga.");

            if (string.Equals(inscricao.Status, "Cancelada", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Não é possível pagar uma inscrição cancelada.");

            var kit = inscricao.IdKitNavigation
                ?? throw new InvalidOperationException("Kit da inscrição não encontrado.");

            MercadoPagoConfig.AccessToken = _accessToken;

            var preferenceRequest = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
                {
                    new()
                    {
                        Id = inscricao.Id.ToString(),
                        Title = $"Inscrição - {inscricao.IdEventoNavigation?.Nome}",
                        Description = $"Kit: {kit.Nome} | Distância: {inscricao.Distancia}",
                        Quantity = 1,
                        UnitPrice = kit.Valor,
                        CurrencyId = "BRL"
                    }
                },
                Payer = new PreferencePayerRequest
                {
                    Name = inscricao.IdCorredorNavigation?.Nome
                },
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = successUrl,
                    Pending = pendingUrl,
                    Failure = failureUrl
                },
                ExternalReference = inscricao.Id.ToString(),
                StatementDescriptor = "EVENPACE"
            };

            if (habilitarRetornoAutomatico)
                preferenceRequest.AutoReturn = "approved";

            if (!string.IsNullOrWhiteSpace(notificationUrl))
                preferenceRequest.NotificationUrl = notificationUrl;

            try
            {
                var client = new PreferenceClient();
                var preference = await client.CreateAsync(preferenceRequest);

                return preference.SandboxInitPoint
                    ?? preference.InitPoint
                    ?? throw new InvalidOperationException("Mercado Pago não retornou uma URL de checkout.");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Erro ao criar checkout no Mercado Pago: {ex.Message}");
            }
        }

        public async Task<CheckoutProDto> CriarCheckoutProInscricaoAsync(
            int idCorredor,
            int idEvento,
            int idKit,
            string distancia,
            string tamanhoCamisa,
            string successUrl,
            string pendingUrl,
            string failureUrl,
            bool habilitarRetornoAutomatico,
            string? notificationUrl)
        {
            var corredor = await _context.Corredors
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == idCorredor);

            if (corredor is null)
                throw new InvalidOperationException("Corredor não encontrado.");

            var evento = await _context.Eventos
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == idEvento);

            if (evento is null)
                throw new InvalidOperationException("Evento não encontrado.");

            if (evento.Data < DateTime.Now)
                throw new InvalidOperationException("Não é possível se inscrever em uma corrida expirada.");

            var kit = await _context.Kits
                .AsNoTracking()
                .FirstOrDefaultAsync(k => k.Id == idKit && k.IdEvento == idEvento);

            if (kit is null)
                throw new InvalidOperationException("Kit não encontrado para esta corrida.");

            if (string.IsNullOrWhiteSpace(distancia))
                throw new InvalidOperationException("Selecione uma distância.");

            if (string.IsNullOrWhiteSpace(tamanhoCamisa))
                throw new InvalidOperationException("Selecione o tamanho da camisa.");

            var possuiInscricaoAtiva = await _context.Inscricao
                .AsNoTracking()
                .AnyAsync(i =>
                    i.IdCorredor == idCorredor &&
                    i.IdEvento == idEvento &&
                    i.Status != "Cancelada");

            if (possuiInscricaoAtiva)
                throw new InvalidOperationException("Você já possui uma inscrição para este evento.");

            MercadoPagoConfig.AccessToken = _accessToken;

            var externalReference = MontarReferenciaPreInscricao(
                idCorredor,
                idEvento,
                idKit,
                distancia,
                tamanhoCamisa);

            var preferenceRequest = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
                {
                    new()
                    {
                        Id = idEvento.ToString(),
                        Title = $"Inscrição - {evento.Nome}",
                        Description = $"Kit: {kit.Nome} | Distância: {distancia}",
                        Quantity = 1,
                        UnitPrice = kit.Valor,
                        CurrencyId = "BRL"
                    }
                },
                Payer = new PreferencePayerRequest
                {
                    Name = corredor.Nome
                },
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = successUrl,
                    Pending = pendingUrl,
                    Failure = failureUrl
                },
                ExternalReference = externalReference,
                StatementDescriptor = "EVENPACE"
            };

            if (habilitarRetornoAutomatico)
                preferenceRequest.AutoReturn = "approved";

            if (!string.IsNullOrWhiteSpace(notificationUrl))
                preferenceRequest.NotificationUrl = notificationUrl;

            try
            {
                var client = new PreferenceClient();
                var preference = await client.CreateAsync(preferenceRequest);

                var checkoutUrl = preference.SandboxInitPoint
                    ?? preference.InitPoint
                    ?? throw new InvalidOperationException("Mercado Pago não retornou uma URL de checkout.");

                return new CheckoutProDto
                {
                    CheckoutUrl = checkoutUrl,
                    ExternalReference = externalReference
                };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Erro ao criar checkout no Mercado Pago: {ex.Message}");
            }
        }

        public async Task<ResultadoPagamentoDto> VerificarCheckoutProPorReferenciaAsync(string externalReference)
        {
            if (string.IsNullOrWhiteSpace(externalReference))
                throw new InvalidOperationException("Referência do checkout inválida.");

            try
            {
                var pagamento = await BuscarPagamentoPorReferenciaAsync(externalReference);

                if (pagamento is null)
                {
                    return new ResultadoPagamentoDto
                    {
                        Success = true,
                        Status = "pending"
                    };
                }

                return await RegistrarRetornoCheckoutProAsync(
                    pagamento.IdTransacaoMP,
                    externalReference,
                    pagamento.Status);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Erro ao consultar checkout no Mercado Pago: {ex.Message}");
            }
        }

        public async Task<ResultadoPagamentoDto> RegistrarRetornoCheckoutProAsync(
            string paymentId,
            string? externalReference,
            string? statusRetorno)
        {
            if (string.IsNullOrWhiteSpace(paymentId))
                throw new InvalidOperationException("ID do pagamento inválido.");

            var mpPayment = await ObterPagamentoPorIdAsync(paymentId);

            var referencia = !string.IsNullOrWhiteSpace(mpPayment.ExternalReference)
                ? mpPayment.ExternalReference
                : externalReference;
            var status = mpPayment.Status ?? statusRetorno ?? "unknown";
            var idTransacaoMP = mpPayment.IdTransacaoMP;

            if (EhReferenciaPreInscricao(referencia))
            {
                return await RegistrarPreInscricaoAprovadaAsync(
                    referencia!,
                    status,
                    idTransacaoMP,
                    mpPayment.ValorPago,
                    mpPayment.FormaPagamento,
                    mpPayment.Parcelas);
            }

            var idInscricao = ObterIdInscricao(mpPayment.ExternalReference, externalReference);
            var inscricao = await _context.Inscricao
                .Include(i => i.IdKitNavigation)
                .FirstOrDefaultAsync(i => i.Id == idInscricao);

            if (inscricao is null)
                throw new InvalidOperationException("Inscrição não encontrada para o pagamento.");

            var pagamento = await _context.Pagamentos
                .FirstOrDefaultAsync(p => p.IdTransacaoMP == idTransacaoMP);

            if (pagamento is null)
            {
                pagamento = new Pagamento
                {
                    IdInscricao = inscricao.Id,
                    ValorPago = mpPayment.ValorPago ?? inscricao.IdKitNavigation?.Valor ?? 0m,
                    Status = status,
                    FormaPagamento = mpPayment.FormaPagamento ?? "checkout_pro",
                    IdTransacaoMP = idTransacaoMP,
                    DataPagamento = DateTime.Now,
                    Parcelas = mpPayment.Parcelas ?? 1
                };

                await _context.Pagamentos.AddAsync(pagamento);
            }
            else
            {
                pagamento.Status = status;
                pagamento.FormaPagamento = mpPayment.FormaPagamento ?? pagamento.FormaPagamento;
                pagamento.ValorPago = mpPayment.ValorPago ?? pagamento.ValorPago;
                pagamento.Parcelas = mpPayment.Parcelas ?? pagamento.Parcelas;
                pagamento.DataPagamento = DateTime.Now;
            }

            inscricao.Status = status switch
            {
                "approved" => "Confirmada",
                "in_process" => "Pendente",
                "pending" => "Pendente",
                _ => "Pendente"
            };

            await _context.SaveChangesAsync();

            return new ResultadoPagamentoDto
            {
                Success = true,
                Status = status,
                IdPagamento = pagamento.Id,
                IdTransacaoMP = idTransacaoMP
            };
        }

        public async Task<decimal> ObterValorKitAsync(int idInscricao)
        {
            var kit = await _context.Inscricao
                .AsNoTracking()
                .Where(i => i.Id == idInscricao)
                .Select(i => i.IdKitNavigation)
                .FirstOrDefaultAsync();

            return kit?.Valor ?? 0m;
        }

        private async Task<PagamentoMercadoPagoResumo?> BuscarPagamentoPorReferenciaAsync(string externalReference)
        {
            var url = "https://api.mercadopago.com/v1/payments/search" +
                $"?external_reference={Uri.EscapeDataString(externalReference)}" +
                "&sort=date_created&criteria=desc";

            using var httpClient = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            using var response = await httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(
                    $"Mercado Pago retornou erro ao consultar checkout: {(int)response.StatusCode} {json}");
            }

            using var document = JsonDocument.Parse(json);

            if (!document.RootElement.TryGetProperty("results", out var results) ||
                results.ValueKind != JsonValueKind.Array)
            {
                return null;
            }

            PagamentoMercadoPagoResumo? primeiro = null;

            foreach (var item in results.EnumerateArray())
            {
                var id = ObterString(item, "id");
                if (string.IsNullOrWhiteSpace(id))
                    continue;

                var status = ObterString(item, "status") ?? "unknown";
                var pagamento = new PagamentoMercadoPagoResumo(id, status);

                primeiro ??= pagamento;

                if (string.Equals(status, "approved", StringComparison.OrdinalIgnoreCase))
                    return pagamento;
            }

            return primeiro;
        }

        private async Task<PagamentoMercadoPagoDetalhe> ObterPagamentoPorIdAsync(string paymentId)
        {
            var url = $"https://api.mercadopago.com/v1/payments/{Uri.EscapeDataString(paymentId)}";

            using var httpClient = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            using var response = await httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(
                    $"Mercado Pago retornou erro ao consultar pagamento: {(int)response.StatusCode} {json}");
            }

            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;
            var idTransacaoMP = ObterString(root, "id");

            if (string.IsNullOrWhiteSpace(idTransacaoMP))
                throw new InvalidOperationException("Mercado Pago não retornou o ID do pagamento.");

            return new PagamentoMercadoPagoDetalhe(
                idTransacaoMP,
                ObterString(root, "external_reference"),
                ObterString(root, "status"),
                ObterDecimal(root, "transaction_amount"),
                ObterString(root, "payment_method_id"),
                ObterInt(root, "installments"));
        }

        private static string? ObterString(JsonElement elemento, string propriedade)
        {
            if (!elemento.TryGetProperty(propriedade, out var valor))
                return null;

            return valor.ValueKind switch
            {
                JsonValueKind.String => valor.GetString(),
                JsonValueKind.Number => valor.GetRawText(),
                _ => null
            };
        }

        private static decimal? ObterDecimal(JsonElement elemento, string propriedade)
        {
            if (!elemento.TryGetProperty(propriedade, out var valor) ||
                valor.ValueKind != JsonValueKind.Number)
            {
                return null;
            }

            return valor.TryGetDecimal(out var resultado) ? resultado : null;
        }

        private static int? ObterInt(JsonElement elemento, string propriedade)
        {
            if (!elemento.TryGetProperty(propriedade, out var valor) ||
                valor.ValueKind != JsonValueKind.Number)
            {
                return null;
            }

            return valor.TryGetInt32(out var resultado) ? resultado : null;
        }

        private async Task<ResultadoPagamentoDto> RegistrarPreInscricaoAprovadaAsync(
            string externalReference,
            string status,
            string idTransacaoMP,
            decimal? valorPago,
            string? formaPagamento,
            int? parcelas)
        {
            var dados = ObterDadosPreInscricao(externalReference);

            var pagamentoExistente = await _context.Pagamentos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.IdTransacaoMP == idTransacaoMP);

            if (pagamentoExistente is not null)
            {
                return new ResultadoPagamentoDto
                {
                    Success = true,
                    Status = status,
                    IdPagamento = pagamentoExistente.Id,
                    IdTransacaoMP = idTransacaoMP
                };
            }

            if (!string.Equals(status, "approved", StringComparison.OrdinalIgnoreCase))
            {
                return new ResultadoPagamentoDto
                {
                    Success = true,
                    Status = status,
                    IdPagamento = 0,
                    IdTransacaoMP = idTransacaoMP
                };
            }

            var possuiInscricaoAtiva = await _context.Inscricao
                .AsNoTracking()
                .AnyAsync(i =>
                    i.IdCorredor == dados.IdCorredor &&
                    i.IdEvento == dados.IdEvento &&
                    i.Status != "Cancelada");

            if (possuiInscricaoAtiva)
                throw new InvalidOperationException("Você já possui uma inscrição para este evento.");

            var kit = await _context.Kits
                .AsNoTracking()
                .FirstOrDefaultAsync(k => k.Id == dados.IdKit && k.IdEvento == dados.IdEvento);

            if (kit is null)
                throw new InvalidOperationException("Kit não encontrado para esta corrida.");

            var inscricao = new Inscricao
            {
                IdEvento = dados.IdEvento,
                IdCorredor = dados.IdCorredor,
                IdKit = dados.IdKit,
                DataInscricao = DateTime.Now,
                Status = "Confirmada",
                Distancia = dados.Distancia,
                TamanhoCamisa = dados.TamanhoCamisa,
                StatusRetiradaKit = false,
                Tempo = null,
                Posicao = null,
                IdAvaliacaoEvento = null
            };

            await _context.Inscricao.AddAsync(inscricao);
            await _context.SaveChangesAsync();

            var pagamento = new Pagamento
            {
                IdInscricao = inscricao.Id,
                ValorPago = valorPago ?? kit.Valor,
                Status = status,
                FormaPagamento = formaPagamento ?? "checkout_pro",
                IdTransacaoMP = idTransacaoMP,
                DataPagamento = DateTime.Now,
                Parcelas = parcelas ?? 1
            };

            await _context.Pagamentos.AddAsync(pagamento);
            await _context.SaveChangesAsync();

            return new ResultadoPagamentoDto
            {
                Success = true,
                Status = status,
                IdPagamento = pagamento.Id,
                IdTransacaoMP = idTransacaoMP
            };
        }

        private static int ObterIdInscricao(string? referenciaMercadoPago, string? referenciaRetorno)
        {
            var referencia = !string.IsNullOrWhiteSpace(referenciaMercadoPago)
                ? referenciaMercadoPago
                : referenciaRetorno;

            if (!int.TryParse(referencia, out var idInscricao) || idInscricao <= 0)
                throw new InvalidOperationException("Referência da inscrição inválida no retorno do Mercado Pago.");

            return idInscricao;
        }

        private static string MontarReferenciaPreInscricao(
            int idCorredor,
            int idEvento,
            int idKit,
            string distancia,
            string tamanhoCamisa)
        {
            return string.Join("|",
                "preinscricao",
                idCorredor,
                idEvento,
                idKit,
                Uri.EscapeDataString(distancia),
                Uri.EscapeDataString(tamanhoCamisa));
        }

        private static bool EhReferenciaPreInscricao(string? externalReference)
        {
            return externalReference?.StartsWith("preinscricao|", StringComparison.OrdinalIgnoreCase) == true;
        }

        private static DadosPreInscricao ObterDadosPreInscricao(string externalReference)
        {
            var partes = externalReference.Split('|');
            if (partes.Length != 6 ||
                !int.TryParse(partes[1], out var idCorredor) ||
                !int.TryParse(partes[2], out var idEvento) ||
                !int.TryParse(partes[3], out var idKit))
            {
                throw new InvalidOperationException("Referência da inscrição inválida no retorno do Mercado Pago.");
            }

            return new DadosPreInscricao(
                idCorredor,
                idEvento,
                idKit,
                Uri.UnescapeDataString(partes[4]),
                Uri.UnescapeDataString(partes[5]));
        }

        private sealed record DadosPreInscricao(
            int IdCorredor,
            int IdEvento,
            int IdKit,
            string Distancia,
            string TamanhoCamisa);

        private sealed record PagamentoMercadoPagoResumo(
            string IdTransacaoMP,
            string Status);

        private sealed record PagamentoMercadoPagoDetalhe(
            string IdTransacaoMP,
            string? ExternalReference,
            string? Status,
            decimal? ValorPago,
            string? FormaPagamento,
            int? Parcelas);
    }
}

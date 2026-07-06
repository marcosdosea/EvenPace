using Core;
using Core.Service;
using Core.Service.Dtos;
using MercadoPago.Client.Common;
using MercadoPago.Client.Payment;
using MercadoPago.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


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

        public async Task<decimal> ObterValorKitAsync(int idInscricao)
        {
            var kit = await _context.Inscricao
                .AsNoTracking()
                .Where(i => i.Id == idInscricao)
                .Select(i => i.IdKitNavigation)
                .FirstOrDefaultAsync();

            return kit?.Valor ?? 0m;
        }
    }
}

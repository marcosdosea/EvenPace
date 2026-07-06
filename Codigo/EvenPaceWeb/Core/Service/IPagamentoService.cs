using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Service.Dtos;

namespace Core.Service
{
    public interface IPagamentoService
    {
        /// <summary>Processa pagamento no Mercado Pago e salva no banco.</summary>
        Task<ResultadoPagamentoDto> ProcessarAsync(ProcessarPagamentoDto dto);

        /// <summary>Cria uma preferência do Checkout Pro e retorna a URL para redirecionamento.</summary>
        Task<string> CriarCheckoutProAsync(
            int idInscricao,
            string successUrl,
            string pendingUrl,
            string failureUrl,
            bool habilitarRetornoAutomatico,
            string? notificationUrl);

        /// <summary>Cria uma preferência do Checkout Pro para uma inscrição ainda não salva.</summary>
        Task<CheckoutProDto> CriarCheckoutProInscricaoAsync(
            int idCorredor,
            int idEvento,
            int idKit,
            string distancia,
            string tamanhoCamisa,
            string successUrl,
            string pendingUrl,
            string failureUrl,
            bool habilitarRetornoAutomatico,
            string? notificationUrl);

        Task<ResultadoPagamentoDto> VerificarCheckoutProPorReferenciaAsync(string externalReference);

        /// <summary>Consulta o pagamento do Checkout Pro no Mercado Pago e atualiza a inscrição.</summary>
        Task<ResultadoPagamentoDto> RegistrarRetornoCheckoutProAsync(string paymentId, string? externalReference, string? statusRetorno);

        /// <summary>Retorna o valor do kit da inscrição.</summary>
        Task<decimal> ObterValorKitAsync(int idInscricao);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json.Serialization;

namespace Core.Service.Dtos
{
    /// <summary>
    /// Dados enviados pelo Card Payment Brick do Mercado Pago.
    /// O JS monta tudo isso automaticamente; só o idInscricao é adicionado manualmente no onSubmit.
    /// </summary>
    public class ProcessarPagamentoDto
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = null!;

        [JsonPropertyName("issuer_id")]
        public string? IssuerId { get; set; }

        [JsonPropertyName("payment_method_id")]
        public string PaymentMethodId { get; set; } = null!;

        [JsonPropertyName("transaction_amount")]
        public decimal TransactionAmount { get; set; }

        [JsonPropertyName("installments")]
        public int Installments { get; set; } = 1;

        [JsonPropertyName("payer")]
        public ProcessarPagamentoPayer? Payer { get; set; }

        /// <summary>Campo extra adicionado no onSubmit do JS para identificar a inscrição.</summary>
        [JsonPropertyName("idInscricao")]
        public int IdInscricao { get; set; }
    }

    public class ProcessarPagamentoPayer
    {
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("identification")]
        public ProcessarPagamentoIdentification? Identification { get; set; }
    }

    public class ProcessarPagamentoIdentification
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("number")]
        public string? Number { get; set; }
    }
}

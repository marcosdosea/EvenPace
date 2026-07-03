namespace EvenPaceWeb.Models
{
    public class PagamentoViewModel
    {
        public int IdInscricao { get; set; }
        public string NomeEvento { get; set; } = string.Empty;
        public string NomeCorredor { get; set; } = string.Empty;
        public string? NomeKit { get; set; }
        public string Distancia { get; set; } = string.Empty;
        public string TamanhoCamisa { get; set; } = string.Empty;
        public decimal ValorKit { get; set; }

        /// <summary>Public Key do Mercado Pago — vem do appsettings via controller</summary>
        public string MercadoPagoPublicKey { get; set; } = string.Empty;

        // Usados pela view Resultado.cshtml
        public string? StatusPagamento { get; set; }
        public string? IdTransacaoMP { get; set; }
    }
}
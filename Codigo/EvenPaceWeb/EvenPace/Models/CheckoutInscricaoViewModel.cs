namespace EvenPaceWeb.Models
{
    public class CheckoutInscricaoViewModel
    {
        public int IdEvento { get; set; }
        public int IdKit { get; set; }
        public string Distancia { get; set; } = string.Empty;
        public string TamanhoCamisa { get; set; } = string.Empty;
        public string CheckoutUrl { get; set; } = string.Empty;
        public string ExternalReference { get; set; } = string.Empty;
        public string StatusUrl { get; set; } = string.Empty;
        public string ResultadoUrl { get; set; } = string.Empty;
    }
}

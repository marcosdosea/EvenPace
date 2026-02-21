namespace Core.Service.Dtos;

/// <summary>
/// DTO com dados para a tela de confirmação de cancelamento de inscrição.
/// </summary>
public class DadosTelaDeleteDto
{
    public string NomeEvento { get; set; } = null!;
    public DateTime DataEvento { get; set; }
    public string Local { get; set; } = null!;
    public string NomeKit { get; set; } = null!;
    public int IdInscricao { get; set; }
    public string Distancia { get; set; } = null!;
    public string TamanhoCamisa { get; set; } = null!;
    public DateTime DataInscricao { get; set; }
}

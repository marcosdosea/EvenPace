namespace Core.Service.Dtos;

/// <summary>
/// Resultado da obtenção dos dados para tela de cancelamento.
/// </summary>
public class GetDadosTelaDeleteResult
{
    public bool Success { get; set; }
    /// <summary> "NotFound" ou "EventoExpirado" quando Success é false. </summary>
    public string? ErrorType { get; set; }
    public DadosTelaDeleteDto? Data { get; set; }
}

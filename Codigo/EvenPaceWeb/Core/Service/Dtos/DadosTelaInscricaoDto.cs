using Core;

namespace Core.Service.Dtos;

/// <summary>
/// DTO com dados para montagem da tela de inscrição (evento + kits).
/// </summary>
public class DadosTelaInscricaoDto
{
    public int IdEvento { get; set; }
    public string NomeEvento { get; set; } = null!;
    public string Local { get; set; } = null!;
    public DateTime DataEvento { get; set; }
    public string Descricao { get; set; } = null!;
    public string? ImagemEvento { get; set; }
    public IEnumerable<Kit> Kits { get; set; } = new List<Kit>();
}

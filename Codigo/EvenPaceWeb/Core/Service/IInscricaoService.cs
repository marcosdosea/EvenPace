using Core.Service.Dtos;

namespace Core.Service;

public interface IInscricaoService
{
    Task EditAsync(Inscricao inscricao);
    Task<int> CreateAsync(Inscricao inscricao);
    Task<Inscricao?> GetAsync(int id);
    Task DeleteAsync(int id);
    Task CancelarAsync(int idInscricao, int idCorredor);

    Task<IEnumerable<Inscricao>> GetAllAsync();
    Task<IEnumerable<Inscricao>> GetAllByEventoAsync(int idEvento);

    /// <summary>Obtém dados para a tela de inscrição (evento + kits).</summary>
    Task<DadosTelaInscricaoDto> GetDadosTelaInscricaoAsync(int idEvento);

    /// <summary>Obtém dados para a tela de cancelamento. Retorna Success=false com ErrorType quando inscrição não existe ou evento já passou.</summary>
    Task<GetDadosTelaDeleteResult> GetDadosTelaDeleteAsync(int idInscricao);

    Task ConfirmarRetiradaKitAsync(int idInscricao);
}

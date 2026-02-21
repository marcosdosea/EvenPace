using System.Collections.Generic;
using Core.Service.Dtos;

namespace Core.Service;

public interface IInscricaoService
{
    void Edit(Inscricao inscricao);
    int Create(Inscricao inscricao);
    Inscricao Get(int id);
    void Delete(int id);
    void Cancelar(int idInscricao, int idCorredor);

    IEnumerable<Inscricao> GetAll();
    IEnumerable<Inscricao> GetAllByEvento(int idEvento);

    /// <summary>Obtém dados para a tela de inscrição (evento + kits).</summary>
    DadosTelaInscricaoDto GetDadosTelaInscricao(int idEvento);

    /// <summary>Obtém dados para a tela de cancelamento. Retorna Success=false com ErrorType quando inscrição não existe ou evento já passou.</summary>
    GetDadosTelaDeleteResult GetDadosTelaDelete(int idInscricao);
}

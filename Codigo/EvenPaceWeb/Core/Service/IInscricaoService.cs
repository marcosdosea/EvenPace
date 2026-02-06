using System.Collections.Generic;

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
}

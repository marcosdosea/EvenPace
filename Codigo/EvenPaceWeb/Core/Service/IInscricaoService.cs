namespace Core.Service;

public interface IInscricaoService
{
    void Edit(Inscricao inscricao);
    int Create(Inscricao inscricao);
    Inscricao Get(int id);
    void Delete(int id);
    IEnumerable<Inscricao> GetAll();
}
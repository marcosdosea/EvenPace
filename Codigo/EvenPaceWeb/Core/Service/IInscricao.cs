namespace Core.Service;

public interface IInscricao
{
    void Edit(Inscricao inscricao);
    uint Create(Inscricao inscricao);
    Inscricao Get(int id);
    void Delete(int id);
    IEnumerable<Inscricao> GetAll();
}
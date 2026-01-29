namespace Core.Service;

public interface IOrganizacaoService
{
    void Edit(Organizacao organizacao);
    int Create(Organizacao organizacao);
    Organizacao Get(int id);
    void Delete(int id);
    IEnumerable<Organizacao> GetAll();
}
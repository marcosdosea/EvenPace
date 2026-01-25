using Core;
using Core.Service;

namespace Service;

public class OrganizacaoService : IOrganizacaoService
{
    private EvenPaceContext _context;

    public OrganizacaoService(EvenPaceContext context)
    {
        _context = context;
    }

    public void Edit(Organizacao organizacao)
    {
        _context.Update(organizacao);
        _context.SaveChanges();
    }

    public uint Create(Organizacao organizacao)
    {
        _context.Add(organizacao);
        _context.SaveChanges();
        return organizacao.Id;
    }

    public Organizacao Get(int id)
    {
        var _organizacao = _context.Organizacaos.Find(id);
        return _organizacao;
    }

    public void Delete(int id)
    {
        var  _organizacao = _context.Organizacaos.Find(id);
        _context.Remove(_organizacao);
        _context.SaveChanges();
    }

    public IEnumerable<Organizacao> GetAll()
    {
        return _context.Organizacaos;
    }
}
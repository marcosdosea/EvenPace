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

    /// <summary>
    /// Confirma e envia uma requisição formalizada das modificações propostas nas informações das organizações diretamente às tabelas transacionais do Entity.
    /// </summary>
    /// <param name="organizacao">Representação relacional contendo o CNPJ ativamente preenchido e já acoplado às referências revisadas do organizador.</param>
    public void Edit(Organizacao organizacao)
    {
        _context.Update(organizacao);
        _context.SaveChanges();
    }

    /// <summary>
    /// Implanta sistematicamente no núcleo físico de dados uma recém gerada conta gestora destinada a gerenciar as transações dos eventos e providenciar autenticidade de criador.
    /// </summary>
    /// <param name="organizacao">Objeto contendo informações de acesso essenciais à autenticação do futuro cliente mantenedor.</param>
    /// <returns>Id atribuído ao perfil logo após a efetivação no repositório final relacional do framework.</returns>
    public int Create(Organizacao organizacao)
    {
        _context.Add(organizacao);
        _context.SaveChanges();
        return organizacao.Id;
    }

    /// <summary>
    /// Promove a sondagem relacional baseada num código unicamente estabelecido a recuperar, sem a exigência de consultas expansivas, a documentação pertinente do gestor no escopo lógico da estrutura persistida no SQL.
    /// </summary>
    /// <param name="id">Chave atrelativa designada para filtrar as informações sem equívocos durante extrações conjuntas.</param>
    /// <returns>Modelo isolado correspondente e já interpretado sob o registro em DBSet; será retornado um valor nulo em cenários inconclusivos ou divergentes com a base local do programa.</returns>
    public Organizacao Get(int id)
    {
        var _organizacao = _context.Organizacaos.Find(id);
        return _organizacao;
    }

    /// <summary>
    /// Destitui e rompe as diretrizes relacionais da conta com os bancos operantes atrelados no EvenPace, destruindo vestígios informativos de perfil configurados antecipadamente.
    /// </summary>
    /// <param name="id">Número de localização no escopo do contexto de dados do organizador submetido aos parâmetros do framework de persistência visual.</param>
    public void Delete(int id)
    {
        var _organizacao = _context.Organizacaos.Find(id);
        _context.Remove(_organizacao);
        _context.SaveChanges();
    }

    /// <summary>
    /// Elege a devolução indiscriminada de toda a malha atrelada de contas operacionais de organizações registradas que ainda não foram destituídas do programa.
    /// </summary>
    /// <returns>Estrutura interativa com mapeamentos das propriedades dos organizadores globais de todos as raias contidas.</returns>
    public IEnumerable<Organizacao> GetAll()
    {
        return _context.Organizacaos;
    }
}
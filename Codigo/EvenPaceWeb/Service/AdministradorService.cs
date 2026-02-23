
using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class AdministradorService : IAdministradorService
    {
        private readonly EvenPaceContext _context;

        public AdministradorService(EvenPaceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Insere um administrador no banco de dados
        /// </summary>
        /// <param name="administrador">As diretrizes e campos do gestor estruturados em entidade pronta a assinar a validação ORM.</param>
        /// <returns>A chave primária estipulada e adotada pela base nas dependências de rastreabilidade geradas.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public int Create(Administrador administrador)
        {
            _context.Add(administrador);
            _context.SaveChanges();
            return(int) administrador.Id;
        }

        /// <summary>
        /// Deleta um administrador do banco de dados
        /// </summary>
        /// <param name="id">Identificador unificado pertencente à entidade alvo do expurgo.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void Delete(int id)
        {
            var _administrador = _context.Administradors.Find(id);

            if (_administrador is not null)
            {
                _context.Remove(_administrador);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Edita um administrador no banco de dados
        /// </summary>
        /// <param name="administrador">A configuração completa e repopulada contendo a sobreposição ao registro obsoleto atrelado.</param>
        public void Edit(Administrador administrador)
        {
            if (administrador is not null)
            {
                _context.Administradors.Find(administrador.Id);
                _context.Update(administrador);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Busca um administrador pelo Id
        /// </summary>
        /// <param name="id">Índice pontual providenciado a interceptar informações isoladas e requeridas a respeito desta submissão de perfil particular.</param>
        /// <returns>Delega de volta a entidade formatada correspondente ou vazio na sua recusa em listagem.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Administrador Get(int id)
        {
            return _context.Administradors.Find(id);
        }

        /// <summary>
        /// Pega todos os administradores do banco de dados
        /// </summary>
        /// <returns>Pacote extensivo contendo as contas gestoras mantidas no núcleo.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<Administrador> GetAll()
        {
            return _context.Administradors.AsNoTracking();
        }

        /// <summary>
        /// Pega administradores pelo nome
        /// </summary>
        /// <param name="nome">Expressão referenciadora alocada para estipular o critério condicional estrito de devolução do conteúdo no filtro contido.</param>
        /// <returns>Repassa apenas subconjunto iterativo acoplado aos perfis que concordem estritamente na validação embutida baseada na condição designada de conter a palavra nas cadeias do núcleo referencial logado operante (AsNoTracking).</returns>
        public IEnumerable<Administrador> GetByName(string nome)
        {
            return _context.Administradors.Where(a => a.Nome.Contains(nome)).AsNoTracking();
        }
        
    }
}

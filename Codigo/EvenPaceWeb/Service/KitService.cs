using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class KitService : IKitService
    {
        private readonly EvenPaceContext _context;
        public KitService(EvenPaceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Realiza a inserção dos dados fornecidos e estabelece o registro físico e lógico do Kit dentro da estrutura do banco.
        /// </summary>
        /// <param name="kit">Propriedades encapsuladas referenciando o kit a ser persistido.</param>
        /// <returns>A chave gerada pelo banco para atrelar a entidade criada.</returns>
        public int Create(Kit kit)
        {
            _context.Add(kit);
            _context.SaveChanges();
            return kit.Id;
        }

        /// <summary>
        /// Gerencia as modificações realizadas nos dados preexistentes do Kit, lidando com conflitos de estado de entidade (Local) em memória.
        /// </summary>
        /// <param name="kit">O objeto que substitui as variáveis antigas da entidade localizada previamente.</param>
        public void Edit(Kit kit)
        {
            var local = _context.Set<Kit>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(kit.Id));

            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }

            _context.Entry(kit).State = EntityState.Modified;

            _context.SaveChanges();
        }

        /// <summary>
        /// Remove de forma categórica e definitiva a referência atrelada ao registro do kit das matrizes no banco de dados da aplicação.
        /// </summary>
        /// <param name="id">Chave primária numérica e única informada do kit.</param>
        public void Delete(int id)
        {
            var _kit = _context.Kits.Find((int)id);

            if (_kit is not null)
            {
                _context.Remove(_kit);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Concede acesso à captura das informações diretas de um único kit na base com a respectiva chave de localização passada.
        /// </summary>
        /// <param name="id">Código primário de identificação da busca.</param>
        /// <returns>Retorna a entidade mapeada pertinente ao respectivo número informado.</returns>
        public Kit Get(int id)
        {
            return _context.Kits.Find((int)id)!;
        }

        /// <summary>
        /// Adquire toda a relação extensa catalogada contendo os kits existentes na base de dados de maneira integral e irrestrita.
        /// </summary>
        /// <returns>Coleção genérica contendo todo o pool de resultados correspondentes.</returns>
        public IEnumerable<Kit> GetAll()
        {
            return _context.Kits.ToList();
        }

        /// <summary>
        /// Submete a leitura via consulta de caracteres, varrendo a base de dados sob os kits para retornar todas as concordâncias atreladas ao filtro de texto.
        /// </summary>
        /// <param name="nome">Expressão referenciadora que dita o comportamento fracionado do filtro.</param>
        /// <returns>Coleção textual alinhada e compativel aos critérios exigidos pela regra negocial.</returns>
        public IEnumerable<Kit> GetByName(string nome)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Produz uma consulta condicional que agrupa de forma relacional os múltiplos kits ativados contidos ou programados à vigência exclusiva de um evento esportivo específico.
        /// </summary>
        /// <param name="idEvento">Id da entidade de evento da qual pretende extrair todos os ingressos de kit criados a ele.</param>
        /// <returns>Compilação segmentada listando as informações resultantes da requisição filtrada.</returns>
        public IEnumerable<Kit> GetKitsPorEvento(int idEvento)
        {
            return _context.Kits
                           .Where(k => k.IdEvento == (int)idEvento)
                           .ToList();
        }
    }
}
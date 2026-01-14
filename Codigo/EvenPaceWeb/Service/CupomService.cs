using Core;
using Core.Service;

namespace Service
{
    internal class CupomService : ICupom
    {
        private readonly EvenPaceContext _context;

        public CupomService(EvenPaceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Insere um cupom no banco de dados
        /// </summary>
        /// <param name="cupom"></param>
        /// <returns>Retorna o Id do cupom</returns>
        public uint Insert(Cupom cupom)
        {
            _context.Add(cupom);
            _context.SaveChanges();
            return cupom.Id;
        }

        /// <summary>
        /// Edita um cupom no banco de dados
        /// </summary>
        /// <param name="cupom"></param>
        public void Edit(Cupom cupom)
        {
            _context.Cupoms.Find(cupom.Id);
            _context.Update(cupom);
            _context.SaveChanges();
        }

        /// <summary>
        /// Deleta um cupom do banco de dados
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var _cupom = _context.Cupoms.Find(id);
            _context.Remove(_cupom);
            _context.SaveChanges();

        }

        /// <summary>
        /// Pega um cupom do banco de dados pelo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retorna o cupom</returns>
        public Cupom Get(int id)
        {
            return _context.Cupoms.Find(id);
        }

        /// <summary>
        /// Pega todos os cupons do banco de dados
        /// </summary>
        /// <returns>Retorna todos os cupons no banco de dados</returns>
        public IEnumerable<Cupom> GetAll()
        {
            return _context.Cupoms.ToList();
        }

        /// <summary>
        /// Pega cupons pelo nome
        /// </summary>
        /// <param name="nome"></param>
        /// <returns>Retorna todos os cupons com o nome inserido</returns>
        public IEnumerable<Cupom> GetByName(string nome)
        {
            return _context.Cupoms.Where(c => c.Nome.Contains(nome)).ToList();
        }
    }
}

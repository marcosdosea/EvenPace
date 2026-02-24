
using Core;
using Core.Service;

namespace Service
{
    public class CorredorService : ICorredorService
    {
        private readonly EvenPaceContext _context;
        public CorredorService(EvenPaceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Insere um evento no banco de dados
        /// </summary>
        /// <param name="corredor"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int Create(Corredor corredor)
        {
            _context.Add(corredor);
            _context.SaveChanges();
            return corredor.Id;
        }

        /// <summary>
        /// Encontra o Corredor com o id correspondente
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retorna um Corredor</returns>
        public Corredor Get(int id)
        {
            var lista = _context.Corredors.ToList();
            Console.WriteLine($"[DEBUG] Total de corredores no contexto: {lista.Count}");
            if (lista.Count > 0)
            {
                Console.WriteLine($"[DEBUG] IDs encontrados: {string.Join(", ", lista.Select(c => c.Id))}");
            }

            return lista.FirstOrDefault(c => c.Id == id);
        }

        public Corredor GetByCpf(string cpf)
        {
            return _context.Corredors.SingleOrDefault(c => c.Cpf == cpf);
        }

        /// <summary>
        /// Pega o corredor com email e senha compativeis
        /// </summary>
        /// <param name="corredor"></param>
        public void Edit(Corredor corredor)
        {
            if (corredor != null)
            {
                _context.Administradors.Find(corredor.Id);
                _context.Update(corredor);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Deleta um evento do banco de dados
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var _corredor = _context.Corredors.Find(id);

            if (_corredor != null)
            {
                _context.Remove(_corredor);
                _context.SaveChanges();
            }
        }
        
        /// <summary>
        /// Pega todos os eventos do banco de dados
        /// </summary>
        /// <returns>Retorna todos os Eventos cadastrados</returns>
        public IEnumerable<Corredor> GetAll()
        {
            return _context.Corredors.ToList();
        }

        public IEnumerable<Corredor> GetByName(string nome)
        {
            return _context.Corredors.Where(e => e.Nome.Contains(nome)).ToList();
        }
    }
}

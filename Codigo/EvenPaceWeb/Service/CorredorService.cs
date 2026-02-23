
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
        /// <param name="corredor">A entidade contendo as informações do novo corredor a ser cadastrado.</param>
        /// <returns>Retorna o Id numérico gerado pelo banco para a nova entidade.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public int Create(Corredor corredor)
        {
            _context.Add(corredor);
            _context.SaveChanges();
            return corredor.Id;
        }
        /// <summary>
        /// Localiza o cadastro de um corredor baseando-se no endereço de e-mail registrado.
        /// </summary>
        /// <param name="email">E-mail único de registro do usuário procurado.</param>
        /// <returns>Retorna a entidade do corredor correspondente ou valor nulo se inexistente.</returns>
        public Corredor? GetByEmail(string email)
        {
            return _context.Corredors
                    .FirstOrDefault(c => c.Email == email);
        }

        /// <summary>
        /// Encontra o Corredor com o id correspondente
        /// </summary>
        /// <param name="id">Chave primária do corredor requisitado.</param>
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

        /// <summary>
        /// Pega o corredor com email e senha compativeis
        /// </summary>
        /// <param name="corredor">A entidade já instanciada com os dados e chaves prontas para substituição.</param>
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
        /// <param name="id">O identificador exato atrelado ao registro do corredor no banco de dados.</param>
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
        /// <summary>
        /// Realiza a filtragem textual de corredores buscando correspondências fragmentadas pelo nome do usuário.
        /// </summary>
        /// <param name="nome">Sequência de caracteres que deve estar contida no nome do corredor buscado.</param>
        /// <returns>Coleção de corredores cujos nomes satisfazem o critério estipulado.</returns>
        public IEnumerable<Corredor> GetByName(string nome)
        {
            return _context.Corredors.Where(e => e.Nome.Contains(nome)).ToList();
        }
    }
}

using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

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
        /// Insere um corredor no banco de dados
        /// </summary>
        /// <param name="corredor"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public uint Create(Corredor corredor)
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
        public Corredor Get(uint id)
        {
            return  _context.Corredors.FirstOrDefault(c => c.Id == id);
        }
        
        /// <summary>
        /// Pega o corredor com email e senha compativeis
        /// </summary>
        /// <param name="email"></param>
        /// <param name="senha"></param>
        /// <returns>retorna corredor com email e senha compativeis</returns>
        public Corredor Login(string email, string senha)
        {
            return _context.Corredors.FirstOrDefault(e => e.Email == email && e.Senha == senha);
        }

        /// <summary>
        /// Deleta um corredor do banco de dados
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="NotImplementedException"></exception>
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
        /// Edita um corredor no banco de dados
        /// </summary>
        /// <param name="corredor"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Edit(Corredor corredor)
        {
            if (corredor is not null)
            {
                _context.Administradors.Find(corredor.Id);
                _context.Update(corredor);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Pega todos os corredores do banco de dados
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Corredor> GetAll()
        {
            return _context.Corredors.AsNoTracking();
        }

        /// <summary>
        /// Pega corredores pelo nome
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<Corredor> GetByName(string nome)
        {
            return _context.Corredors.Where(c => c.Nome.Contains(nome)).AsNoTracking();
        }
    }
}

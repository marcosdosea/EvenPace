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
        /// Insere um kit no banco de dados
        /// </summary>
        /// <param name="kit"></param>
        /// <returns>Retorna o Id do Kit</returns>
        public int Create(Kit kit)
        {
            _context.Add(kit);
            _context.SaveChanges();
            return kit.Id;
        }

        /// <summary>
        /// Edita um kit no banco de dados
        /// </summary>
        /// <param name="kit"></param>
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
        /// Deleta um kit do banco de dados
        /// </summary>
        /// <param name="id"></param>
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
        /// Busca um kit pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retorna o kit</returns>
        public Kit Get(int id)
        {
            return _context.Kits.Find((int)id)!;
        }
      
        public IEnumerable<Kit> GetAll()
        {
            return _context.Kits.ToList();
        }

        public IEnumerable<Kit> GetByName(string nome)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Kit> GetKitsPorEvento(int idEvento)
        {
            // Convertemos o idEvento para int para bater com o tipo da tabela
            return _context.Kits
                           .Where(k => k.IdEvento == (int)idEvento)
                           .ToList();
        }

    }
}

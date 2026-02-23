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

        public int Create(Kit kit)
        {
            _context.Add(kit);
            _context.SaveChanges();
            return kit.Id;
        }

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

        public void Delete(int id)
        {
            var _kit = _context.Kits.Find((int)id);

            if (_kit is not null)
            {
                _context.Remove(_kit);
                _context.SaveChanges();
            }
        }

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
            return _context.Kits
                           .Where(k => k.IdEvento == (int)idEvento)
                           .ToList();
        }
    }
}
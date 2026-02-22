using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class CupomService : ICupomService
    {
        private readonly EvenPaceContext _context;

        public CupomService(EvenPaceContext context)
        {
            _context = context;
        }

        public int Create(Cupom cupom)
        {
            cupom.Id = 0;
            _context.Add(cupom);
            _context.SaveChanges();
            return cupom.Id;
        }

        public void Edit(Cupom cupom)
        {
            _context.Update(cupom);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var cupom = _context.Cupoms.Find(id);

            _context.Remove(cupom);
            _context.SaveChanges();
        }

        public Cupom Get(int id)
        {
            return _context.Cupoms.FirstOrDefault(cupom => cupom.Id == id)
            ?? throw new ServiceException("Cupom não encontrado");
        }

        public IEnumerable<Cupom> GetAll()
        {
            return _context.Cupoms.AsNoTracking();
        }

        public IEnumerable<Cupom> GetByName(string nome)
        {
            return _context.Cupoms.Where(c => c.Nome.Contains(nome)).AsNoTracking();
        }
    }
}
using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class InscricaoService : IInscricaoService
    {
        private EvenPaceContext _context;

        public InscricaoService(EvenPaceContext context)
        {
            _context = context;
        }


        public uint Create(Inscricao inscricao)
        {
            _context.Add(inscricao);
            _context.SaveChanges();
            return inscricao.Id;
        }

        public void Edit(Inscricao inscricao)
        {
            _context.Update(inscricao);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var _inscricao = _context.Inscricaos.Find(id);
            _context.Remove(_inscricao);
            _context.SaveChanges();            
        }

        public Inscricao Get(int id)
        {
            return _context.Inscricaos.Find(id);
        }

        public IEnumerable<Inscricao> GetAll()
        {
            return _context.Inscricaos.AsNoTracking();
        }
    }
}

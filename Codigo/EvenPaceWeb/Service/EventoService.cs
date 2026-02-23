using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class EventoService : IEventosService
    {
        private readonly EvenPaceContext _context;
        public EventoService(EvenPaceContext context)
        {
            _context = context;
        }

        public int Create(Evento eventos)
        {
            _context.Add(eventos);
            _context.SaveChanges();
            return eventos.Id;
        }

        public void Edit(Evento eventoEditado)
        {
            if (eventoEditado is not null)
            {
                var local = _context.Set<Evento>()
                    .Local
                    .FirstOrDefault(entry => entry.Id.Equals(eventoEditado.Id));

                if (local != null)
                {
                    _context.Entry(local).State = EntityState.Detached;
                }

                _context.Entry(eventoEditado).State = EntityState.Modified;

                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var _evento = _context.Eventos.Find(id);
            if (_evento is not null)
            {
                _context.Remove(_evento);
                _context.SaveChanges();
            }
        }

        public Evento Get(int id)
        {
            return _context.Eventos.AsNoTracking().FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<Evento> GetAll()
        {
            return _context.Eventos.AsNoTracking();
        }

        public IEnumerable<Evento> GetByName(string nome)
        {
            return _context.Eventos.Where(e => e.Nome.Contains(nome)).AsNoTracking();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        // --- AQUI ESTAVA O ERRO ---
        // Método Edit corrigido para evitar conflito de IDs
        public void Edit(Evento eventoEditado)
        {
            if (eventoEditado is not null)
            {
                // 1. Verifica se já existe uma entidade com esse ID rastreada na memória do EF
                var local = _context.Set<Evento>()
                    .Local
                    .FirstOrDefault(entry => entry.Id.Equals(eventoEditado.Id));

                // 2. Se existir, "desanexa" (Detach) ela para não dar conflito
                if (local != null)
                {
                    _context.Entry(local).State = EntityState.Detached;
                }

                // 3. Agora dizemos que o estado da nova entidade é "Modificado"
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
            // Usamos AsNoTracking aqui para evitar problemas de leitura em cenários de edição
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
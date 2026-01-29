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

        /// <summary>
        /// Insere um evento no banco de dados
        /// </summary>
        /// <param name="eventos"></param>
        /// <returns>Retorna o valor o id evento</returns>
        public int Create(Evento eventos)
        {
            _context.Add(eventos);
            _context.SaveChanges();
            return eventos.Id;
        }

        /// <summary>
        /// Edita um evento no banco de dados
        /// </summary>
        /// <param name="eventos"></param>
        public void Edit(Evento eventos)
        {
            if (eventos is not null)
            {
                _context.Eventos.Find(eventos.Id);
                _context.Update(eventos);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Deleta um evento do banco de dados
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var _evento = _context.Eventos.Find(id);

            if (_evento is not null)
            {
                _context.Remove(_evento);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Busca um evento pelo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retorna um evento</returns>
        public Evento Get(int id)
        {
            return _context.Eventos.Find(id)!;
        }

        /// <summary>
        /// Pega todos os eventos do banco de dados
        /// </summary>
        /// <returns>Retorna todos os Eventos cadastrados</returns>
        public IEnumerable<Evento> GetAll()
        {
            return _context.Eventos.AsNoTracking();
        }

        /// <summary>
        /// Pega eventos pelo nome
        /// </summary>
        /// <param name="nome"></param>
        /// <returns>Retorna todos os Eventos que contein a string</returns>
        public IEnumerable<Evento> GetByName(string nome)
        {
            return _context.Eventos.Where(e => e.Nome.Contains(nome)).AsNoTracking(); 
        }
    }
}

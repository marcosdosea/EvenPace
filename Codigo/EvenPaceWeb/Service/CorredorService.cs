using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Service;

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
        /// <param name="corredor"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int Create(Corredor corredor)
        {
            _context.Add(eventos);
            _context.SaveChanges();
            return eventos.Id;
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
        
        /// <summary>
        /// Pega o corredor com email e senha compativeis
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
            if (corredor == null && corredor.Id == 0) throw new ServiceException("Corredor Invalido");
            
            _context.Corredors.Update(corredor);
            _context.SaveChanges();
        }
        
        
        /// <summary>
        /// Pega todos os eventos do banco de dados
        /// </summary>
        /// <returns>Retorna todos os Eventos cadastrados</returns>
        public IEnumerable<Evento> GetAll()
        {
            return _context.Eventos.ToList();
        }

        public IEnumerable<Evento> GetByName(string nome)
        {
            return _context.Eventos.Where(e => e.Nome.Contains(nome)).ToList();
        }
    }
}

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
    public class AvaliacaoEventoService : IAvaliacaoEventoService
    {
        private readonly EvenPaceContext _context;

        public AvaliacaoEventoService(EvenPaceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Cria uma avaliação
        /// </summary>
        /// <param name="avaliacaoevento"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public uint Create(AvaliacaoEvento avaliacaoevento)
        {
            _context.Add(avaliacaoevento);
            _context.SaveChanges();
            return avaliacaoevento.Id;

        }

        /// <summary>  
        /// Deleta uma avaliação de evento do banco de dados  
        /// </summary>  
        /// <param name="id"></param>  
        public void Delete(int id)
        {
            var _avaliacaoEvento = _context.AvaliacaoEventos.Find(id);

            if (_avaliacaoEvento != null)
            {
                _context.Remove(_avaliacaoEvento);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Edita uma avaliação de evento
        /// </summary>
        /// <param name="avaliacaoEvento"></param>
        public void Edit(AvaliacaoEvento avaliacaoEvento)
        {
            if (avaliacaoEvento is not null)
            {
                _context.Administradors.Find(avaliacaoEvento.Id);
                _context.Update(avaliacaoEvento);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Pega uma avaliação pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AvaliacaoEvento Get(int id)
        {
            return _context.AvaliacaoEventos.Find(id);
        }

        /// <summary>
        /// Lista todas as avaliações
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<AvaliacaoEvento> GetAll()
        {
            return _context.AvaliacaoEventos.AsNoTracking();
        }

        // Este método está aqui apenas para não dar erro na classe, no entanto,
        // AvaliaçãoEvento não tem nome.
        public IEnumerable<AvaliacaoEvento> GetByName(string nome)
        {
            throw new NotImplementedException(); 
        }
    }
}

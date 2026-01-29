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
    public class CupomService : ICupomService
    {
        private readonly EvenPaceContext _context;

        public CupomService(EvenPaceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Insere um cupom no banco de dados
        /// </summary>
        /// <param name="cupom"></param>
        /// <returns>Retorna o Id do cupom</returns>
        public int Create(Cupom cupom)
        {
            _context.Add(cupom);
            _context.SaveChanges();
            return cupom.Id;
        }

        /// <summary>
        /// Edita um cupom no banco de dados
        /// </summary>
        /// <param name="cupom"></param>
        public void Edit(Cupom cupom)
        {
            if (cupom == null && cupom.Id > 0) throw new ServiceException("Cupom inválido");

            _context.Update(cupom);
            _context.SaveChanges();
            
        }

        /// <summary>
        /// Deleta um cupom do banco de dados
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var _cupom = _context.Cupoms.Find(id);

            if (_cupom is not null)
            {
                _context.Remove(_cupom);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Pega um cupom do banco de dados pelo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retorna o cupom</returns>
        public Cupom Get(int id)
        {
            //return _context.Cupoms.Find(id);
            return _context.Cupoms.FirstOrDefault(cupom => cupom.Id == id)
            ?? throw new ServiceException("Cupom não encontrado");
        }

        /// <summary>
        /// Pega todos os cupons do banco de dados
        /// </summary>
        /// <returns>Retorna todos os cupons no banco de dados</returns>
        public IEnumerable<Cupom> GetAll()
        {
            return _context.Cupoms.AsNoTracking();
        }

        /// <summary>
        /// Pega cupons pelo nome
        /// </summary>
        /// <param name="nome"></param>
        /// <returns>Retorna todos os cupons com o nome inserido</returns>
        public IEnumerable<Cupom> GetByName(string nome)
        {
            return _context.Cupoms.Where(c => c.Nome.Contains(nome)).AsNoTracking();
        }
    }

}

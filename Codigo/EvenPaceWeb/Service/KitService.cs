using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Service;

namespace Service
{
    internal class KitService : IKitService
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
        public uint Create(Kit kit)
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
            if (kit is not null)
            {
                _context.Kits.Find(kit.Id);
                _context.Update(kit);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Deleta um kit do banco de dados
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var _kit = _context.Kits.Find(id);

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
            return _context.Kits.Find(id);
        }
    }
}

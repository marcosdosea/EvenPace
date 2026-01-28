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
            // 1. Verifica se já existe algum Kit com esse ID na memória do EF
            var local = _context.Set<Kit>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(kit.Id));

            // 2. Se existir, "desanexa" (solta) ele para não dar conflito
            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }

            // 3. Agora dizemos que O NOSSO kit (que veio da tela) é o que vale e foi modificado
            _context.Entry(kit).State = EntityState.Modified;

            // 4. Salva
            _context.SaveChanges();
        }

        /// <summary>
        /// Deleta um kit do banco de dados
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var _kit = _context.Kits.Find((uint)id);

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
            return _context.Kits.Find((uint)id)!;
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
            // Convertemos o idEvento para uint para bater com o tipo da tabela
            return _context.Kits
                           .Where(k => k.IdEvento == (uint)idEvento)
                           .ToList();
        }

    }
}

using Core;
using Core.Service;

namespace Service
{
    internal class CupomService(EvenPaceContext context) : ICupom
    {
        /// <summary>
        /// Insere um cupom no banco de dados
        /// </summary>
        /// <param name="cupom"></param>
        /// <returns>Retorna o Id do cupom</returns>
        public uint Insert(Cupom cupom)
        {
            context.Add(cupom);
            context.SaveChanges();
            return cupom.Id;
        }

        /// <summary>
        /// Edita um cupom no banco de dados
        /// </summary>
        /// <param name="cupom"></param>
        public void Edit(Cupom cupom)
        {
            context.Cupoms.Find(cupom.Id);
            context.Update(cupom);
            context.SaveChanges();
        }

        /// <summary>
        /// Deleta um cupom do banco de dados
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var _cupom = context.Cupoms.Find(id);
            context.Remove(_cupom); 
            context.SaveChanges();

        }

        /// <summary>
        /// Pega um cupom do banco de dados pelo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retorna o cupom</returns>
        public Cupom Get(int id)
        {
            var _cupom = context.Cupoms.Find(id);
            return _cupom;
        }

        /// <summary>
        /// Pega todos os cupons do banco de dados
        /// </summary>
        /// <returns>Retorna todos os cupons no banco de dados</returns>
        public IEnumerable<Cupom> GetAll()
        {
            return context.Cupoms.ToList();
        }

        /// <summary>
        /// Pega cupons pelo nome
        /// </summary>
        /// <param name="nome"></param>
        /// <returns>Retorna todos os cupons com o nome inserido</returns>
        public IEnumerable<Cupom> GetByName(string nome)
        {
            return context.Cupoms.Where(c => c.Nome.Contains(nome)).ToList();
        }
    }
}

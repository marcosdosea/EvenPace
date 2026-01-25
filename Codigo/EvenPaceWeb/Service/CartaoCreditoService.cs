using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Service;
namespace Service
{
    internal class CartaoCreditoService : ICartaoCredito
    {
        private readonly EvenPaceContext _context;

        public CartaoCreditoService(EvenPaceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Insere um cartão de crédito no banco de dados
        /// </summary>
        /// <param name="cartaoCredito"></param>
        /// <returns>Retorna o Id do cartão</returns>
        public uint Insert(CartaoCredito cartaoCredito)
        {
            _context.Add(cartaoCredito);
            _context.SaveChanges();
            return cartaoCredito.Id;
        }

        /// <summary>
        /// Edita um cartão de crédito no banco de dados
        /// </summary>
        /// <param name="cartaoCredito"></param>
        public void Edit(CartaoCredito cartaoCredito)
        {
            if (cartaoCredito is not null)
            {
                _context.CartaoCreditos.Find(cartaoCredito.Id);
                _context.Update(cartaoCredito);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Deleta um cartão de crédito do banco de dados
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var cartao = _context.CartaoCreditos.Find(id);

            if (cartao is not null)
            {
                _context.Remove(cartao);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Pega um cartão de crédito do banco de dados pelo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retorna o cartão</returns>
        public CartaoCredito Get(int id)
        {
            return _context.CartaoCreditos.Find(id);
        }

        /// <summary>
        /// Pega todos os cartões de crédito do banco de dados
        /// </summary>
        /// <returns>Retorna todos os cartões</returns>
        public IEnumerable<CartaoCredito> GetAll()
        {
            return _context.CartaoCreditos.ToList();
        }
    }
}


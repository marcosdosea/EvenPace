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
        public uint Insert(CartaoCredito cartaoCredito)
        {
            _context.Add(cartaoCredito);
            _context.SaveChanges();
            return cartaoCredito.Id;
        }
        public void Edit(CartaoCredito cartaoCredito)
        {
            if (cartaoCredito is not null)
            {
                _context.CartaoCreditos.Find(cartaoCredito.Id);
                _context.Update(cartaoCredito);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var cartao = _context.CartaoCreditos.Find(id);

            if (cartao is not null)
            {
                _context.Remove(cartao);
                _context.SaveChanges();
            }
        }

        public CartaoCredito Get(int id)
        {
            return _context.CartaoCreditos.Find(id);
        }

        public IEnumerable<CartaoCredito> GetAll()
        {
            return _context.CartaoCreditos.ToList();
        }
    }
}


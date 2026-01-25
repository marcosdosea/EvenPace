using System.Collections.Generic;

namespace Core.Service
{
    public interface ICartaoCredito
    {
        void Edit(CartaoCredito cartaoCredito);
        uint Insert(CartaoCredito cartaoCredito);
        CartaoCredito Get(int id);
        void Delete(int id);
        IEnumerable<CartaoCredito> GetAll();
    }
}


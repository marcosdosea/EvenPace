using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface ICartaoCreditoService
    {
        void Edit(CartaoCredito cartaoCredito);
        uint Create(CartaoCredito cartaoCredito);
        CartaoCredito Get(int id);
        void Delete(int id);
        IEnumerable<CartaoCredito> GetAll();
    }
}

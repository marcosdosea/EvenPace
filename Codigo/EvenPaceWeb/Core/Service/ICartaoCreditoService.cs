using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface ICartaoCreditoService
    {
        void Edit(Cartaocredito cartaoCredito);
        uint Insert(Cartaocredito cartaoCredito);
        Cartaocredito Get(int id);
        void Delete(int id);
        IEnumerable<Cartaocredito> GetAll();
    }
}

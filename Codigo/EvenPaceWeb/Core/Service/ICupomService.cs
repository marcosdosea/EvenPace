using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface ICupomService
    {
        void Edit(Cupom cupom);
        uint Create(Cupom cupom);
        Cupom Get(uint id);
        void Delete(uint id);
        IEnumerable<Cupom> GetAll();
        IEnumerable<Cupom> GetByName(string nome);

    }
}

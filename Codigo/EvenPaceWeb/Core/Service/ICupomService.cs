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
        int Create(Cupom cupom);
        Cupom Get(int id);
        void Delete(int id);
        IEnumerable<Cupom> GetAll();
        IEnumerable<Cupom> GetByName(string nome);

    }
}

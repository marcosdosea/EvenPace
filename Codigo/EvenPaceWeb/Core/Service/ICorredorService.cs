using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface ICorredorService
    {
        void Edit(Corredor corredor);
        uint Create(Corredor corredor);
        Corredor Get(int id);
        void Delete(int id);
        IEnumerable<Corredor> GetAll();
        IEnumerable<Corredor> GetByName(string nome);
    }
}

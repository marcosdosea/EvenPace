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
        int Create(Corredor corredor);
        Corredor Get(int id);
        Corredor Login(string email, string senha);
        void Delete(int id);
        IEnumerable<Corredor> GetAll();
        IEnumerable<Corredor> GetByName(string nome);
        Corredor? GetByEmail(string email);
    }
}

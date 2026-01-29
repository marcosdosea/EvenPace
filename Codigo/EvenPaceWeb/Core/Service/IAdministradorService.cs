using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IAdministradorService
    {
        void Edit(Administrador administrador);
        int Create(Administrador administrador);
        Administrador Get(int id);
        void Delete(int id);
        IEnumerable<Administrador> GetAll();
        IEnumerable<Administrador> GetByName(string nome);
    }
}

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
        Corredor Get(uint id);
        Corredor Login(string email, string senha);
        void Delete(int id);
        IEnumerable<Corredor> GetAll();
        IEnumerable<Corredor> GetByName(string nome);
       // IEnumerable<Evento> GetHistoricoEventos(int idCorredor);
    }
}

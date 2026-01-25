using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IEventos
    {
        void Edit(Evento eventos);
        uint Insert(Evento eventos);
        Evento Get(int id);
        void Delete(int id);
        IEnumerable<Evento> GetAll();
        IEnumerable<Evento> GetByName(string nome);
    }
}

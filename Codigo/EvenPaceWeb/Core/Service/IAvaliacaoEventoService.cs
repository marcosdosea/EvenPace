using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IAvaliacaoEventoService
    {
        void Edit(Avaliacaoevento avaliacaoEvento);
        uint Create(Avaliacaoevento avaliacaoEvento);
        Avaliacaoevento Get(int id);
        void Delete(int id);
        IEnumerable<Avaliacaoevento> GetAll();
        IEnumerable<Avaliacaoevento> GetByEventoId(int eventoId);
    }
}

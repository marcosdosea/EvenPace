using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IAvaliacaoEventoService
    {
        void Edit(AvaliacaoEvento avaliacaoEvento);
        uint Create(AvaliacaoEvento avaliacaoevento);
        AvaliacaoEvento Get(int id);
        void Delete(int id);
        IEnumerable<AvaliacaoEvento> GetAll();
        IEnumerable<AvaliacaoEvento> GetByName(string nome);
    }
}

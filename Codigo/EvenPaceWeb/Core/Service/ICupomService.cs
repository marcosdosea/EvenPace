using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface ICupomService
    {
        Task Edit(Cupom cupom);
        Task<int> Create(Cupom cupom);
        Cupom Get(int id);
        Task Delete(int id);
        Task<IEnumerable<Cupom>> GetAll();
        Task<IEnumerable<Cupom>> GetByName(string nome);
    }
}
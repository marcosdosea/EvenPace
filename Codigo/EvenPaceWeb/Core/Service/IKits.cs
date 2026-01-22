using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IKits
    {
        void Edit(Kit kit);
        uint Insert(Kit kit);
        Kit Get(int id);
        void Delete(int id);
    }
}

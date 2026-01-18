namespace Core.Service
{
    public interface ICupom
    {
        void Edit(Cupom cupom);
        uint Create(Cupom cupom);
        Cupom Get(int id);
        void Delete(int id);
        IEnumerable<Cupom> GetAll();
        IEnumerable<Cupom> GetByName(string nome);
    }
}
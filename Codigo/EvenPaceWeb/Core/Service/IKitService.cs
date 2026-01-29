namespace Core.Service
{
    public interface IKitService
    {
        void Edit(Kit kit);
        int Create(Kit kit);
        Kit Get(int id);
        void Delete(int id);
        IEnumerable<Kit> GetAll();
        IEnumerable<Kit> GetByName(string nome);
        IEnumerable<Kit> GetKitsPorEvento(int id);

    }
}

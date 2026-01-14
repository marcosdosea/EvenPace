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
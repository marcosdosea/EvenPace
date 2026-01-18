namespace Core.Service
{
    public interface IKits
    {
        void Edit(Kit kit);
        uint Create(Kit kit);
        Kit Get(int id);
        void Delete(int id);
    }
}
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
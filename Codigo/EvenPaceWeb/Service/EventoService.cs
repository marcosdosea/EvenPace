using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class EventoService : IEventosService
    {
        private readonly EvenPaceContext _context;
        public EventoService(EvenPaceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Persiste as informações de um novo evento no banco de dados.
        /// </summary>
        /// <param name="eventos">Objeto de domínio representando o evento a ser salvo.</param>
        /// <returns>Retorna o identificador (ID) gerado para o evento inserido.</returns>
        public int Create(Evento eventos)
        {
            _context.Add(eventos);
            _context.SaveChanges();
            return eventos.Id;
        }

        /// <summary>
        /// Atualiza o registro de um evento existente com os novos dados fornecidos, contornando problemas de rastreamento de entidade (tracking).
        /// </summary>
        /// <param name="eventoEditado">Objeto de evento com os dados modificados que substituirão os antigos.</param>
        public void Edit(Evento eventoEditado)
        {
            if (eventoEditado is not null)
            {
                var local = _context.Set<Evento>()
                    .Local
                    .FirstOrDefault(entry => entry.Id.Equals(eventoEditado.Id));

                if (local != null)
                {
                    _context.Entry(local).State = EntityState.Detached;
                }

                _context.Entry(eventoEditado).State = EntityState.Modified;

                _context.SaveChanges();
            }
        }
        /// <summary>
        /// Remove definitivamente um evento da base de dados.
        /// </summary>
        /// <param name="id">A chave primária do evento a ser deletado.</param>
        public void Delete(int id)
        {
            var _evento = _context.Eventos.Find(id);
            if (_evento is not null)
            {
                _context.Remove(_evento);
                _context.SaveChanges();
            }
        }
        /// <summary>
        /// Recupera os dados de um evento específico sem aplicar rastreamento (AsNoTracking) para otimizar operações de leitura.
        /// </summary>
        /// <param name="id">Identificador exclusivo do evento procurado.</param>
        /// <returns>A entidade de evento correspondente ao identificador, ou nulo se não existir.</returns>
        public Evento Get(int id)
        {
            return _context.Eventos.AsNoTracking().FirstOrDefault(e => e.Id == id);
        }
        /// <summary>
        /// Obtém a listagem completa de todos os eventos registrados no sistema.
        /// </summary>
        /// <returns>Uma coleção do tipo IEnumerable contendo os eventos.</returns>
        public IEnumerable<Evento> GetAll()
        {
            return _context.Eventos.AsNoTracking();
        }
        /// <summary>
        /// Localiza eventos que contenham um fragmento de texto em seus nomes.
        /// </summary>
        /// <param name="nome">Termo ou trecho do nome do evento usado como filtro.</param>
        /// <returns>Uma lista de eventos que correspondem ao critério de busca.</returns>
        public IEnumerable<Evento> GetByName(string nome)
        {
            return _context.Eventos.Where(e => e.Nome.Contains(nome)).AsNoTracking();
        }
    }
}
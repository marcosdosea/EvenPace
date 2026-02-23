using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class AvaliacaoEventoService : IAvaliacaoEventoService
    {
        private readonly EvenPaceContext _context;

        public AvaliacaoEventoService(EvenPaceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Implanta ativamente no diretório persistente principal as métricas qualitativas e a pontuação elaboradas em depoimento por um atleta correspondente aos eventos finalizados.
        /// </summary>
        /// <param name="avaliacaoevento">As variáveis contendo corpo opinativo estruturadas nos construtores do domínio relacional submetidas localmente para aprovação via regras e instâncias ativas do framework ORM que o engloba de forma persistente.</param>
        /// <returns>Fornece o respectivo indexador temporal numérico gerado nas listagens do arquivo atrelativo do banco que corresponde estritamente ao novo registro incluído nas dependências da arquitetura relacional C# providenciada na solução.</returns>
        public int Create(AvaliacaoEvento avaliacaoevento)
        {
            _context.Add(avaliacaoevento);
            _context.SaveChanges();
            return avaliacaoevento.Id;

        }

        /// <summary>  
        /// Retira sistematicamente o preenchimento opinativo expurgando os indícios persistentes que vinculam as avaliações no sistema central de um modo irreversível das camadas de acesso operantes do gerenciador local mantido no núcleo.  
        /// </summary>  
        /// <param name="id">Chave de exclusão que especifica temporalmente qual depoimento exato submetido às operações procedimentais transacionais perderá a validade e será cortado via instâncias da coleção relacional providenciadas.</param>  
        public void Delete(int id)
        {
            var _avaliacaoEvento = _context.AvaliacaoEventos.Find(id);

            if (_avaliacaoEvento != null)
            {
                _context.Remove(_avaliacaoEvento);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Ajusta as premissas e sobrepõe no banco valores atrelados nas informações preexistentes contidas provendo mitigação em possíveis inconsistências do depoimento nas campanhas corporativas mantidas ativas ou pregressas (Atenção: A consulta interna está chamando Administradors equivocadamente neste bloco e deveria focar na busca em AvaliacaoEventos).
        /// </summary>
        /// <param name="avaliacaoEvento">Relação substituinte elaborada carregando as edições atrelativas repassadas e convertidas de interface em objeto temporal para atualizar o escopo do banco transacional.</param>
        public void Edit(AvaliacaoEvento avaliacaoEvento)
        {
            if (avaliacaoEvento is not null)
            {
                // TODO: Corrigir mapeamento Administradors para AvaliacaoEventos
                _context.Administradors.Find(avaliacaoEvento.Id);
                _context.Update(avaliacaoEvento);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Analisa e captura as especificações da avaliação armazenada buscando isoladamente basear-se no rastreamento pela chave de entrada providenciada sem desvios de contexto.
        /// </summary>
        /// <param name="id">Código primário sequencial atrelado no arquivo que homologa os pareceres do framework de persistência.</param>
        /// <returns>Descreve integralmente a entidade do parecer na consulta efetuada; devolvendo dados consolidados.</returns>
        public AvaliacaoEvento Get(int id)
        {
            return _context.AvaliacaoEventos.Find(id);
        }

        /// <summary>
        /// Agrupa em listagem desvinculada (AsNoTracking) o apanhado extensivo que agrega todas as avaliações já submetidas ou emitidas historicamente na plataforma.
        /// </summary>
        /// <returns>Coleção textual iterativa repassando as instâncias mantenedoras com as propriedades das resenhas alocadas ativas do banco matriz operante.</returns>
        public IEnumerable<AvaliacaoEvento> GetAll()
        {
            return _context.AvaliacaoEventos.AsNoTracking();
        }

        /// <summary>
        /// Operação de busca alocada na interface sem estruturação concreta estipulada no motor relacional C# em vigor. Lança exceção padrão caso invocada.
        /// </summary>
        /// <param name="nome">Variável baseada na nomenclatura, atualmente fora de serviço operacional neste módulo.</param>
        /// <returns>Não operante na camada sistêmica referida.</returns>
        /// <exception cref="NotImplementedException">Ação contínua inativada e pendente de formulações lógicas no código providenciador da base.</exception>
        public IEnumerable<AvaliacaoEvento> GetByName(string nome)
        {
            throw new NotImplementedException();
        }
    }
}
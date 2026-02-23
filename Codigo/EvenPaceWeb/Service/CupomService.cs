using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class CupomService : ICupomService
    {
        private readonly EvenPaceContext _context;

        public CupomService(EvenPaceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Aciona e realiza efetivamente no sistema local ou nuvem transacional a alocação de atributos persistentes correspondendo as variáveis do objeto processado, inserindo suas informações atreladas de código temporal nos relatórios operacionais do banco ativo de campanhas em voga.
        /// </summary>
        /// <param name="cupom">Entidade já estipulada em memória proveniente das execuções do criador contendo o formato do benefício submetido, pronto a assinar a validação corporativa sistêmica do núcleo persistente de bônus ativos das competições integradas do framework principal.</param>
        /// <returns>Retorna a respectiva numeração contida da identificação provida das tabelas autoincrementais relativas à nova inclusão da diretriz promocional injetada no banco unificado correspondente às ações do contexto inserido pelo gestor administrativo em comando logístico atual do evento esportivo parametrizado na operação do método e retornado em variáveis inteiras provendo suporte nas consultas de rastreabilidade a vir à tona nos próximos registros operantes requeridos.</returns>
        public int Create(Cupom cupom)
        {
            cupom.Id = 0;
            _context.Add(cupom);
            _context.SaveChanges();
            return cupom.Id;
        }

        /// <summary>
        /// Submete ao motor do Entity Framework Core a modificação do estado relacional e repassa às transações os valores pontuados na interface que alteram validade temporal ou pecuniária de um recurso preexistente sem destruir as premissas primárias criadas nas operações passadas de bônus providenciados.
        /// </summary>
        /// <param name="cupom">Registro do desconto contendo substituições aplicadas nas chaves permitidas e submetidas para aprovação com modificações confirmadas no escopo local temporal das camadas sistêmicas e visuais.</param>
        public void Edit(Cupom cupom)
        {
            _context.Update(cupom);
            _context.SaveChanges();
        }

        /// <summary>
        /// Executa e remove definitivamente o componente relacional do cupom a ser retirado de validações para novas atribuições na aplicação com as lógicas ativadas para encontrar a devida referencialidade sem falhas através de uma busca primária antecessora com o método interno fornecido pelas instâncias ativas do framework ORM que o engloba de forma persistente.
        /// </summary>
        /// <param name="id">Chave de registro rastreadora numérica que referencia irrestritamente a identidade de bônus a ser expurgada no arquivo mestre central do software mantenedor corporativo relacional interconectado às organizações presentes nas configurações sistêmicas mantenedoras em operações gerais estipuladas localmente.</param>
        public void Delete(int id)
        {
            var cupom = _context.Cupoms.Find(id);

            _context.Remove(cupom);
            _context.SaveChanges();
        }

        /// <summary>
        /// Intercepta as informações diretas armazenadas no arquivo atrelativo corporativo fornecendo as particularidades ativas pertencentes do registro pesquisado isoladamente fornecido de parâmetro temporal.
        /// </summary>
        /// <param name="id">Referência em formato integral do bônus pesquisado.</param>
        /// <returns>Descreve a entidade processada após o percorrimento da chave nas relações operantes em bancos ou relatórios se devidamente compatível e aprovado à existência ativa constatada empiricamente nos diretórios mantenedores do motor lógico e provê de regresso os valores do achado para acesso posterior em variáveis e atributos globais.</returns>
        /// <exception cref="ServiceException">Desvia as respostas em modo reativo fornecendo a justificativa sistêmica de não encontrarem nenhuma aderência informacional ou de chaves na consulta realizada nos bancos relacionais, prevenindo interrupções globais críticas na aplicação corporativa de serviços logísticos que sustentam as campanhas das provas e competições em andamento no portal estipulado do organizador operante com o contexto gerador.</exception>
        public Cupom Get(int id)
        {
            return _context.Cupoms.FirstOrDefault(cupom => cupom.Id == id)
            ?? throw new ServiceException("Cupom não encontrado");
        }

        /// <summary>
        /// Varre e devolve todos os cupons listados em bancos inativando as verificações de estado (tracking) do servidor relacional provendo otimização de velocidade na leitura em abas extensas contendo o arquivo de rastreamento das submissões de descontos já geradas do banco matriz de registros dos provedores atrelativos logísticos de competições corporativas cadastradas ativas e inativas no repositório geral de atributos globais.
        /// </summary>
        /// <returns>Array unificador dos atributos que descrevem o catálogo estipulado dos cupons cadastrados até o momento atual nas premissas mantenedoras do core com listagem iterativa desvinculada de amarras lógicas restritivas e livres para preenchimento providenciado e iterativo nas estruturas requerentes dos formulários visuais alocados do desenvolvedor requisitante logado operante no ato processual estipulado e acessado pelas instâncias do ORM do C# relacional providenciado na arquitetura estipuladora base da solução.</returns>
        public IEnumerable<Cupom> GetAll()
        {
            return _context.Cupoms.AsNoTracking();
        }

        /// <summary>
        /// Fornece as validações baseadas em recortes e fragmentos da titulação atrelada do nome descritivo do pacote de descontos ou palavras operantes pesquisadas como restritivas na tabela mestre e devolvidas filtradas no catálogo de maneira inerte e passiva (AsNoTracking) à memória e às regras mantenedoras relacionais estipuladas localmente provendo leveza e acurácia corporativas informativas nas buscas com suporte total.
        /// </summary>
        /// <param name="nome">Expressão atrelativa submetida em campo textual informando trechos que obrigatoriamente farão parte construtiva da matriz descritiva nominativa nas linhas do banco a serem extraídas contidas da busca operante ativada pelo utilizador pesquisador da entidade referida no portal configurador de bônus mantido no núcleo.</param>
        /// <returns>Descreve um subconjunto interativo relacional que preenche e engloba o requisito informacional do texto validando a listagem passível restritiva às expressões pesquisadas como verdadeiras entre todas as possibilidades disponíveis em repositórios gerais de acesso do componente mantenedor providenciador e gerenciador dos bônus integrados no motor relacional C# atrelativo às premissas lógicas sistêmicas em vigor.</returns>
        public IEnumerable<Cupom> GetByName(string nome)
        {
            return _context.Cupoms.Where(c => c.Nome.Contains(nome)).AsNoTracking();
        }
    }
}
using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class CartaoCreditoService : ICartaoCreditoService
    {
        private readonly EvenPaceContext _context;

        public CartaoCreditoService(EvenPaceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Cria o elo transacional e incorpora metodicamente os registros sistêmicos contendo as métricas validadas atrelativas das chaves e senhas cifradas de um cartão de crédito no banco geral mantenedor do projeto.
        /// </summary>
        /// <param name="cartaoCredito">Componentes ativados formulando a entidade metodológica providenciada no processador submetido no formato da rede.</param>
        /// <returns>Identidade numérica primária fornecida referente à estipulação logada de cartões atrelativos.</returns>
        public int Create(CartaoCredito cartaoCredito)
        {
            _context.Add(cartaoCredito);
            _context.SaveChanges();
            return cartaoCredito.Id;
        }

        /// <summary>
        /// Modifica as restrições e premissas cadastradas previamente no cartão submetendo-as ao servidor relacional sem destruir integrações passadas.
        /// </summary>
        /// <param name="cartaoCredito">Estrutura substituinte editada e aprovada perante o validador submetido no contexto local contendo o Id de referência temporal para sobreposição ao antigo cartão correspondente no mercado de transações ativadas na operação do método e retornado logístico corporativo relacional.</param>
        public void Edit(CartaoCredito cartaoCredito)
        {
            if (cartaoCredito is not null)
            {
                _context.CartaoCreditos.Find(cartaoCredito.Id);
                _context.Update(cartaoCredito);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Expulsa o instrumento de transação da contabilidade da base de operações interrompendo vínculos perenes nas estruturas persistentes dos usuários através da submissão explícita baseada num código unicamente estabelecido a interceptar no ato da identificação.
        /// </summary>
        /// <param name="id">Chave de acompanhamento rastreadora estipulada na estrutura em repouso da relação de exclusão.</param>
        public void Delete(int id)
        {
            var cartao = _context.CartaoCreditos.Find(id);

            if (cartao is not null)
            {
                _context.Remove(cartao);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Invoca a captura de credenciais ativas do diretório unificado atestando referencialidade precisa para a chave enviada nos comandos do serviço logístico.
        /// </summary>
        /// <param name="id">Variável exata contendo as diretrizes requerentes do índice em SQL.</param>
        /// <returns>Descreve a configuração de pagamento extraída; resultando em nulo em ocasiões incertas sem a aderência informacional ou de chaves na base de registros.</returns>
        public CartaoCredito Get(int id)
        {
            return _context.CartaoCreditos.Find(id);
        }

        /// <summary>
        /// Engloba iterativamente as listagens relativas de perfis e extrai passivamente o conjunto integral e absoluto sem restrições ou travas no contexto para fornecer agilidade transacional (AsNoTracking).
        /// </summary>
        /// <returns>Arranjo preenchido enumerando todo o parque de metodologias financeiras cartões vinculados nos domínios atrelativos corporativos estipulados de antemão.</returns>
        public IEnumerable<CartaoCredito> GetAll()
        {
            return _context.CartaoCreditos.AsNoTracking();
        }
    }
}
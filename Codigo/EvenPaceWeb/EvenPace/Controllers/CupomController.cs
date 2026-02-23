using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Core.Service;
using Models;

namespace EvenPaceWeb.Controllers
{
    public class CupomController : Controller
    {
        private readonly ICupomService _cupomService;
        private readonly IMapper _mapper;

        public CupomController(ICupomService cupomService, IMapper mapper)
        {
            _cupomService = cupomService;
            _mapper = mapper;
        }

        /// <summary>
        /// Aciona a visualização englobadora extraindo a relação sequencial provinda do banco de registros detalhando todas as parametrizações geradas ativas e inativas dos bônus transacionais de cupom listados pela plataforma organizacional.
        /// </summary>
        /// <returns>Disponibiliza um ViewModel relacional listando e compondo o formulário visual com as particularidades de cada código atrelado no hub principal do gerenciador.</returns>
        public ActionResult Index()
        {
            var cupons = _cupomService.GetAll();
            var cupomViewModels = _mapper.Map<List<CupomViewModel>>(cupons);
            return View(cupomViewModels);
        }

        /// <summary>
        /// Destrincha as informações e particularidades aplicadas exclusivamente em um dos descontos da base para que possam ser checadas pelo seu gestor correspondente através das telas de apoio secundárias da requisição HTTP processada no model MVC.
        /// </summary>
        /// <param name="id">Código primário sequencial correspondente às propriedades atrelativas do cupom em questão nas listas SQL relacionais.</param>
        /// <returns>Página informacional exibindo regras transacionais de forma detalhada e concisa no formato do design imposto à view sem que edições sejam geradas de antemão.</returns>
        public ActionResult Details(int id)
        {
            var cupom = _cupomService.Get((int)id);
            var cupomViewModel = _mapper.Map<CupomViewModel>(cupom);
            return View(cupomViewModel);
        }

        /// <summary>
        /// Transita o responsável direto pela gestão administrativa corporativa para o portal isolado em que os bônus podem vir à luz do desenvolvimento no formato visual, desatrelado aos bancos e sem interferências de lógicas preexistentes.
        /// </summary>
        /// <returns>Exposição imediata no método GET com espaço integral reservado e livre destinado à geração estipulada em prol de vantagens para inscrições por corredores associados.</returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Atua efetuando o repasse e armazenamento em infraestrutura persistente de um desconto criado através das especificações de percentual e código da janela visual e enviando o repasse às instâncias apropriadas com a tratativa de vulnerabilidades e erros associados ao envio HTTP.
        /// </summary>
        /// <param name="cupomViewModel">Mapeamento embutindo instâncias dos valores estipulados via submit submetido pela interface interacional da view.</param>
        /// <returns>Promove o reenquadramento ao catálogo ou, detectando anomalia sintática providenciada pelos requerimentos em tela não satisfeitos perante as exigências do ModelState, reescreve os erros in-loco da janela visual do emissor de criação abortado temporariamente.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CupomViewModel cupomViewModel)
        {
            if (ModelState.IsValid)
            {
                var cupom = _mapper.Map<Core.Cupom>(cupomViewModel);
                _cupomService.Create(cupom);
                return RedirectToAction(nameof(Index));
            }

            return View(cupomViewModel);
        }

        /// <summary>
        /// Encaixa uma requisição em forma de POST englobando e garantindo que eventuais disparidades não afetem a integridade e, se aprovadas sem contratempos transacionais pelo validador submetido no formato da rede e de estado de modelo providenciado via C#, procedem em substituição irrestrita atrelativa aos antigos ditames de regra do cupom já vigente no mercado e nas campanhas de publicidade do gestor logado no sistema unificado corporativo de bônus ativados aos atletas credenciados na aplicação online globalizadora de inscrições promocionais baseados no design system adotado.
        /// </summary>
        /// <param name="cupomViewModel">Mecanismos encapsuladores englobados pela edição já finalizada na estrutura temporal HTML da página requisitadora no momento logístico operado em que o trigger envia ao framework de submissões das requisições geradoras baseadas em Modelos ASP.NET.</param>
        /// <returns>Transição de regresso para a home dos ingressos confirmando as modificações promovidas.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CupomViewModel cupomViewModel)
        {
            if (ModelState.IsValid)
            {
                var cupom = _mapper.Map<Core.Cupom>(cupomViewModel);
                _cupomService.Edit(cupom);
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Realiza a interpolação preliminar atrelada aos serviços em submissões das propriedades de modelo preexistente, permitindo que a edição e a leitura das lógicas passadas ocorra sem bloqueios para repopular os campos editáveis.
        /// </summary>
        /// <param name="id">Índice numérico exclusivo a ser procurado e transportado aos mecanismos submetidos.</param>
        /// <returns>Interface visual preenchida propícia para edições pontuais do gestor logado e do item alocado.</returns>
        public ActionResult Edit(int id)
        {
            var cupom = _cupomService.Get((int)id);

            var cupomViewModel = _mapper.Map<CupomViewModel>(cupom);
            return View(cupomViewModel);
        }

        /// <summary>
        /// Solicita a preclusão dos registros fornecendo ao criador logístico uma janela de validação das intenções focadas e estritamente dedicadas a interromper a validade sistêmica do registro e atestar que a exclusão é assertiva daquele desconto.
        /// </summary>
        /// <param name="id">Dado pontual contendo rastreio referencial provido perante a arquitetura de acesso SQL das estruturas internas.</param>
        /// <returns>Visualização que sumariza as dependências em modo reativo e fornece a decisão ao responsável e confirmador administrativo do bônus extinto.</returns>
        public ActionResult Delete(int id)
        {
            var cupom = _cupomService.Get((int)id);

            var cupomViewModel = _mapper.Map<CupomViewModel>(cupom);
            return View(cupomViewModel);
        }

        /// <summary>
        /// Confirma operacionalmente e conclui de modo assertivo sem margens temporais para volta atrelativa os parâmetros das transações designadas com a extinção persistente e incontestável do arquivo e suas dependências sem afetar as lógicas base com as tratativas via formulário restrito do protocolo transacional do Anti Forgery.
        /// </summary>
        /// <param name="id">Variável atrelativa apontada via sistema provendo acesso irrestrito às execuções procedimentais designadas com o ato de submissão restritiva dos recursos de exclusão em banco da chave principal informada de tal atributo operado internamente pela ferramenta integrativa do núcleo.</param>
        /// <param name="cupomViewModel">Engloba os atributos do modelo repassado de bônus, utilizado sem aprofundamentos lógicos atrelativos ao código direto, e sim em conjunção dos mecanismos do framework das classes visuais de manipulação intermediária.</param>
        /// <returns>Desvia a exibição temporal do utilizador retornando com fluxo processado em reescrita imediata no índice do repositório contendo todas as peças válidas que o circundavam antecipadamente antes de ter sido processado em apagamento lógico generalizado.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, CupomViewModel cupomViewModel)
        {
            _cupomService.Delete((int)id);
            return RedirectToAction(nameof(Index));
        }
    }
}
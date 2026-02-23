using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Core.Service;
using Models;
using System.Threading.Tasks;

namespace EvenPaceWeb.Controllers
{
    public class OrganizacaoController : Controller
    {
        private readonly IOrganizacaoService _organizacaoService;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService; 

        public OrganizacaoController(
            IOrganizacaoService organizacaoService,
            IMapper mapper,
            IAuthService authService) 
        {
            _organizacaoService = organizacaoService;
            _mapper = mapper;
            _authService = authService;
        }

        /// <summary>
        /// Fornece a interface visual para a autenticação do perfil da Organização.
        /// </summary>
        /// <returns>View contendo o formulário de login empresarial.</returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Valida as credenciais da organização e delega a autenticação ao serviço centralizado.
        /// </summary>
        /// <param name="email">E-mail corporativo de acesso.</param>
        /// <param name="senha">Senha secreta de acesso.</param>
        /// <returns>Redireciona para o painel gerencial de eventos em caso de sucesso, ou retorna aviso de erro.</returns>
        [HttpPost]
        public async Task<IActionResult> Login(string email, string senha)
        {
            var isAutenticado = await _authService.LoginAsync(email, senha);

            if (isAutenticado)
            {
                return RedirectToAction("Index", "Evento");
            }

            ModelState.AddModelError("", "Credenciais da organização inválidas");
            return View();
        }
        /// <summary>
        /// Compila e exibe de maneira geral o resumo catalogado com as contas de todas as organizações inscritas.
        /// </summary>
        /// <returns>A página geradora renderizada com os resultados contidos na listagem visual.</returns>
        public ActionResult Index()
        {
            var organizacoes = _organizacaoService.GetAll();
            var organizacaoViewModels = _mapper.Map<List<OrganizacaoViewModel>>(organizacoes);
            return View(organizacaoViewModels);
        }

        /// <summary>
        /// Elabora um carregamento descritivo minucioso focando em disponibilizar transparência informativa referente aos dados cadastrais de uma organização.
        /// </summary>
        /// <param name="id">Valor determinante para localizar a entidade isoladamente.</param>
        /// <returns>Interface estendendo o objeto do sistema traduzido em painel.</returns>
        public ActionResult Details(int id)
        {
            var organizacao = _organizacaoService.Get((int)id);
            var organizacaoViewModel = _mapper.Map<OrganizacaoViewModel>(organizacao);
            return View(organizacaoViewModel);
        }

        /// <summary>
        /// Estrutura e retorna o formulário destinado à criação de uma conta corporativa (Organização).
        /// </summary>
        /// <returns>Página base de registros.</returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Submete os dados essenciais referentes ao cadastro de organização nova após triagem no model state.
        /// </summary>
        /// <param name="organizacaoViewModel">Mapeamento direto da classe representacional da interface.</param>
        /// <returns>Redireciona à grade de listagens em formato de retorno aprovado ou retrocede apontando inconformidades.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrganizacaoViewModel organizacaoViewModel)
        {
            if (ModelState.IsValid)
            {
                var organizacao = _mapper.Map<Core.Organizacao>(organizacaoViewModel);
                _organizacaoService.Create(organizacao);
                return RedirectToAction(nameof(Index));
            }
            return View(organizacaoViewModel);
        }

        /// <summary>
        /// Invoca as credenciais em posse do banco de informações para alocar um painel interativo de edição da Organização.
        /// </summary>
        /// <param name="id">Chave que valida as credenciais relativas à estrutura requisitada.</param>
        /// <returns>View de repopulação textual propícia a edições pontuais do gestor logado.</returns>
        public ActionResult Edit(int id)
        {
            var organizacao = _organizacaoService.Get((int)id);
            var organizacaoViewModel = _mapper.Map<OrganizacaoViewModel>(organizacao);
            return View(organizacaoViewModel);
        }

        /// <summary>
        /// Avaliza as tratativas editáveis promovidas nas regras da conta da organização.
        /// </summary>
        /// <param name="organizacaoViewModel">Agrupamento pós edição provindo das rotinas estruturadas na view.</param>
        /// <returns>Redireciona visando a central de configurações principais se houver êxito.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrganizacaoViewModel organizacaoViewModel)
        {
            organizacaoViewModel.Id = 3;

            ModelState.Remove("Senha");

            if (ModelState.IsValid)
            {
                var organizacao = _mapper.Map<Core.Organizacao>(organizacaoViewModel);
                _organizacaoService.Edit(organizacao);

                return RedirectToAction(nameof(Index));
            }
            return View(organizacaoViewModel);
        }

        /// <summary>
        /// Aciona e preenche o espaço visual destinado a atestar as confirmações decisivas antes de realizar os descartes do registro da organização.
        /// </summary>
        /// <param name="id">Código primário contendo a entidade pré-aprovada aos filtros.</param>
        /// <returns>Formulário de preenchimento voltado à inativação contendo informações consolidadas da conta.</returns>
        public ActionResult Delete(int id)
        {
            var organizacao = _organizacaoService.Get((int)id);
            var organizacaoViewModel = _mapper.Map<OrganizacaoViewModel>(organizacao);
            return View(organizacaoViewModel);
        }

        /// <summary>
        /// Executa a eliminação total da conta da organização nas tratativas integradas do banco principal.
        /// </summary>
        /// <param name="id">Indexador que aponta à estrutura extinta referenciada na transação.</param>
        /// <param name="organizacaoViewModel">Condução sistêmica que auxilia nas diretrizes internas das requisições via ASP.NET MVC model-binding.</param>
        /// <returns>O utilizador é realocado ao ponto inicial da navegação com uma reescrita dos estados transicionais.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, OrganizacaoViewModel organizacaoViewModel)
        {
            _organizacaoService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Core.Service;
using Models;
using EvenPaceWeb.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace EvenPaceWeb.Controllers
{
    public class OrganizacaoController : Controller
    {
        private readonly IOrganizacaoService _organizacaoService;
        private readonly IMapper _mapper;
        private readonly UserManager<UsuarioIdentity> _userManager;
        private readonly SignInManager<UsuarioIdentity> _signInManager;

        public OrganizacaoController(IOrganizacaoService organizacaoService, IMapper mapper, UserManager<UsuarioIdentity> userManager,
            SignInManager<UsuarioIdentity> signInManager)
        {
            _organizacaoService = organizacaoService;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Disponibiliza a interface visual (formulário) para a autenticação da organização no sistema.
        /// </summary>
        /// <returns>A página contendo os campos de login para CPF/CNPJ e Senha.</returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Recebe as credenciais, higieniza a formatação do documento informado e tenta validar a autenticação da organização junto ao banco do Identity.
        /// </summary>
        /// <param name="documento">CPF ou CNPJ da organização informados na tela de login.</param>
        /// <param name="senha">A credencial secreta correspondente à conta.</param>
        /// <returns>Em caso de sucesso, redireciona para a página principal. Em caso de falha, recarrega a página apontando o erro estrutural de credenciais.</returns>
        [HttpPost]
        public async Task<IActionResult> Login(string documento, string senha)
        {
            if (string.IsNullOrEmpty(documento) || string.IsNullOrEmpty(senha))
            {
                ModelState.AddModelError("", "Preencha o CPF/CNPJ e a senha.");
                return View();
            }

            documento = documento.Replace(".", "").Replace("-", "").Replace("/", "");

            var result = await _signInManager.PasswordSignInAsync(
                documento,
                senha,
                false,
                false
            );

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "CPF/CNPJ ou Senha inválidos");
            return View();
        }

        /// <summary>
        /// Compila e exibe de maneira geral o resumo catalogado com as contas de todas as organizações inscritas e regularizadas nos bancos de registros.
        /// </summary>
        /// <returns>A página geradora renderizada com os resultados contidos na listagem visual.</returns>
        public ActionResult Index()
        {
            var organizacoes = _organizacaoService.GetAll();
            var organizacaoViewModels = _mapper.Map<List<OrganizacaoViewModel>>(organizacoes);
            return View(organizacaoViewModels);
        }

        /// <summary>
        /// Elabora um carregamento descritivo minucioso focado em disponibilizar transparência informativa referente aos dados cadastrais de uma organização.
        /// </summary>
        /// <param name="id">Valor determinante para localizar uma entidade isoladamente com fins consultivos.</param>
        /// <returns>Interface estendendo o objeto do sistema traduzido em painel.</returns>
        public ActionResult Details(int id)
        {
            var organizacao = _organizacaoService.Get((int)id);
            var organizacaoViewModel = _mapper.Map<OrganizacaoViewModel>(organizacao);
            return View(organizacaoViewModel);
        }

        /// <summary>
        /// Estrutura e retorna as prerrogativas de formulário sem formatações residuais destinadas à criação limpa de uma conta/organização corporativa recente.
        /// </summary>
        /// <returns>Página base de registros.</returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Submete a finalização das pendências visuais preenchidas, criando simultaneamente a credencial de acesso do administrador no Identity e os dados cadastrais da organização na base do sistema.
        /// </summary>
        /// <param name="organizacaoViewModel">Mapeamento direto traduzido pelo processo construtivo da classe representacional.</param>
        /// <param name="senha">Senha de acesso em texto plano informada na View de cadastro para liberação de login futuro.</param>
        /// <returns>Redireciona à grade de listagens em formato de retorno aprovado ou retrocede ao próprio quadro referenciando inconformidades nos parâmetros propostos.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(OrganizacaoViewModel organizacaoViewModel, string senha)
        {
            if (ModelState.IsValid)
            {
                var organizacao = _mapper.Map<Core.Organizacao>(organizacaoViewModel);

                string username = string.Empty;

                if (!string.IsNullOrEmpty(organizacao.Cnpj))
                {
                    username = organizacao.Cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
                }
                else if (!string.IsNullOrEmpty(organizacao.Cpf))
                {
                    username = organizacao.Cpf.Replace(".", "").Replace("-", "");
                }
                else
                {
                    ModelState.AddModelError("", "É obrigatório informar um CPF ou CNPJ.");
                    return View(organizacaoViewModel);
                }

                if (string.IsNullOrEmpty(senha))
                {
                    ModelState.AddModelError("", "A senha é obrigatória para o cadastro.");
                    return View(organizacaoViewModel);
                }

                var identityUser = new UsuarioIdentity
                {
                    UserName = username
                };

                var result = await _userManager.CreateAsync(identityUser, senha);

                if (result.Succeeded)
                {
                    _organizacaoService.Create(organizacao);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(organizacaoViewModel);
        }

        /// <summary>
        /// Invoca as credenciais em posse do banco de informações para alocar um painel interativo que viabiliza alterações sistêmicas sobre um determinado CNPJ/Conta base de organizador.
        /// </summary>
        /// <param name="id">Cifra que valida as credenciais numéricas relativas à estrutura requisitada.</param>
        /// <returns>View de repopulação textual propícia a edições pontuais do gestor logado.</returns>
        public ActionResult Edit(int id)
        {
            var organizacao = _organizacaoService.Get((int)id);
            var organizacaoViewModel = _mapper.Map<OrganizacaoViewModel>(organizacao);
            return View(organizacaoViewModel);
        }

        /// <summary>
        /// Avaliza as tratativas editáveis promovidas nas regras da conta da organização, validando as informações enquanto exclui da triagem parâmetros sensíveis e pontuais, como a senha do administrador principal.
        /// </summary>
        /// <param name="organizacaoViewModel">Agrupamento pós edição provindo das rotinas estruturadas no preenchimento de view models.</param>
        /// <returns>Rotaciona o trânsito da requisição visando a central de configurações principais se houver êxito no preenchimento de integridade.</returns>
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
        /// Aciona e preenche o espaço visual destinado e reservado a atestar as confirmações decisivas antes de realizar os descartes do registro da organização.
        /// </summary>
        /// <param name="id">Código primário contendo a entidade pré-aprovada aos filtros.</param>
        /// <returns>Formulário de preenchimento voltado à inativação contendo informações consolidadas da conta que perderá o vínculo com o programa e o sistema vigente.</returns>
        public ActionResult Delete(int id)
        {
            var organizacao = _organizacaoService.Get((int)id);
            var organizacaoViewModel = _mapper.Map<OrganizacaoViewModel>(organizacao);
            return View(organizacaoViewModel);
        }

        /// <summary>
        /// Executa e expurga os indícios lógicos que relacionavam a organização logada nas tratativas integradas do bando de banco principal, promovendo uma eliminação total dos indícios pregressos em conjunto à validação Anti-forgery.
        /// </summary>
        /// <param name="id">Indexador que aponta à estrutura extinta referenciada na transação.</param>
        /// <param name="organizacaoViewModel">Condução sistêmica que auxilia nas diretrizes internas das requisições via ASP.NET MVC model-binding.</param>
        /// <returns>O usuário é destituído de suas ações e realocado ao ponto inicial da navegação com uma reescrita dos estados transicionais.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, OrganizacaoViewModel organizacaoViewModel)
        {
            _organizacaoService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
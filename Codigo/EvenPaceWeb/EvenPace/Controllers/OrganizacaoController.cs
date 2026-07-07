using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Core.Service;
using Models;
using EvenPaceWeb.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Service;

namespace EvenPaceWeb.Controllers
{
    public class OrganizacaoController : Controller
    {
        private readonly IOrganizacaoService _organizacaoService;
        private readonly IEventosService _eventoService; // 1. Adicionado o serviço de eventos para o painel
        private readonly IMapper _mapper;
        private readonly UserManager<UsuarioIdentity> _userManager;
        private readonly SignInManager<UsuarioIdentity> _signInManager;


        // Injete o IEventoService no construtor
        public OrganizacaoController(
            IOrganizacaoService organizacaoService,
            IEventosService eventoService,
            IMapper mapper,
            UserManager<UsuarioIdentity> userManager,
            SignInManager<UsuarioIdentity> signInManager)
        {
            _organizacaoService = organizacaoService;
            _eventoService = eventoService;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

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
            {
                return RedirectToAction("Index", "Evento");
            }

            ModelState.AddModelError("", "CPF/CNPJ ou Senha inválidos");
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userName = User.Identity?.Name; // CPF ou CNPJ usado no login

            // Busca a organização logada para capturar o Nome e o ID
            var organizacao = _organizacaoService.GetAll()
                .FirstOrDefault(o => o.Cpf == userName || o.Cnpj == userName);

            if (organizacao == null)
            {
                return NotFound("Organização não encontrada.");
            }

            ViewBag.NomeOrganizacao = organizacao.Nome;

            // Filtra os eventos vinculados ao ID desta organização
            var meusEventos = _eventoService.GetAll()
                .Where(e => e.IdOrganizacao == organizacao.Id)
                .ToList();

            return View(meusEventos);
        }

        public ActionResult Details(int id)
        {
            var organizacao = _organizacaoService.Get((int)id);
            var organizacaoViewModel = _mapper.Map<OrganizacaoViewModel>(organizacao);
            return View(organizacaoViewModel);
        }

        [HttpGet]
        public ActionResult Create(string documento = null)
        {
            var model = new OrganizacaoViewModel();
            if (!string.IsNullOrEmpty(documento))
            {
                var docLimpo = new string(documento.Where(char.IsDigit).ToArray());
                if (docLimpo.Length <= 11)
                    model.Cpf = docLimpo;
                else
                    model.Cnpj = docLimpo;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(OrganizacaoViewModel model, string tipoDocumento)
        {
            string documentoLimpo = "";

            if (tipoDocumento == "CPF")
            {
                model.Cnpj = null;
                ModelState.Remove("Cnpj");
                if (string.IsNullOrEmpty(model.Cpf))
                    ModelState.AddModelError("Cpf", "O CPF é obrigatório.");
                else
                    documentoLimpo = new string(model.Cpf.Where(char.IsDigit).ToArray());
            }
            else
            {
                model.Cpf = null;
                ModelState.Remove("Cpf");
                if (string.IsNullOrEmpty(model.Cnpj))
                    ModelState.AddModelError("Cnpj", "O CNPJ é obrigatório.");
                else
                    documentoLimpo = new string(model.Cnpj.Where(char.IsDigit).ToArray());
            }

            if (ModelState.IsValid)
            {
                // Variáveis de controle para sabermos se o usuário Identity foi salvo
                bool usuarioIdentityCriado = false;
                UsuarioIdentity novoUsuario = null;

                try
                {
                    model.Cep = new string(model.Cep?.Where(char.IsDigit).ToArray());
                    if (tipoDocumento == "CPF") model.Cpf = documentoLimpo;
                    if (tipoDocumento == "CNPJ") model.Cnpj = documentoLimpo;

                    novoUsuario = new UsuarioIdentity
                    {
                        UserName = documentoLimpo,
                        PhoneNumber = model.Telefone,
                        Email = model.Email,
                        NormalizedEmail = model.Email.ToUpper()
                    };

                    // 1. Tenta criar o usuário no Identity (Isso já salva no banco)
                    var identityResult = await _userManager.CreateAsync(novoUsuario, model.Senha);

                    if (identityResult.Succeeded)
                    {
                        usuarioIdentityCriado = true; // Marcamos que o usuário foi criado

                        await _userManager.AddToRoleAsync(novoUsuario, "Organizacao");

                        // 2. Tenta criar a Organização no seu banco de dados
                        var organizacao = _mapper.Map<Core.Organizacao>(model);
                        _organizacaoService.Create(organizacao);

                        await _signInManager.SignInAsync(novoUsuario, isPersistent: false);

                        return RedirectToAction("Index", "Evento");
                    }
                    else
                    {
                        foreach (var error in identityResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // ROLLBACK MANUAL: Se caiu aqui e o usuário tinha sido criado, nós apagamos ele.
                    if (usuarioIdentityCriado && novoUsuario != null)
                    {
                        await _userManager.DeleteAsync(novoUsuario);
                    }

                    // Captura o erro real retornado pelo banco de dados
                    var mensagemReal = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    ModelState.AddModelError("", "Erro ao salvar registros: " + mensagemReal);
                }
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var organizacao = _organizacaoService.Get((int)id);
            var organizacaoViewModel = _mapper.Map<OrganizacaoViewModel>(organizacao);
            return View(organizacaoViewModel);
        }

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

        public ActionResult Delete(int id)
        {
            var organizacao = _organizacaoService.Get((int)id);
            var organizacaoViewModel = _mapper.Map<OrganizacaoViewModel>(organizacao);
            return View(organizacaoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, OrganizacaoViewModel organizacaoViewModel)
        {
            _organizacaoService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
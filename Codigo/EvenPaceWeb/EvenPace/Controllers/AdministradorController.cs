using Microsoft.AspNetCore.Mvc;
using Core.Service;
using AutoMapper;
using Models;

namespace EvenPaceWeb.Controllers
{
    public class AdministradorController : Controller
    {
        private readonly IAdministradorService _administradorService;
        private readonly IMapper _mapper;

        public AdministradorController(IAdministradorService administradorService, IMapper mapper)
        {
            _administradorService = administradorService;
            _mapper = mapper;
        }

        /// <summary>
        /// Exibe a listagem completa de todos os perfis com privilégios de administração cadastrados no sistema.
        /// </summary>
        /// <returns>View contendo uma grade iterativa dos administradores mapeados.</returns>
        public ActionResult Index()
        {
            var administradores = _administradorService.GetAll();
            var administradorViewModels = _mapper.Map<List<AdministradorViewModel>>(administradores);
            return View(administradorViewModels);
        }

        /// <summary>
        /// Apresenta o painel detalhado de informações cadastrais e acessos pertencentes a um administrador específico.
        /// </summary>
        /// <param name="id">Chave numérica exclusiva do administrador a ser detalhado.</param>
        /// <returns>View com as informações isoladas do gestor.</returns>
        public ActionResult Details(int id)
        {
            var administrador = _administradorService.Get((int)id);
            var administradorViewModel = _mapper.Map<AdministradorViewModel>(administrador);
            return View(administradorViewModel);
        }

        /// <summary>
        /// Fornece o formulário em branco dedicado à inserção e criação de um novo perfil gestor.
        /// </summary>
        /// <returns>View limpa contendo os campos de registro.</returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Submete os dados preenchidos na interface para registrar e validar um novo perfil de administrador na plataforma.
        /// </summary>
        /// <param name="administradorViewModel">Objeto contendo os campos de criação preenchidos pelo requisitante.</param>
        /// <returns>Transita para a tela principal de listagem em caso afirmativo de validação.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AdministradorViewModel administradorViewModel)
        {
            if (ModelState.IsValid)
            {
                var administrador = _mapper.Map<Core.Administrador>(administradorViewModel);
                _administradorService.Create(administrador);
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Resgata o perfil existente do banco de dados e repopula o formulário para modificações operacionais ou ajustes cadastrais.
        /// </summary>
        /// <param name="id">Código primário do perfil alvo de modificação.</param>
        /// <returns>View com o modelo preenchido para edição de campos.</returns>
        public ActionResult Edit(int id)
        {
            var administrador = _administradorService.Get((int)id);
            var administradorViewModel = _mapper.Map<AdministradorViewModel>(administrador);
            return View(administradorViewModel);
        }

        /// <summary>
        /// Recebe a coleção de valores alterados da interface e consolida a sobrescrita do cadastro do administrador nas tabelas locais.
        /// </summary>
        /// <param name="id">Código indexador da conta gestora a ser substituída.</param>
        /// <param name="collection">Pacote genérico contendo a estrutura chave-valor submetida do front-end.</param>
        /// <returns>Redireciona para o catálogo primário atualizado.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                var administrador = _mapper.Map<Core.Administrador>(collection);
                _administradorService.Edit(administrador);
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Providencia uma interface confirmatória exibindo o resumo dos dados de um administrador que está prestes a ser inativado permanentemente.
        /// </summary>
        /// <param name="id">Identificador base do sujeito na tabela do banco relacional.</param>
        /// <returns>Página de retenção solicitando autorização final da destruição do registro.</returns>
        public ActionResult Delete(int id)
        {
            var administrador = _administradorService.Get((int)id);
            var administradorViewModel = _mapper.Map<AdministradorViewModel>(administrador);
            return View(administradorViewModel);
        }

        /// <summary>
        /// Promove a exclusão lógica ou física categórica do perfil de gestão referenciado, encerrando seu ciclo de vida dentro da arquitetura administrativa.
        /// </summary>
        /// <param name="id">Numeração de indexação para localizar exatamente a entidade a ser expurgada.</param>
        /// <param name="administradorViewModel">Pacote auxiliar submetido via modelo no momento do botão confirmatório.</param>
        /// <returns>Realoca a navegação visando o índice generalizado de gestores ativos remanescentes.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, AdministradorViewModel administradorViewModel)
        {
            _administradorService.Delete((int)id);
            return RedirectToAction(nameof(Index));
        }
    }
}
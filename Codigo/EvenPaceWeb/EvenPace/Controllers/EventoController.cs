using Microsoft.AspNetCore.Mvc;
using Core.Service;
using Core;
using Models;
using AutoMapper;


namespace EvenPace.Controllers
{
    public class EventoController : Controller
    {
        private readonly IEventosService _service;
        private readonly IKitService _kitService;
        private readonly IMapper _mapper;
        private readonly EvenPaceContext _context;

        public EventoController(
            IEventosService service,
            IKitService kitService,
            IMapper mapper,
            EvenPaceContext context)
        {
            _service = service;
            _kitService = kitService;
            _mapper = mapper;
            _context = context;
        }

        /// <summary>
        /// Exibe a listagem de eventos vinculados à organização logada, ordenados pela data mais recente.
        /// </summary>
        /// <returns>View contendo uma lista de modelos de visualização de eventos.</returns>
        public IActionResult Index()
        {
            int idOrganizacao = 1; 

            var listaEntidades = _service.GetAll()
                                         .Where(e => e.IdOrganizacao == idOrganizacao)
                                         .OrderByDescending(e => e.Data)
                                         .ToList();

            var listaViewModel = _mapper.Map<List<EventoViewModel>>(listaEntidades);

            return View(listaViewModel);
        }

        /// <summary>
        /// Apresenta os detalhes completos de um evento específico.
        /// </summary>
        /// <param name="id">Identificador único do evento a ser detalhado.</param>
        /// <returns>View de detalhes caso o evento exista; caso contrário, redireciona para a listagem principal.</returns>
        public IActionResult Details(int id)
        {
            var entidade = _service.Get(id);
            if (entidade == null) return RedirectToAction("Index");

            var viewModel = _mapper.Map<EventoViewModel>(entidade);

            ViewBag.IdEventoAtual = viewModel.Id;
            ViewBag.NomeCorrida = viewModel.Nome;

            return View(viewModel);
        }

        /// <summary>
        /// Carrega o formulário para o registro de um novo evento no sistema.
        /// </summary>
        /// <returns>View com um modelo de evento vazio.</returns>
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.TituloPagina = "Novo Evento";
            return View(new EventoViewModel());
        }

        /// <summary>
        /// Processa os dados submetidos pelo formulário para gravar um novo evento, incluindo o upload de imagem associada.
        /// </summary>
        /// <param name="model">Modelo de visualização contendo os dados do evento preenchidos pelo usuário.</param>
        /// <returns>Redireciona para a listagem em caso de sucesso ou retorna a View com erros de validação.</returns>
        /// <exception cref="Exception">Captura exceções gerais de conversão ou acesso a disco ao salvar a imagem, repassando para o ModelState.</exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EventoViewModel model)
        {
            ModelState.Remove("Imagem");
            ModelState.Remove("Data");

            if (ModelState.IsValid)
            {
                try
                {
                    var evento = _mapper.Map<Evento>(model);

                    if (model.DataOnly.HasValue && model.HoraOnly.HasValue)
                    {
                        evento.Data = model.DataOnly.Value.Add(model.HoraOnly.Value);
                    }

                    if (model.ImagemUpload != null)
                    {
                        evento.Imagem = SalvarImagemNoDisco(model.ImagemUpload);
                    }

                    if (evento.IdOrganizacao == 0) evento.IdOrganizacao = 1;

                    _service.Create(evento);
                    TempData["MensagemSucesso"] = "Evento criado com sucesso! 🏃‍♂️";

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erro: " + ex.Message);
                }
            }

            ViewBag.TituloPagina = "Novo Evento";
            return View(model);
        }

        /// <summary>
        /// Prepara e exibe o formulário de edição com os dados atuais de um evento existente.
        /// </summary>
        /// <param name="id">Identificador do evento que será modificado.</param>
        /// <returns>View com o formulário preenchido ou redirecionamento caso o evento não seja encontrado.</returns>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var entidade = _service.Get(id);
            if (entidade == null) return RedirectToAction("Index");

            var viewModel = _mapper.Map<EventoViewModel>(entidade);
            viewModel.DataOnly = entidade.Data.Date;
            viewModel.HoraOnly = entidade.Data.TimeOfDay;

            ViewBag.TituloPagina = "Editar Evento";
            return View(viewModel);
        }

        /// <summary>
        /// Aplica as alterações fornecidas pelo usuário a um evento previamente cadastrado, gerenciando também a substituição da imagem.
        /// </summary>
        /// <param name="id">Identificador único do evento a ser atualizado.</param>
        /// <param name="model">Objeto contendo as novas informações do evento.</param>
        /// <returns>Redireciona para a listagem em caso de sucesso ou retorna a View com erros em caso de falha.</returns>
        /// <exception cref="Exception">Captura falhas durante a persistência dos dados ou substituição do arquivo físico.</exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EventoViewModel model)
        {
            if (id != model.Id) return NotFound();

            ModelState.Remove("Imagem");
            ModelState.Remove("Data");

            if (ModelState.IsValid)
            {
                try
                {
                    var evento = _mapper.Map<Evento>(model);

                    if (model.DataOnly.HasValue && model.HoraOnly.HasValue)
                    {
                        evento.Data = model.DataOnly.Value.Add(model.HoraOnly.Value);
                    }

                    if (model.ImagemUpload != null)
                    {
                        if (!string.IsNullOrEmpty(model.Imagem))
                            DeletarImagemDoDisco(model.Imagem);

                        evento.Imagem = SalvarImagemNoDisco(model.ImagemUpload);
                    }
                    else
                    {
                        evento.Imagem = model.Imagem;
                    }

                    _service.Edit(evento);
                    TempData["MensagemSucesso"] = "Evento atualizado com sucesso! ✏️";

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erro: " + ex.Message);
                }
            }

            ViewBag.TituloPagina = "Editar Evento";
            return View(model);
        }

        /// <summary>
        /// Exclui um evento do sistema, bem como os kits associados e a respectiva imagem armazenada em disco.
        /// </summary>
        /// <param name="id">Identificador do evento a ser deletado.</param>
        /// <returns>Redireciona para a página principal de listagem de eventos.</returns>
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var evento = _service.Get(id);

            if (evento != null)
            {
                var kitsDoEvento = _kitService.GetAll().Where(k => k.IdEvento == id).ToList();
                foreach (var kit in kitsDoEvento)
                {
                    if (!string.IsNullOrEmpty(kit.Imagem)) DeletarImagemDoDisco(kit.Imagem);
                    _kitService.Delete(kit.Id);
                }

                if (!string.IsNullOrEmpty(evento.Imagem))
                {
                    DeletarImagemDoDisco(evento.Imagem);
                }

                _service.Delete(id);

                TempData["MensagemSucesso"] = "Evento excluído com sucesso! 🗑️";
            }

            return RedirectToAction("Index");
        }

        private string SalvarImagemNoDisco(Microsoft.AspNetCore.Http.IFormFile imagemUpload)
        {
            string pasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens");
            if (!Directory.Exists(pasta)) Directory.CreateDirectory(pasta);

            string nomeUnico = Guid.NewGuid().ToString() + "_" + imagemUpload.FileName;
            string caminhoCompleto = Path.Combine(pasta, nomeUnico);

            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                imagemUpload.CopyTo(stream);
            }

            return nomeUnico;
        }
        /// <summary>
        /// Exibe o catálogo geral de eventos disponíveis na plataforma para os usuários finais, permitindo buscas dinâmicas.
        /// </summary>
        /// <param name="search">Termo opcional de pesquisa para filtrar eventos pelo nome, cidade ou estado.</param>
        /// <returns>View contendo a vitrine de eventos compatíveis com a busca.</returns>
        [HttpGet]
        public IActionResult IndexUsuario(string search)
        {
            var eventos = _context.Eventos.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                eventos = eventos.Where(e =>
                    e.Nome.Contains(search) ||
                    e.Cidade.Contains(search) ||
                    e.Estado.Contains(search));
            }

            var model = eventos
                .Select(e => new EventoViewModel
                {
                    Id = (uint)e.Id,
                    Nome = e.Nome,
                    Data = e.Data,
                    Cidade = e.Cidade,
                    Estado = e.Estado,
                    Imagem = e.Imagem
                })
                .ToList();

            return View(model);
        }

        private void DeletarImagemDoDisco(string nomeImagem)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", nomeImagem);
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            }
            catch
            {
            }
        }
    }
}
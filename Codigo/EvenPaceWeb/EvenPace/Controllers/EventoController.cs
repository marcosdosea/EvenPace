using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Service;

namespace EvenPace.Controllers
{
    public class EventoController : Controller
    {
        private readonly IEventosService _service; // Este é o seu serviço principal
        private readonly IKitService _kitService;
        private readonly IMapper _mapper;
        private readonly EvenPaceContext _context;
        private readonly ICorredorService _corredorService;
        private object _logger;

        public EventoController(
            IEventosService service,
            IKitService kitService,
            IMapper mapper,
            EvenPaceContext context,
            ICorredorService corredorService)
        {
            _service = service;
            _kitService = kitService;
            _mapper = mapper;
            _context = context;
            _corredorService = corredorService;
        }

        [Authorize]
        public IActionResult Index()
        {
            // 1. Obtém o documento (CPF/CNPJ) do usuário logado
            var documentoSessao = User.Identity.Name;

            // 2. Busca o ID da organização através do documento
            var organizacao = _context.Organizacaos
                .FirstOrDefault(o => o.Cpf == documentoSessao || o.Cnpj == documentoSessao);

            if (organizacao == null)
                return View(new List<EventoViewModel>());

            // 3. Busca eventos filtrando pelo ID da organização
            var entidades = _service.GetByOrganizacao((int)organizacao.Id);
            var model = _mapper.Map<IEnumerable<EventoViewModel>>(entidades);

            return View(model);
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
        [Authorize]
        public IActionResult Create(EventoViewModel model)
        {
            ModelState.Remove("Imagem");
            ModelState.Remove("Data");

            if (ModelState.IsValid)
            {
                try
                {
                    var documentoSessao = User.Identity.Name;
                    var organizacao = _context.Organizacaos
                        .FirstOrDefault(o => o.Cpf == documentoSessao || o.Cnpj == documentoSessao);

                    if (organizacao == null)
                    {
                        ModelState.AddModelError("", "Erro: Perfil de organização não encontrado para este usuário.");
                        return View(model);
                    }

                    var evento = _mapper.Map<Evento>(model);
                    evento.IdOrganizacao = (int)organizacao.Id;

                    if (model.DataOnly.HasValue && model.HoraOnly.HasValue)
                    {
                        evento.Data = model.DataOnly.Value.Date.Add(model.HoraOnly.Value);
                    }

                    // 1º PASSO: CRIAR O EVENTO PRIMEIRO
                    // Isso insere no banco e captura o ID gerado pelo auto incremento
                    _service.Create(evento);

                    // 2º PASSO: PROCESSAR A IMAGEM COM O ID CORRETO
                    if (model.ImagemUpload != null)
                    {
                        string extensao = Path.GetExtension(model.ImagemUpload.FileName);

                        // Agora o evento.Id possui o número correto gerado pelo banco!
                        string nomeArquivo = $"EventoBanner_{evento.Id}{extensao}";

                        // Salva a imagem no disco
                        evento.Imagem = SalvarImagemNoDisco(model.ImagemUpload, nomeArquivo);

                        // 3º PASSO: ATUALIZAR O REGISTRO
                        // Fazemos o update apenas para colocar o nome correto da imagem na linha que acabamos de criar
                        _service.Edit(evento);
                    }

                    TempData["MensagemSucesso"] = "Evento criado com sucesso! 🏃‍♂️";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocorreu um erro interno ao salvar o evento: " + ex.Message);
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
                        // Deleta a anterior se existir
                        if (!string.IsNullOrEmpty(model.Imagem))
                            DeletarImagemDoDisco(model.Imagem);

                        // Salva com o mesmo padrão de nomenclatura usando o ID que já existe
                        string extensao = Path.GetExtension(model.ImagemUpload.FileName);
                        string nomeArquivo = $"EventoBanner_{evento.Id}{extensao}";

                        evento.Imagem = SalvarImagemNoDisco(model.ImagemUpload, nomeArquivo);
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
        [Authorize]
        public IActionResult Delete(int id)
        {
            // 1. Busca o evento para garantir que ele existe
            var evento = _service.Get(id);
            if (evento == null)
            {
                TempData["MensagemErro"] = "Evento não encontrado.";
                return RedirectToAction("Index");
            }

            // 2. VERIFICAÇÃO REAL NA TABELA DE INSCRIÇÕES:
            // Contamos quantos registros existem para este IdEvento na tabela 'inscricao'
            int totalAtletasInscritos = _context.Inscricao.Count(i => i.IdEvento == id);

            if (totalAtletasInscritos > 0)
            {
                TempData["MensagemErro"] = $"O evento '{evento.Nome}' não pode ser excluído porque possui {totalAtletasInscritos} atleta(s) inscrito(s).";
                return RedirectToAction("Index");
            }

            try
            {
                // 3. Se não há inscritos, removemos os Kits primeiro (dependência de FK)
                var kits = _kitService.GetAll().Where(k => k.IdEvento == id).ToList();
                foreach (var kit in kits)
                {
                    if (!string.IsNullOrEmpty(kit.Imagem)) DeletarImagemDoDisco(kit.Imagem);
                    _kitService.Delete(kit.Id);
                }

                // 4. Remove a imagem do evento e o registro do banco
                if (!string.IsNullOrEmpty(evento.Imagem))
                {
                    DeletarImagemDoDisco(evento.Imagem);
                }

                _service.Delete(id);

                TempData["MensagemSucesso"] = "Evento removido com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro ao tentar excluir: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        private string SalvarImagemNoDisco(Microsoft.AspNetCore.Http.IFormFile imagemUpload, string nomeDesejado)
        {
            string pasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens/banner-eventos");
            if (!Directory.Exists(pasta)) Directory.CreateDirectory(pasta);

            // Usa o nome passado como parâmetro
            string caminhoCompleto = Path.Combine(pasta, nomeDesejado);

            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                imagemUpload.CopyTo(stream);
            }

            return nomeDesejado;
        }
        /// <summary>
        /// Exibe o catálogo geral de eventos disponíveis na plataforma para os usuários finais, permitindo buscas dinâmicas.
        /// </summary>
        /// <param name="search">Termo opcional de pesquisa para filtrar eventos pelo nome, cidade ou estado.</param>
        /// <returns>View contendo a vitrine de eventos compatíveis com a busca.</returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> IndexUsuario(string? search)
        {
            // CPF salvo como UserName no Identity, sem ponto e hífen.
            string? cpfUsuarioLogado = User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(cpfUsuarioLogado))
            {
                return RedirectToAction("Login", "Corredor");
            }

            // Busca o perfil do corredor no banco principal.
            var corredor = await _corredorService.GetByCpfAsync(cpfUsuarioLogado);

            if (corredor == null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Corredor");
            }

            // Dados disponíveis para a View.
            ViewBag.IdCorredor = corredor.Id;
            ViewBag.NomeCorredor = corredor.Nome;

            search = search?.Trim();
            var agora = DateTime.Now;

            var eventos = _context.Eventos
                .AsNoTracking()
                .Where(e => e.Data >= agora)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                eventos = eventos.Where(e =>
                    e.Nome.Contains(search) ||
                    e.Cidade.Contains(search) ||
                    e.Estado.Contains(search));
            }

            var model = await eventos
                .OrderBy(e => e.Data)
                .Select(e => new EventoViewModel
                {
                    Id = (uint)e.Id,
                    Nome = e.Nome,
                    Data = e.Data,
                    NumeroParticipantes = e.NumeroParticipantes,
                    Descricao = e.Descricao,
                    Distancia3 = e.Distancia3,
                    Distancia5 = e.Distancia5,
                    Distancia7 = e.Distancia7,
                    Distancia10 = e.Distancia10,
                    Distancia15 = e.Distancia15,
                    Distancia21 = e.Distancia21,
                    Distancia42 = e.Distancia42,
                    Rua = e.Rua,
                    Bairro = e.Bairro,
                    Cidade = e.Cidade,
                    Estado = e.Estado,
                    InfoRetiradaKit = e.InfoRetiradaKit,
                    IdOrganizacao = (uint)e.IdOrganizacao,
                    Imagem = e.Imagem
                })
                .ToListAsync();

            return View(model);
        }

        private void DeletarImagemDoDisco(string nomeImagem)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens/banner-eventos", nomeImagem);
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            }
            catch
            {
            }
        }
    }
}

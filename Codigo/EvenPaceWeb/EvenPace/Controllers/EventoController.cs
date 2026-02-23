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

        public IActionResult Details(int id)
        {
            var entidade = _service.Get(id);
            if (entidade == null) return RedirectToAction("Index");

            var viewModel = _mapper.Map<EventoViewModel>(entidade);

            ViewBag.IdEventoAtual = viewModel.Id;
            ViewBag.NomeCorrida = viewModel.Nome;

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.TituloPagina = "Novo Evento";
            return View(new EventoViewModel());
        }

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
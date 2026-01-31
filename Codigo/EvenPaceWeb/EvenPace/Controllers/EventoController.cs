using AutoMapper;
using Core;
using Core.Service;
using Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EvenPaceWeb.Controllers
{
    public class EventoController : Controller
    {
        private readonly IEventosService _eventoService;
        private readonly IMapper _mapper;
        public EventoController(IEventosService eventoService, IMapper mapper)
        {
            _eventoService = eventoService;
            _mapper = mapper;
        }
        // GET: EventoController
        public ActionResult Index()
        {
            var eventos = _eventoService.GetAll();
            var eventoViewModels = _mapper.Map<List<EventoViewModel>>(eventos);
            return View(eventoViewModels);
        }

        // GET: EventoController/Details/5
        public ActionResult Details(int id)
        {
            var evento = _eventoService.Get(id);
            var eventoViewModel = _mapper.Map<EventoViewModel>(evento);
            return View(eventoViewModel);
        }

        // GET: EventoController/Create
        public ActionResult Create()
        {
            var viewModel = new EventoViewModel();

            // SIMULAÇÃO DE LOGIN: Define a organização fixa como 1
            viewModel.IdOrganizacao = 1;

            return View(viewModel);
        }

        // POST: EventoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EventoViewModel model)
        {
            // Remove validações de campos que não vêm preenchidos diretamente do form
            ModelState.Remove("Imagem");
            ModelState.Remove("Data");

            if (ModelState.IsValid)
            {
                try
                {
                    // 1. MAPEAMENTO AUTOMÁTICO (ViewModel -> Entidade)
                    var evento = _mapper.Map<Evento>(model);

                    // 2. LOGICA DE DATA E HORA
                    // Junta o DatePicker e o TimePicker em um único DateTime para o banco
                    if (model.DataOnly.HasValue && model.HoraOnly.HasValue)
                    {
                        evento.Data = model.DataOnly.Value.Add(model.HoraOnly.Value);
                    }

                    // 3. UPLOAD DE IMAGEM
                    if (model.ImagemUpload != null)
                    {
                        string pastaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens");
                        if (!Directory.Exists(pastaDestino)) Directory.CreateDirectory(pastaDestino);

                        // Gera nome único para não substituir fotos com mesmo nome
                        string nomeArquivo = Guid.NewGuid().ToString() + "_" + model.ImagemUpload.FileName;
                        string caminhoCompleto = Path.Combine(pastaDestino, nomeArquivo);

                        using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                        {
                            model.ImagemUpload.CopyTo(stream);
                        }

                        evento.Imagem = nomeArquivo;
                    }

                    // 4. SALVAR NO BANCO
                    _eventoService.Create(evento);

                    TempData["MensagemSucesso"] = "Evento criado com sucesso! 🏃‍♂️";

                    // Redireciona para a lista de eventos (Index) ou volta para a Home
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erro ao salvar: " + ex.Message);
                }
            }

            // Se der erro, retorna a tela com os dados preenchidos
            return View(model);
        }
    


        // GET: EventoController/Edit/5
        public ActionResult Edit(int id)
        {
            var evento = _eventoService.Get(id);
            var eventoViewModel = _mapper.Map<EventoViewModel>(evento);
            return View(eventoViewModel);
        }

        // POST: EventoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EventoViewModel eventoViewModel)
        {
            if (ModelState.IsValid)
            {
                var evento = _mapper.Map<Core.Evento>(eventoViewModel);
                _eventoService.Edit(evento);
            }
            return RedirectToAction(nameof(Index));

        }

        // GET: EventoController/Delete/5
        public ActionResult Delete(int id)
        {
            var evento = _eventoService.Get(id);
            var eventoViewModel = _mapper.Map<EventoViewModel>(evento);
            return View(eventoViewModel);
        }

        // POST: EventoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            _eventoService.Delete((int)id);
            return RedirectToAction(nameof(Index));

        }
    }
}

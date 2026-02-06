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

        public ActionResult Index()
        {
            var eventos = _eventoService.GetAll();
            var eventoViewModels = _mapper.Map<List<EventoViewModel>>(eventos);
            return View(eventoViewModels);
        }

        public ActionResult Details(int id)
        {
            var evento = _eventoService.Get(id);
            var eventoViewModel = _mapper.Map<EventoViewModel>(evento);
            return View(eventoViewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EventoViewModel eventoViewModel)
        {
            if (ModelState.IsValid)
            {
                if (eventoViewModel.ImagemUpload != null)
                {
                    string pasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens");

                    if (!Directory.Exists(pasta))
                        Directory.CreateDirectory(pasta);

                    string nomeArquivo = Guid.NewGuid().ToString() +
                                         Path.GetExtension(eventoViewModel.ImagemUpload.FileName);

                    string caminho = Path.Combine(pasta, nomeArquivo);

                    using (var stream = new FileStream(caminho, FileMode.Create))
                    {
                        await eventoViewModel.ImagemUpload.CopyToAsync(stream);
                    }

                    eventoViewModel.Imagem = nomeArquivo;
                }

                var evento = _mapper.Map<Evento>(eventoViewModel);
                _eventoService.Create(evento);

                return RedirectToAction(nameof(Index));
            }

            return View(eventoViewModel);
        }

        public ActionResult Edit(int id)
        {
            var evento = _eventoService.Get(id);
            var eventoViewModel = _mapper.Map<EventoViewModel>(evento);
            return View(eventoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EventoViewModel eventoViewModel)
        {
            if (ModelState.IsValid)
            {
                if (eventoViewModel.ImagemUpload != null)
                {
                    string pasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens");

                    if (!Directory.Exists(pasta))
                        Directory.CreateDirectory(pasta);

                    string nomeArquivo = Guid.NewGuid().ToString() +
                                         Path.GetExtension(eventoViewModel.ImagemUpload.FileName);

                    string caminho = Path.Combine(pasta, nomeArquivo);

                    using (var stream = new FileStream(caminho, FileMode.Create))
                    {
                        await eventoViewModel.ImagemUpload.CopyToAsync(stream);
                    }

                    eventoViewModel.Imagem = nomeArquivo;
                }

                var evento = _mapper.Map<Evento>(eventoViewModel);
                _eventoService.Edit(evento);

                return RedirectToAction(nameof(Index));
            }

            return View(eventoViewModel);
        }

        public ActionResult Delete(int id)
        {
            var evento = _eventoService.Get(id);
            var eventoViewModel = _mapper.Map<EventoViewModel>(evento);
            return View(eventoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            _eventoService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

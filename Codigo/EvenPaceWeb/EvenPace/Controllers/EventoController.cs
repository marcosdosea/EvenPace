using AutoMapper;
using Core;
using Core.Service;
using EvenPaceWeb.Models;
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
            var evento = _eventoService.Get((int)id);
            var eventoViewModel = _mapper.Map<EventoViewModel>(evento);
            return View(eventoViewModel);
        }

        // GET: EventoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EventoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventoViewModel eventoViewModel)
        {
            if (ModelState.IsValid)
            {
                var evento = _mapper.Map<Core.Evento>(eventoViewModel);
                _eventoService.Create(evento);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: EventoController/Edit/5
        public ActionResult Edit(int id)
        {
            var evento = _eventoService.Get((int)id);
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
            var evento = _eventoService.Get((int)id);
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
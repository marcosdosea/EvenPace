using Microsoft.AspNetCore.Mvc;
using Core.Service;
using Core;
using Models;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using EvenPaceWeb.Models;
using Microsoft.EntityFrameworkCore;

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
   

        // ==========================================================
        // 1. INDEX (Listagem)
        // ==========================================================
        public IActionResult Index()
        {
            int idOrganizacao = 1; // Simulação de login

            var listaEntidades = _service.GetAll()
                                         .Where(e => e.IdOrganizacao == idOrganizacao)
                                         .OrderByDescending(e => e.Data)
                                         .ToList();

            var listaViewModel = _mapper.Map<List<EventoViewModel>>(listaEntidades);

            return View(listaViewModel);
        }

        // ==========================================================
        // 2. DETAILS (Antigo Resumo)
        // ==========================================================
        public IActionResult Details(int id)
        {
            var entidade = _service.Get(id);
            if (entidade == null) return RedirectToAction("Index");

            var viewModel = _mapper.Map<EventoViewModel>(entidade);

            ViewBag.IdEventoAtual = viewModel.Id;
            ViewBag.NomeCorrida = viewModel.Nome;

           
            return View(viewModel);
        }

        // ==========================================================
        // 3. CREATE (Criar - GET)
        // ==========================================================
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.TituloPagina = "Novo Evento";
            return View(new EventoViewModel());
        }

        // ==========================================================
        // 4. CREATE (Criar - POST)
        // ==========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EventoViewModel model)
        {
            // Remove validações que não são obrigatórias na criação se necessário
            ModelState.Remove("Imagem");
            ModelState.Remove("Data");

            if (ModelState.IsValid)
            {
                try
                {
                    var evento = _mapper.Map<Evento>(model);

                    // Lógica de Data + Hora
                    if (model.DataOnly.HasValue && model.HoraOnly.HasValue)
                    {
                        evento.Data = model.DataOnly.Value.Add(model.HoraOnly.Value);
                    }

                    // Upload de Imagem
                    if (model.ImagemUpload != null)
                    {
                        evento.Imagem = SalvarImagemNoDisco(model.ImagemUpload);
                    }

                    // Define organização padrão
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

        // ==========================================================
        // 5. EDIT (Editar - GET)
        // ==========================================================
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

        // ==========================================================
        // 6. EDIT (Editar - POST)
        // ==========================================================
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

                    // Lógica de Data + Hora
                    if (model.DataOnly.HasValue && model.HoraOnly.HasValue)
                    {
                        evento.Data = model.DataOnly.Value.Add(model.HoraOnly.Value);
                    }

                    // Lógica de Imagem no Edit:
                    // 1. Se tem upload novo: Salva a nova e deleta a antiga.
                    // 2. Se não tem upload: Mantém a string da imagem antiga (que vem no model.Imagem via hidden input).
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




        // ==========================================================
        // 7. DELETE (Excluir)
        // ==========================================================
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var evento = _service.Get(id);

            if (evento != null)
            {
                // Apaga Kits vinculados
                var kitsDoEvento = _kitService.GetAll().Where(k => k.IdEvento == id).ToList();
                foreach (var kit in kitsDoEvento)
                {
                    if (!string.IsNullOrEmpty(kit.Imagem)) DeletarImagemDoDisco(kit.Imagem);
                    _kitService.Delete(kit.Id);
                }

                // Apaga Imagem do Evento
                if (!string.IsNullOrEmpty(evento.Imagem))
                {
                    DeletarImagemDoDisco(evento.Imagem);
                }

                // Apaga o Evento do Banco
                _service.Delete(id);

                TempData["MensagemSucesso"] = "Evento excluído com sucesso! 🗑️";
            }

            return RedirectToAction("Index");
        }

        // ==========================================================
        // MÉTODOS AUXILIARES (Privados)
        // ==========================================================

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
                .Select(e => new TelaListaEventosViewModel
                {
                    Id = e.Id,
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
                // Logar erro se necessário, mas não parar a execução
            }
        }
    }
}

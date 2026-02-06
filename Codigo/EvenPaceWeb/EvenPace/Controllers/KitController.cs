using AutoMapper;
using Core;
using Core.Service;
using Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace EvenPace.Controllers
{
    public class KitController : Controller
    {
        private readonly IKitService _kitsService;
        private readonly IMapper _mapper;
        private readonly IEventosService _eventosService;

        public KitController(IKitService kits, IMapper mapper, IEventosService eventosService)
        {
            _kitsService = kits;
            _mapper = mapper;
            _eventosService = eventosService;
        }

        // ==========================================================
        // 1. INDEX (Listagem dos Kits de um Evento)
        // ==========================================================
        [HttpGet]
        public IActionResult IndexKit(int? idEvento)
        {
            // 1. Organização Fixa (Simulação de Login)
            int idOrganizacaoLogada = 1;

            // 2. Busca eventos dessa organização para validar ou pegar o padrão
            var eventosDaOrganizacao = _eventosService.GetAll()
                                                       .Where(e => e.IdOrganizacao == idOrganizacaoLogada)
                                                       .ToList();

            // 3. Define qual evento exibir
            if (!idEvento.HasValue || idEvento.Value == 0)
            {
                var eventoPadrao = eventosDaOrganizacao.FirstOrDefault();

                if (eventoPadrao != null)
                {
                    idEvento = eventoPadrao.Id;
                }
                else
                {
                    TempData["MensagemErro"] = "Você ainda não possui eventos cadastrados.";
                    return RedirectToAction("Index", "Home");
                }
            }

            int idFinal = idEvento.Value;

            // Preenche ViewBag para a View saber qual evento estamos vendo
            var eventoAtual = eventosDaOrganizacao.FirstOrDefault(e => e.Id == idFinal);
            ViewBag.NomeCorrida = eventoAtual != null ? eventoAtual.Nome : "Evento";
            ViewBag.IdEventoAtual = idFinal;

            // Filtra os kits do evento específico
            var allKits = _kitsService.GetAll();
            var kitsDoEvento = allKits.Where(k => k.IdEvento == idFinal).ToList();

            var listaViewModel = _mapper.Map<List<KitViewModel>>(kitsDoEvento);
            return View(listaViewModel);
        }

        // ==========================================================
        // 2. CREATE (Criar - GET)
        // ==========================================================
        [HttpGet]
        public IActionResult Create(int? idEvento)
        {
            var viewModel = new KitViewModel();

            // Define o evento pai (se não vier, assume 1 por segurança)
            viewModel.IdEvento = idEvento ?? 1;

            // Dados para a View
            ViewBag.TituloPagina = "Novo Kit";
            var evento = _eventosService.Get(viewModel.IdEvento);
            ViewBag.NomeCorrida = evento != null ? evento.Nome : "Evento";

            return View(viewModel);
        }

        // ==========================================================
        // 3. CREATE (Criar - POST)
        // ==========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(KitViewModel model)
        {
            RemoverValidacoesNaoObrigatorias();

            if (ModelState.IsValid)
            {
                try
                {
                    var kit = _mapper.Map<Kit>(model);

                    // Upload de Imagem
                    if (model.ImagemUpload != null)
                    {
                        kit.Imagem = SalvarImagemNoDisco(model.ImagemUpload);
                    }

                    _kitsService.Create(kit);
                    TempData["MensagemSucesso"] = "Kit criado com sucesso! ✅";

                    return RedirectToAction("IndexKit", new { idEvento = model.IdEvento });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erro ao criar: " + ex.Message);
                }
            }

            // Se falhar, recarrega dados da view
            ViewBag.TituloPagina = "Novo Kit";
            var evento = _eventosService.Get(model.IdEvento);
            ViewBag.NomeCorrida = evento != null ? evento.Nome : "Evento";

            return View(model);
        }

        // ==========================================================
        // 4. EDIT (Editar - GET)
        // ==========================================================
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var kit = _kitsService.Get(id);
            if (kit == null) return RedirectToAction("IndexKit");

            var viewModel = _mapper.Map<KitViewModel>(kit);

            // Dados para a View
            ViewBag.TituloPagina = "Editar Kit";
            var evento = _eventosService.Get(kit.IdEvento);
            ViewBag.NomeCorrida = evento != null ? evento.Nome : "Evento";

            return View(viewModel);
        }

        // ==========================================================
        // 5. EDIT (Editar - POST)
        // ==========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, KitViewModel model)
        {
            if (id != model.Id) return NotFound();

            RemoverValidacoesNaoObrigatorias();

            if (ModelState.IsValid)
            {
                try
                {
                    var kit = _mapper.Map<Kit>(model);

                    // Lógica de Imagem no Edit
                    if (model.ImagemUpload != null)
                    {
                        // Se tem nova imagem, deleta a antiga e salva a nova
                        if (!string.IsNullOrEmpty(model.Imagem))
                            DeletarImagemDoDisco(model.Imagem);

                        kit.Imagem = SalvarImagemNoDisco(model.ImagemUpload);
                    }
                    else
                    {
                        // Mantém a imagem antiga
                        kit.Imagem = model.Imagem;
                    }

                    _kitsService.Edit(kit);
                    TempData["MensagemSucesso"] = "Kit atualizado com sucesso! ✏️";

                    return RedirectToAction("IndexKit", new { idEvento = model.IdEvento });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erro ao editar: " + ex.Message);
                }
            }

            ViewBag.TituloPagina = "Editar Kit";
            var evento = _eventosService.Get(model.IdEvento);
            ViewBag.NomeCorrida = evento != null ? evento.Nome : "Evento";

            return View(model);
        }

        // ==========================================================
        // 6. DELETE (Excluir)
        // ==========================================================
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var kit = _kitsService.Get(id);

            if (kit != null)
            {
                int idEventoDoKit = kit.IdEvento;

                // Apagar a imagem física do disco se existir
                if (!string.IsNullOrEmpty(kit.Imagem))
                {
                    DeletarImagemDoDisco(kit.Imagem);
                }

                _kitsService.Delete(id);
                TempData["MensagemSucesso"] = "Kit excluído com sucesso! 🗑️";

                return RedirectToAction("IndexKit", new { idEvento = idEventoDoKit });
            }

            return RedirectToAction("IndexKit"); // Fallback sem ID
        }

        // ==========================================================
        // MÉTODOS AUXILIARES (Privados)
        // ==========================================================

        private void RemoverValidacoesNaoObrigatorias()
        {
            ModelState.Remove("ImagemUpload");
            ModelState.Remove("IdEvento");
            ModelState.Remove("UtilizadaP");
            ModelState.Remove("UtilizadaM");
            ModelState.Remove("UtilizadaG");
            ModelState.Remove("StatusRetiradaKit");
        }

        private string SalvarImagemNoDisco(Microsoft.AspNetCore.Http.IFormFile imagemUpload)
        {
            string pastaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens");
            if (!Directory.Exists(pastaDestino)) Directory.CreateDirectory(pastaDestino);

            string nomeUnico = Guid.NewGuid().ToString() + "_" + imagemUpload.FileName;
            string caminhoCompleto = Path.Combine(pastaDestino, nomeUnico);

            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                imagemUpload.CopyTo(stream);
            }

            return nomeUnico;
        }

        private void DeletarImagemDoDisco(string nomeImagem)
        {
            try
            {
                string caminhoCompleto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", nomeImagem);
                if (System.IO.File.Exists(caminhoCompleto))
                {
                    System.IO.File.Delete(caminhoCompleto);
                }
            }
            catch { /* Ignora erro de arquivo travado/inexistente */ }
        }
    }
}
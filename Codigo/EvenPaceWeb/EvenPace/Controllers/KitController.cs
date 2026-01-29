using AutoMapper;
using Core;
using Core.Service;
using Models;
using Microsoft.AspNetCore.Mvc;

namespace EvenPace.Controllers;

public class KitController : Controller
{
    private IKitService _kitsService;
    private IMapper _mapper;
    private IEventosService _eventosService;

    public KitController(IKitService kits, IMapper mapper, IEventosService eventosService)
    {
        _kitsService = kits;
        _mapper = mapper;
        _eventosService = eventosService;
    }

    // GET: Abre a Tela 17 (Create) para o usuário preencher
    [HttpGet]
    public IActionResult Create(int? id, int? idEvento)
    {
        KitViewModel viewModel = new KitViewModel();

        // CENÁRIO 1: EDIÇÃO (Clicou no Lápis)
        if (id.HasValue && id.Value > 0)
        {
            var kit = _kitsService.Get(id.Value);
            if (kit != null)
            {
                // Mapeia os dados do Banco para a Tela (Nome, Valor, Preço...)
                viewModel = _mapper.Map<KitViewModel>(kit);
                ViewBag.TituloPagina = "Editar Kit";
            }
        }
        // CENÁRIO 2: NOVO (Clicou no + Criar)
        else if (idEvento.HasValue)
        {
            viewModel.IdEvento = idEvento.Value;
            ViewBag.TituloPagina = "Novo Kit";
        }
        else
        {
            // Segurança: Pega o evento 1 se não vier nada
            viewModel.IdEvento = 1;
        }

        // Busca o nome da corrida para exibir no topo
        var evento = _eventosService.Get(viewModel.IdEvento);
        ViewBag.NomeCorrida = evento != null ? evento.Nome : "Evento";

        return View(viewModel);
    }

    // POST: Recebe os dados do formulário quando clica em Salvar
    // POST: KitController/Tela17_Organizacao_CriarKit (Create)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(KitViewModel model)
    {
     
        ModelState.Remove("ImagemUpload");
        ModelState.Remove("IdEvento");
        ModelState.Remove("UtilizadaP");
        ModelState.Remove("UtilizadaM");
        ModelState.Remove("UtilizadaG");
        ModelState.Remove("StatusRetiradaKit");

        if (ModelState.IsValid)
        {
            try
            {
                var kit = _mapper.Map<Kit>(model);

                /
                if (model.ImagemUpload != null)
                {
                    
                    if (model.Imagem != null)
                    {
                        DeletarImagemDoDisco(model.Imagem);
                    }
                
                    string pastaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens");

                 
                    if (!Directory.Exists(pastaDestino)) Directory.CreateDirectory(pastaDestino);

                    
                    string nomeUnico = Guid.NewGuid().ToString() + "_" + model.ImagemUpload.FileName;
                    string caminhoCompleto = Path.Combine(pastaDestino, nomeUnico);

            
                    using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                    {
                        model.ImagemUpload.CopyTo(stream);
                    }

                   
                    //kit.Imagem = nomeUnico;
                }
                //else
                //{
                    // Se NÃO enviou foto nova, mantém a string da foto antiga
                    //kit.Imagem = model.Imagem;
                //}

              
                if (model.Id > 0)
                {
                 
                    _kitsService.Edit(kit);
                    TempData["MensagemSucesso"] = "Kit atualizado com sucesso! ✏️";
                }
                else
                {
                    // CRIAÇÃO
                    _kitsService.Create(kit);
                    TempData["MensagemSucesso"] = "Kit criado com sucesso! ✅";
                }

             
                return RedirectToAction("IndexKit", new { idEvento = model.IdEvento });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao salvar: " + ex.Message);
            }
        }

       
        var evento = _eventosService.Get(model.IdEvento);
        ViewBag.NomeCorrida = evento != null ? evento.Nome : "Evento";

        return View(model);
    }

    
    [HttpGet]
    public IActionResult IndexKit(int? idEvento)
    {
      
        int idOrganizacaoLogada = 1;

   
        var eventosDaOrganizacao = _eventosService.GetAll()
                                    .Where(e => e.IdOrganizacao == idOrganizacaoLogada)
                                    .ToList();

     
        if (!idEvento.HasValue || idEvento.Value == 0)
        {
            
            var eventoPadrao = eventosDaOrganizacao.FirstOrDefault();

            if (eventoPadrao != null)
            {
                idEvento = (int)eventoPadrao.Id;
            }
            else
            {
  
                TempData["MensagemErro"] = "Você ainda não possui eventos cadastrados.";
                return RedirectToAction("Index", "Home");
            }
        }

      
        int idFinal = idEvento.Value;

       
        var eventoAtual = eventosDaOrganizacao.FirstOrDefault(e => e.Id == idFinal);
        ViewBag.NomeCorrida = eventoAtual != null ? eventoAtual.Nome : "Evento";
        ViewBag.IdEventoAtual = idFinal;

        var allKits = _kitsService.GetAll();
        var kitsDoEvento = allKits.Where(k => k.IdEvento == idFinal).ToList();

        var listaViewModel = _mapper.Map<List<KitViewModel>>(kitsDoEvento);
        return View(listaViewModel);
    }

    [HttpGet]
    public IActionResult Excluir(int id)
    {
        var kit = _kitsService.Get(id);

        if (kit != null)
        {
            int idEventoDoKit = (int)kit.IdEvento;

       
            //if (!string.IsNullOrEmpty(kit.Imagem))
            //{
              //  DeletarImagemDoDisco(kit.Imagem);
            //}
            

            _kitsService.Delete(id);

            TempData["MensagemSucesso"] = "Kit excluído com sucesso! 🗑️";

            return RedirectToAction("IndexKit", new { idEvento = idEventoDoKit });
        }

        return RedirectToAction("IndexKit");
    }

    
    private void DeletarImagemDoDisco(string nomeImagem)
    {
       
        if (string.IsNullOrEmpty(nomeImagem)) return;

        
        string caminhoCompleto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", nomeImagem);

        if (System.IO.File.Exists(caminhoCompleto))
        {
            try
            {
                System.IO.File.Delete(caminhoCompleto);
            }
            catch (Exception)
            {
              
            }
        }
    }

}

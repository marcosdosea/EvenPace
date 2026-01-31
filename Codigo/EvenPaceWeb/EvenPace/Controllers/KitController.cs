using AutoMapper;
using Core;
using Core.Service;
using Models;
using Microsoft.AspNetCore.Mvc;

namespace EvenPace.Controllers;

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

    // GET: Abre a tela para Criar ou Editar
    [HttpGet]
    public IActionResult Create(int? id, int? idEvento)
    {
        KitViewModel viewModel = new KitViewModel();

        // CENÁRIO 1: EDIÇÃO (Se veio um ID válido na URL)
        if (id.HasValue && id.Value > 0)
        {
            var kit = _kitsService.Get(id.Value);
            if (kit != null)
            {
                // Mapeia os dados do Banco para a Tela
                viewModel = _mapper.Map<KitViewModel>(kit);
                ViewBag.TituloPagina = "Editar Kit";
            }
            else
            {
                // Se tentou editar um ID que não existe, trata como novo
                viewModel.IdEvento = idEvento ?? 1;
                ViewBag.TituloPagina = "Novo Kit";
            }
        }
        // CENÁRIO 2: NOVO (Clicou no botão Criar)
        else if (idEvento.HasValue)
        {
            viewModel.IdEvento = idEvento.Value;
            ViewBag.TituloPagina = "Novo Kit";
        }
        else
        {
            // Segurança: Se não veio nada, assume evento 1
            viewModel.IdEvento = 1;
            ViewBag.TituloPagina = "Novo Kit";
        }

        // Busca o nome da corrida para exibir no cabeçalho
        var evento = _eventosService.Get(viewModel.IdEvento);
        ViewBag.NomeCorrida = evento != null ? evento.Nome : "Evento não encontrado";

        return View(viewModel);
    }

    // POST: Recebe os dados do formulário para Salvar
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(KitViewModel model)
    {
        // 1. LIMPEZA DE VALIDAÇÕES (Campos que não são obrigatórios no envio)
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

                // --- LÓGICA DE UPLOAD DE IMAGEM ---
                if (model.ImagemUpload != null)
                {
                    // A. Se já existia uma foto antiga, apaga ela
                    if (!string.IsNullOrEmpty(model.Imagem))
                    {
                        DeletarImagemDoDisco(model.Imagem);
                    }

                    // B. Salva a nova foto
                    string pastaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens");
                    if (!Directory.Exists(pastaDestino)) Directory.CreateDirectory(pastaDestino);

                    string nomeUnico = Guid.NewGuid().ToString() + "_" + model.ImagemUpload.FileName;
                    string caminhoCompleto = Path.Combine(pastaDestino, nomeUnico);

                    using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                    {
                        model.ImagemUpload.CopyTo(stream);
                    }
                    kit.Imagem = nomeUnico;
                }
                else
                {
                    // Mantém a imagem antiga se não enviou nova
                    kit.Imagem = model.Imagem;
                }

                // --- CORREÇÃO DO SEU ERRO AQUI ---
                if (model.Id > 0)
                {
                    // Verifica se o Kit realmente existe no banco
                    var kitExistente = _kitsService.Get((int)model.Id);

                    if (kitExistente != null)
                    {
                        // O Kit existe, então podemos atualizar
                        // Garante que o objeto kit tenha o ID correto
                        kit.Id = (int)model.Id;
                        _kitsService.Edit(kit);
                        TempData["MensagemSucesso"] = "Kit atualizado com sucesso! ✏️";
                    }
                    else
                    {
                        // O ID veio > 0, mas o banco foi resetado e esse kit sumiu.
                        // Solução: Criamos como se fosse novo para não dar erro.
                        kit.Id = 0;
                        _kitsService.Create(kit);
                        TempData["MensagemSucesso"] = "Kit recriado com sucesso! (O registro original não existia) ✅";
                    }
                }
                else
                {
                    // ID é 0, então é Criação normal
                    _kitsService.Create(kit);
                    TempData["MensagemSucesso"] = "Kit criado com sucesso! ✅";
                }

                // Redireciona para a lista
                return RedirectToAction("IndexKit", new { idEvento = model.IdEvento });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao salvar: " + ex.Message);
            }
        }

        // Se algo deu errado (Model inválido), recarrega a View
        var evento = _eventosService.Get(model.IdEvento);
        ViewBag.NomeCorrida = evento != null ? evento.Nome : "Evento";

        return View(model);
    }

    // GET: Listagem dos Kits
    [HttpGet]
    public IActionResult IndexKit(int? idEvento)
    {
        // 1. Organização Fixa (Simulação de Login)
        int idOrganizacaoLogada = 1;

        // 2. Busca eventos dessa organização
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

        // Filtra os kits
        var allKits = _kitsService.GetAll();
        var kitsDoEvento = allKits.Where(k => k.IdEvento == idFinal).ToList();

        var listaViewModel = _mapper.Map<List<KitViewModel>>(kitsDoEvento);
        return View(listaViewModel);
    }

    // GET: Excluir Kit
    [HttpGet]
    public IActionResult Excluir(int id)
    {
        var kit = _kitsService.Get(id);

        if (kit != null)
        {
            int idEventoDoKit = kit.IdEvento;

            // Opcional: Apagar a imagem física do disco
            if (!string.IsNullOrEmpty(kit.Imagem))
            {
                DeletarImagemDoDisco(kit.Imagem);
            }

            _kitsService.Delete(id);

            TempData["MensagemSucesso"] = "Kit excluído com sucesso! 🗑️";

            return RedirectToAction("IndexKit", new { idEvento = idEventoDoKit });
        }

        return RedirectToAction("IndexKit");
    }

    // Método Auxiliar para limpeza de arquivos
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
            catch
            {
                // Ignora erro de exclusão de arquivo para não travar o fluxo
            }
        }
    }
}
using AutoMapper;
using Core;
using Core.Service;
using EvenPaceWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Service;

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

    // GET: Abre a Tela 17 para o usuário preencher
    // GET: Abre a Tela 17
    [HttpGet]
    public IActionResult Tela17_Organizacao_CriarKit(int? id, int? idEvento)
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
    // POST: KitController/Tela17_Organizacao_CriarKit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Tela17_Organizacao_CriarKit(KitViewModel model)
    {
        // 1. LIMPEZA DE VALIDAÇÕES (Para não bloquear o salvamento)
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

                // --- LÓGICA DE UPLOAD DE IMAGEM (COM FAXINA) ---
                if (model.ImagemUpload != null)
                {
                    // A. FAXINA: Se já existia uma foto antiga, APAGA ELA do computador
                    // (O campo model.Imagem contém o nome da foto velha vindo do input hidden)
                    DeletarImagemDoDisco(model.Imagem);

                    // B. SALVAR A NOVA
                    string pastaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens");

                    // Cria a pasta se não existir
                    if (!Directory.Exists(pastaDestino)) Directory.CreateDirectory(pastaDestino);

                    // Gera nome único (UUID) para segurança interna
                    string nomeUnico = Guid.NewGuid().ToString() + "_" + model.ImagemUpload.FileName;
                    string caminhoCompleto = Path.Combine(pastaDestino, nomeUnico);

                    // Salva o arquivo fisicamente
                    using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                    {
                        model.ImagemUpload.CopyTo(stream);
                    }

                    // Atualiza o objeto para salvar o novo nome no banco
                    kit.Imagem = nomeUnico;
                }
                else
                {
                    // Se NÃO enviou foto nova, mantém a string da foto antiga
                    kit.Imagem = model.Imagem;
                }
                // ----------------------------------

                // --- SALVAR NO BANCO ---
                if (model.Id > 0)
                {
                    // EDIÇÃO
                    _kitsService.Edit(kit);
                    TempData["MensagemSucesso"] = "Kit atualizado com sucesso! ✏️";
                }
                else
                {
                    // CRIAÇÃO
                    _kitsService.Create(kit);
                    TempData["MensagemSucesso"] = "Kit criado com sucesso! ✅";
                }

                // Redireciona para a lista mantendo o filtro do evento
                return RedirectToAction("Tela09_Organizacao_Kits", new { idEvento = model.IdEvento });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao salvar: " + ex.Message);
            }
        }

        // SE DEU ERRO: Recarrega o nome do evento para a tela não quebrar
        var evento = _eventosService.Get(model.IdEvento);
        ViewBag.NomeCorrida = evento != null ? evento.Nome : "Evento";

        return View(model);
    }

    // GET: Tela09_Organizacao_Kits
    [HttpGet]
    public IActionResult Tela09_Organizacao_Kits(int? idEvento)
    {
        // 1. DEFINIMOS A ORGANIZAÇÃO ATUAL (Simulando o login)
        // Como combinamos no SQL, estamos usando a Organização ID = 1
        int idOrganizacaoLogada = 1;

        // 2. BUSCA EVENTOS *APENAS* DESSA ORGANIZAÇÃO
        // Não buscamos do banco todo, apenas os que "pertencem" ao usuário
        var eventosDaOrganizacao = _eventosService.GetAll()
                                    .Where(e => e.IdOrganizacao == idOrganizacaoLogada)
                                    .ToList();

        // 3. SELEÇÃO DO EVENTO (Dinâmica dentro da Organização)
        if (!idEvento.HasValue || idEvento.Value == 0)
        {
            // Se não veio ID na URL, pegamos o primeiro evento DA LISTA DA ORGANIZAÇÃO
            var eventoPadrao = eventosDaOrganizacao.FirstOrDefault();

            if (eventoPadrao != null)
            {
                idEvento = (int)eventoPadrao.Id;
            }
            else
            {
                // Se a organização não tem evento nenhum, não dá pra ver kits.
                // Redireciona para a Home ou mostra lista vazia.
                TempData["MensagemErro"] = "Você ainda não possui eventos cadastrados.";
                return RedirectToAction("Index", "Home");
            }
        }

        // --- DAQUI PRA BAIXO SEGUE O PADRÃO ---
        int idFinal = idEvento.Value;

        // Apenas garante que o nome do evento exibido é o correto
        var eventoAtual = eventosDaOrganizacao.FirstOrDefault(e => e.Id == idFinal);
        ViewBag.NomeCorrida = eventoAtual != null ? eventoAtual.Nome : "Evento";
        ViewBag.IdEventoAtual = idFinal;

        // Filtra os kits desse evento específico
        var allKits = _kitsService.GetAll();
        var kitsDoEvento = allKits.Where(k => k.IdEvento == idFinal).ToList();

        var listaViewModel = _mapper.Map<List<KitViewModel>>(kitsDoEvento);
        return View(listaViewModel);
    }

    // Get: KitController/Get/1
    public ActionResult Get(int id)
    {
        Kit kit = _kitsService.Get(id);
        KitViewModel kitModel = _mapper.Map<KitViewModel>(kit);
        return View(kitModel);
    }

    // Get: KitController/Create
    public ActionResult Create()
    {
        return View();
    }

    // Post: KitController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(KitViewModel kitModel)
    {
        if (ModelState.IsValid)
        {
            var kit = _mapper.Map<Kit>(kitModel);
            _kitsService.Create(kit);
        }
        return RedirectToAction(nameof(Index));
    }

    // Get: KitController/Edit/1
    public ActionResult Edit(int id)
    {
        Kit kit = _kitsService.Get(id);
        KitViewModel kitModel = _mapper.Map<KitViewModel>(kit);
        return View(kitModel);
    }

    // Post: KitController/Edit/1
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(KitViewModel kitModel)
    {
        if (ModelState.IsValid)
        {
            var kit = _mapper.Map<Kit>(kitModel);
            _kitsService.Edit(kit);
        }
        return RedirectToAction(nameof(Index));
    }

    // Get: KitController/Delete/2
    /*public ActionResult Delete(int id)
    {
        Kit kit = _kitsService.Get(id);
        KitViewModel kitModel = _mapper.Map<KitViewModel>(kit);
        return View(kitModel);
    }*/

    [HttpGet]
    public IActionResult Excluir(int id)
    {
        var kit = _kitsService.Get(id);

        if (kit != null)
        {
            int idEventoDoKit = (int)kit.IdEvento;

            // --- NOVO: APAGA A FOTO ANTES DE APAGAR O REGISTRO ---
            DeletarImagemDoDisco(kit.Imagem);
            // -----------------------------------------------------

            _kitsService.Delete(id);

            TempData["MensagemSucesso"] = "Kit excluído com sucesso! 🗑️";

            return RedirectToAction("Tela09_Organizacao_Kits", new { idEvento = idEventoDoKit });
        }

        return RedirectToAction("Tela09_Organizacao_Kits");
    }
    // MÉTODO PRIVADO PARA APAGAR FOTOS DA PASTA WWWROOT
    private void DeletarImagemDoDisco(string nomeImagem)
    {
        // 1. Se não tiver nome, não faz nada
        if (string.IsNullOrEmpty(nomeImagem)) return;

        // 2. Monta o caminho completo onde a foto está
        string caminhoCompleto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", nomeImagem);

        // 3. Verifica se o arquivo realmente existe e deleta
        if (System.IO.File.Exists(caminhoCompleto))
        {
            try
            {
                System.IO.File.Delete(caminhoCompleto);
            }
            catch (Exception)
            {
                // Se der erro ao apagar (arquivo em uso, permissão, etc), 
                // a gente ignora para não travar o sistema, mas poderia logar o erro.
            }
        }
    }

}
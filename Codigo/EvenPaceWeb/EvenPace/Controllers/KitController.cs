using AutoMapper;
using Core.Service;
using Models;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        /// Realiza a filtragem dos conjuntos de kits subordinados a um evento selecionado, garantindo o gerenciamento focado para o organizador.
        /// </summary>
        /// <param name="idEvento">Referência temporal do evento a ser analisado (se nulo, busca o primeiro existente do organizador).</param>
        /// <returns>Página consolidada contendo os kits mapeados na exibição.</returns>
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
                    idEvento = eventoPadrao.Id;
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

        /// <summary>
        /// Prepara a inicialização da tela voltada para o projeto e cadastro físico/descritivo de um novo kit.
        /// </summary>
        /// <param name="idEvento">Id atrelativo ao evento que deterá o kit.</param>
        /// <returns>Modelo limpo acoplado à formatação estrutural da View.</returns>
        [HttpGet]
        public IActionResult Create(int? idEvento)
        {
            var viewModel = new KitViewModel();

            viewModel.IdEvento = idEvento ?? 1;

            ViewBag.TituloPagina = "Novo Kit";
            var evento = _eventosService.Get(viewModel.IdEvento);
            ViewBag.NomeCorrida = evento != null ? evento.Nome : "Evento";

            return View(viewModel);
        }

        /// <summary>
        /// Registra e hospeda de fato a inserção de um kit, integrando seu processamento de envio e salvamento em disco de mídias ligadas à entidade.
        /// </summary>
        /// <param name="model">Conjunto final submetido pelas ferramentas do formulário, contendo valores em espécie e anexos.</param>
        /// <returns>Recarrega a listagem geral informando a inclusão bem-sucedida, ou reexibe os parâmetros defeituosos solicitando intervenção de preenchimento.</returns>
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

            ViewBag.TituloPagina = "Novo Kit";
            var evento = _eventosService.Get(model.IdEvento);
            ViewBag.NomeCorrida = evento != null ? evento.Nome : "Evento";

            return View(model);
        }

        /// <summary>
        /// Resgata as premissas atuais e dados já registrados do kit requisitado visando a repopulação de campos para fins de consertos ou ajustes na precificação/composição.
        /// </summary>
        /// <param name="id">Chave de acesso primário apontando para o kit desejado.</param>
        /// <returns>Retorna a interface visual acoplada ao repositório de dados encontrado.</returns>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var kit = _kitsService.Get(id);
            if (kit == null) return RedirectToAction("IndexKit");

            var viewModel = _mapper.Map<KitViewModel>(kit);

            ViewBag.TituloPagina = "Editar Kit";
            var evento = _eventosService.Get(kit.IdEvento);
            ViewBag.NomeCorrida = evento != null ? evento.Nome : "Evento";

            return View(viewModel);
        }

        /// <summary>
        /// Realiza a sobrescrita dos atributos modificados de um kit, procedendo com a exclusão preventiva da mídia anterior no sistema de arquivos local perante o envio de novas submissões visuais.
        /// </summary>
        /// <param name="id">Chave que homologa o item do banco sendo editado na sessão vigente.</param>
        /// <param name="model">Variável agrupadora de modificações do kit enviadas na requisição POST.</param>
        /// <returns>Executa a transição limpa para a home page e painel principal do organizador ou resgata a visualização da tela se corrompida durante as etapas lógicas de negócio.</returns>
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

                    if (model.ImagemUpload != null)
                    {
                        if (!string.IsNullOrEmpty(model.Imagem))
                            DeletarImagemDoDisco(model.Imagem);

                        kit.Imagem = SalvarImagemNoDisco(model.ImagemUpload);
                    }
                    else
                    {
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

        /// <summary>
        /// Executa a exclusão de todos os registros persistentes associados ao kit parametrizado, higienizando simultaneamente as mídias da pasta raiz em disco.
        /// </summary>
        /// <param name="id">A identidade atrelativa ao modelo no arquivo virtual do bando de dados principal.</param>
        /// <returns>Promove o reenquadramento automatizado para o painel de listagem em sequência bem-sucedida da exclusão.</returns>
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var kit = _kitsService.Get(id);

            if (kit != null)
            {
                int idEventoDoKit = kit.IdEvento;

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

        private void RemoverValidacoesNaoObrigatorias()
        {
            ModelState.Remove("ImagemUpload");
            ModelState.Remove("IdEvento");
            ModelState.Remove("UtilizadaP");
            ModelState.Remove("UtilizadaM");
            ModelState.Remove("UtilizadaG");
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
            catch { }
        }
    }
}
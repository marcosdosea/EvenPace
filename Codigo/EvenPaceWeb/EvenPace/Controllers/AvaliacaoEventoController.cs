using AutoMapper;
using Core;
using Microsoft.AspNetCore.Mvc;
using Core.Service;
using Models;

namespace EvenPaceWeb.Controllers
{
    public class AvaliacaoEventoController : Controller
    {
        private readonly IAvaliacaoEventoService _avaliacaoEventoService;
        private readonly IMapper _mapper;

        public AvaliacaoEventoController(IAvaliacaoEventoService avaliacaoEventoService, IMapper mapper)
        {
            _avaliacaoEventoService = avaliacaoEventoService;
            _mapper = mapper;
        }

        /// <summary>
        /// Carrega e lista o histórico completo de feedbacks e avaliações fornecidos por atletas ou parceiros para todos os eventos da plataforma.
        /// </summary>
        /// <returns>Página relacional iterativa listando as métricas e opiniões cadastradas.</returns>
        public ActionResult Index()
        {
            var avaliacoes = _avaliacaoEventoService.GetAll();
            var viewModels = _mapper.Map<List<AvaliacaoEventoViewModel>>(avaliacoes);
            return View(viewModels);
        }

        /// <summary>
        /// Extrai o escopo analítico minucioso focando no parecer singular emitido com base num parâmetro referencial (nome).
        /// </summary>
        /// <param name="nome">Variável textual apontadora do registro referenciado de avaliação.</param>
        /// <returns>Painel em tela com a visualização isolada do descritivo contido na avaliação referida.</returns>
        public ActionResult Details(string nome)
        {
            var avaliacao = _avaliacaoEventoService.GetByName((string)nome);
            var viewModel = _mapper.Map<AvaliacaoEventoViewModel>(avaliacao);
            return View(viewModel);
        }

        /// <summary>
        /// Apresenta uma estrutura sem amarras visuais providenciada para acolher o depoimento, notas ou apontamentos referentes a competições ou interações do núcleo de eventos.
        /// </summary>
        /// <returns>Formato visual desobstruído com campos de submissão do feedback.</returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Acopla os parâmetros submetidos pela experiência de uso em uma instância contendo os apontamentos avaliativos, prosseguindo com sua gravação.
        /// </summary>
        /// <param name="viewModel">Engloba as propriedades qualitativas ou quantitativas expressadas na view form.</param>
        /// <returns>Em ocorrências positivas retorna à janela de compilação geral; senão reescreve os estados devolvendo o modelo rejeitado.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AvaliacaoEventoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var avaliacao = _mapper.Map<Core.AvaliacaoEvento>(viewModel);
                _avaliacaoEventoService.Create(avaliacao);
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        /// <summary>
        /// Habilita o contexto propício à edição dos fatores avaliativos pontuados para eventual mitigação ou consertos interpretativos pelo responsável ou moderador.
        /// </summary>
        /// <param name="id">Chave de acesso numérico contendo a relação primária da avaliação a ser reescrita.</param>
        /// <returns>Janela repopulada visualmente pronta às novas validações opinativas.</returns>
        public ActionResult Edit(int id)
        {
            var avaliacao = _avaliacaoEventoService.Get((int)id);
            var viewModel = _mapper.Map<AvaliacaoEventoViewModel>(avaliacao);
            return View(viewModel);
        }

        /// <summary>
        /// Transpõe as eventuais substituições geradas no preenchimento opinativo de volta à base relacional atrelada de depoimentos e notas do evento em particular.
        /// </summary>
        /// <param name="viewModel">Classe espelho portando os novos atributos textuais e classificatórios formulados.</param>
        /// <returns>Executa a conclusão do processo transportando a navegação para índices atualizados.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AvaliacaoEventoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var avaliacao = _mapper.Map<Core.AvaliacaoEvento>(viewModel);
                _avaliacaoEventoService.Edit(avaliacao);
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        /// <summary>
        /// Demanda o bloqueio temporário e a apresentação confirmativa em modo seguro sobre uma exclusão requisitada ao respectivo registro de avaliação gerado.
        /// </summary>
        /// <param name="id">Referência indexadora exata da entidade avaliativa no SQL.</param>
        /// <returns>Janela de segurança solicitando uma decisão do usuário à destruição dos dados consultados.</returns>
        public ActionResult Delete(int id)
        {
            var avaliacao = _avaliacaoEventoService.Get((int)id);
            var viewModel = _mapper.Map<AvaliacaoEventoViewModel>(avaliacao);
            return View(viewModel);
        }

        /// <summary>
        /// Subtrai de vez a dependência e a entidade referenciando a revisão preexistente dos quadros mantenedores após confirmar integridade de repasse via token preventivo.
        /// </summary>
        /// <param name="id">Código primário sequencial correspondendo à avaliação a ser cortada da listagem do repositório.</param>
        /// <param name="viewModel">Condução sistêmica alinhada pelas métricas do formulário submetido.</param>
        /// <returns>Envia de volta ao ciclo iterativo desprovido do objeto removido recém listado na home principal desta entidade.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, AvaliacaoEventoViewModel viewModel)
        {
            _avaliacaoEventoService.Delete((int)id);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Encapsula a ação alternativa de feedback voltada explicitamente à interface pública, recebendo de pronto as concepções a validar e as enviando diretamente ao painel da Home após submissão.
        /// </summary>
        /// <param name="model">Conjunto contendo os critérios da avaliação (estrelas, comentários) extraídos dinamicamente da interface web.</param>
        /// <returns>Redireciona velozmente os atletas de volta para a seção residencial do site garantindo fluidez contínua na usabilidade sem transitar por painéis de gerenciamento se for bem sucedido.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AvaliarEvento(AvaliacaoEventoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var avaliacao = _mapper.Map<AvaliacaoEvento>(model);
                _avaliacaoEventoService.Create(avaliacao);

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
    }
}
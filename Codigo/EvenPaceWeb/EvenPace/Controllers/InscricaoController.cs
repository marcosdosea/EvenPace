using AutoMapper;
using Core;
using Core.Service;
using Core.Service.Dtos;
using Microsoft.AspNetCore.Mvc;
using EvenPaceWeb.Models;
using Models;

namespace EvenPace.Controllers
{
    public class InscricaoController : Controller
    {
        private readonly IInscricaoService _inscricaoService;
        private readonly IMapper _mapper;

        public InscricaoController(IInscricaoService inscricaoService, IMapper mapper)
        {
            _inscricaoService = inscricaoService;
            _mapper = mapper;
        }

        /// <summary>
        /// Verifica as condições e exibe a interface de confirmação para o cancelamento de uma inscrição específica.
        /// </summary>
        /// <param name="id">O identificador numérico da inscrição.</param>
        /// <returns>A view de exclusão caso permitida; do contrário, redireciona exibindo avisos de erro temporal ou inexistência.</returns>
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var result = _inscricaoService.GetDadosTelaDelete(id);

            if (!result.Success)
            {
                if (result.ErrorType == "NotFound")
                    return NotFound("Inscrição não encontrada");
                TempData["Erro"] = "Não é possível cancelar após a data do evento.";
                return RedirectToAction("Index", "Home");
            }

            var dadosTelaDelete = result.Data!;
            var telaInscricaoViewModel = new TelaInscricaoViewModel
            {
                NomeEvento = dadosTelaDelete.NomeEvento,
                DataEvento = dadosTelaDelete.DataEvento,
                Local = dadosTelaDelete.Local,
                NomeKit = dadosTelaDelete.NomeKit,
                Inscricao = new InscricaoViewModel
                {
                    Id = (uint)dadosTelaDelete.IdInscricao,
                    Distancia = dadosTelaDelete.Distancia,
                    TamanhoCamisa = dadosTelaDelete.TamanhoCamisa,
                    DataInscricao = dadosTelaDelete.DataInscricao
                }
            };

            return View("Delete", telaInscricaoViewModel);
        }

        /// <summary>
        /// Acessa a página central (hub) de detalhes de inscrição de um evento.
        /// </summary>
        /// <param name="id">O código exclusivo do evento alvo da inscrição.</param>
        /// <returns>View de índice contendo as informações consolidadas para inscrição.</returns>
        public IActionResult Index(int id)
        {
            var dto = _inscricaoService.GetDadosTelaInscricao(id);
            var vm = MontarTelaInscricaoViewModel(dto, id);
            return View(vm);
        }

        /// <summary>
        /// Exibe o formulário inicial destinado ao registro da participação no evento.
        /// </summary>
        /// <param name="id">O identificador do evento.</param>
        /// <returns>A view de criação de inscrição.</returns>
        [HttpGet]
        public IActionResult Create(int id)
        {
            var dto = _inscricaoService.GetDadosTelaInscricao(id);
            var vm = MontarTelaInscricaoViewModel(dto, id);
            return View("Create", vm);
        }

        /// <summary>
        /// Efetiva o cancelamento lógico da inscrição após o envio e validação das credenciais do usuário e prazo do evento.
        /// </summary>
        /// <param name="idInscricao">O identificador único do registro de inscrição que será inativado.</param>
        /// <param name="idEvento">O identificador do evento atrelado à inscrição.</param>
        /// <returns>Redireciona para o índice do evento mantendo o alerta de sucesso ou erro do processo.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int idInscricao, int idEvento)
        {
            var idCorredorClaim = User.FindFirst("IdCorredor");
            if (idCorredorClaim == null)
            {
                TempData["Erro"] = "Faça login para cancelar a inscrição.";
                return RedirectToAction("Index", new { id = idEvento });
            }

            try
            {
                _inscricaoService.Cancelar(
                    idInscricao,
                    int.Parse(idCorredorClaim.Value)
                );

                TempData["Sucesso"] = "Inscrição cancelada com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["Erro"] = ex.Message;
            }

            return RedirectToAction("Index", new { id = idEvento });
        }

        /// <summary>
        /// Confirma a geração da inscrição com base nas escolhas de formulário (distância, blusa, kit) vinculadas ao usuário logado.
        /// </summary>
        /// <param name="vm">ViewModel englobando todos os dados selecionados na tela de inscrição do evento.</param>
        /// <returns>Retorna à indexação do evento com mensagens indicativas das ações.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(TelaInscricaoViewModel vm)
        {
            var idCorredorClaim = User.FindFirst("IdCorredor");
            if (idCorredorClaim == null)
            {
                TempData["Erro"] = "Faça login para continuar";
                return RedirectToAction("Index", new { id = vm.Inscricao.IdEvento });
            }

            var inscricao = new Inscricao
            {
                Status = "Pendente",
                DataInscricao = DateTime.Now,
                Distancia = vm.Inscricao.Distancia,
                TamanhoCamisa = vm.Inscricao.TamanhoCamisa,
                IdEvento = (int)vm.Inscricao.IdEvento,
                IdKit = (int)vm.Inscricao.IdKit,
                IdCorredor = int.Parse(idCorredorClaim.Value)
            };

            _inscricaoService.Create(inscricao);

            TempData["Sucesso"] = "Inscrição realizada com sucesso!";
            return RedirectToAction("Index", new { id = vm.Inscricao.IdEvento });
        }

        private TelaInscricaoViewModel MontarTelaInscricaoViewModel(DadosTelaInscricaoDto dto, int idEvento)
        {
            return new TelaInscricaoViewModel
            {
                IdEvento = idEvento,
                NomeEvento = dto.NomeEvento,
                Local = dto.Local,
                DataEvento = dto.DataEvento,
                Descricao = dto.Descricao,
                ImagemEvento = dto.ImagemEvento,
                Percursos = new List<string> { "3km", "5km", "10km" },
                Kits = _mapper.Map<List<KitViewModel>>(dto.Kits),
                Inscricao = new InscricaoViewModel { IdEvento = idEvento }
            };
        }

        /// <summary>
        /// Gera uma listagem contendo todos os participantes regularmente inscritos em um determinado evento.
        /// </summary>
        /// <param name="idEvento">O identificador do evento.</param>
        /// <returns>A view com a lista de modelos de inscritos.</returns>
        public ActionResult GetAllByEvento(int idEvento)
        {
            var inscricao = _inscricaoService.GetAllByEvento(idEvento);
            var inscricaoViewModel = _mapper.Map<List<InscricaoViewModel>>(inscricao);
            return View(inscricaoViewModel);
        }

        /// <summary>
        /// Apresenta o painel de suporte que possibilita aos organizadores monitorar e efetuar o checkout de kits de um evento.
        /// </summary>
        /// <param name="idEvento">O evento relacionado à entrega em andamento.</param>
        /// <returns>View contendo o painel de retiradas do evento.</returns>
        [HttpGet]
        public IActionResult Retirada(int idEvento)
        {
            var inscricoes = _inscricaoService.GetAllByEvento(idEvento);

            var inscricoesViewModel = _mapper.Map<List<InscricaoViewModel>>(inscricoes);

            return View("RetiradaKit", inscricoesViewModel);
        }

        /// <summary>
        /// Altera o status da inscrição confirmando que o participante fez a coleta do kit adquirido.
        /// </summary>
        /// <param name="idInscricao">Identificador único do ingresso/inscrição no sistema.</param>
        /// <param name="idEvento">Identificador do evento pertencente para efeitos de redirecionamento posterior.</param>
        /// <returns>Retorna Ok() em caso de requisição assíncrona, ou redireciona a interface novamente para o painel de entrega.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmarRetirada(int idInscricao, int idEvento)
        {
            _inscricaoService.ConfirmarRetiradaKit(idInscricao);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Ok();
            }

            return RedirectToAction("Retirada", new { idEvento = idEvento });
        }
    }
}
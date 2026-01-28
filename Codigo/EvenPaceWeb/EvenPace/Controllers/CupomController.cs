using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Core.Service;
using Models;

namespace EvenPaceWeb.Controllers
{
    public class CupomController : Controller
    {
        private readonly ICupomService _cupomService;
        private readonly IMapper _mapper;

        public CupomController(ICupomService cupomService, IMapper mapper)
        {
            _cupomService = cupomService;
            _mapper = mapper;
        }

        /// <summary>
        /// Pega todos os cupons cadastrados
        /// </summary>
        /// <returns></returns>        
        public ActionResult Index()
        {
            var cupons = _cupomService.GetAll();
            var cupomViewModels = _mapper.Map<List<CupomViewModel>>(cupons);
            return View(cupomViewModels);
        }


        /// <summary>
        /// Retorna os detalhes de um cupom específicado pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            var cupom = _cupomService.Get((uint)id);

            if (cupom == null) return NotFound();

            var cupomViewModel = _mapper.Map<CupomViewModel>(cupom);
            return View(cupomViewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Cria um novo cupom a partir do cupomViewModel
        /// </summary>
        /// <param name="cupomViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CupomViewModel cupomViewModel)
        {
            if (ModelState.IsValid)
            {
                var cupom = _mapper.Map<Core.Cupom>(cupomViewModel);
                _cupomService.Create(cupom);
                return RedirectToAction(nameof(Index)); // Só redireciona se salvar com sucesso
            }

            // Se o modelo for inválido, devolvemos a ViewModel para a View mostrar os erros
            return View(cupomViewModel);
        }

        /// <summary>
        /// Edita um cupom a partir do cupomViewModel
        /// </summary>
        /// <param name="cupomViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CupomViewModel cupomViewModel)
        {
            if (ModelState.IsValid)
            {
                var cupom = _mapper.Map<Core.Cupom>(cupomViewModel);
                _cupomService.Edit(cupom);
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Retorna o cupom para edição a partir do id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            var cupom = _cupomService.Get((uint)id);

            if (cupom == null) return NotFound();
            
            var cupomViewModel = _mapper.Map<CupomViewModel>(cupom);
            return View(cupomViewModel);
        }

        /// <summary>
        /// Retorna um cupom a partir do id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            var cupom = _cupomService.Get((uint)id);

            if (cupom == null) return NotFound();

            var cupomViewModel = _mapper.Map<CupomViewModel>(cupom);
            return View(cupomViewModel);
        }

        /// <summary>
        /// Deleta um cupom a partir do id e do cupomViewModel
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cupomViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, CupomViewModel cupomViewModel)
        {
            _cupomService.Delete((uint)id);
            return RedirectToAction(nameof(Index));
        }
    }
}

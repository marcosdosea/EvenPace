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

        public ActionResult Index()
        {
            var cupons = _cupomService.GetAll();
            var cupomViewModels = _mapper.Map<List<CupomViewModel>>(cupons);
            return View(cupomViewModels);
        }

        public ActionResult Details(int id)
        {
            var cupom = _cupomService.Get((int)id);
            var cupomViewModel = _mapper.Map<CupomViewModel>(cupom);
            return View(cupomViewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CupomViewModel cupomViewModel)
        {
            if (ModelState.IsValid)
            {
                var cupom = _mapper.Map<Core.Cupom>(cupomViewModel);
                _cupomService.Create(cupom);
                return RedirectToAction(nameof(Index));
            }

            return View(cupomViewModel);
        }

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

        public ActionResult Edit(int id)
        {
            var cupom = _cupomService.Get((int)id);

            var cupomViewModel = _mapper.Map<CupomViewModel>(cupom);
            return View(cupomViewModel);
        }

        public ActionResult Delete(int id)
        {
            var cupom = _cupomService.Get((int)id);

            var cupomViewModel = _mapper.Map<CupomViewModel>(cupom);
            return View(cupomViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, CupomViewModel cupomViewModel)
        {
            _cupomService.Delete((int)id);
            return RedirectToAction(nameof(Index));
        }
    }
}
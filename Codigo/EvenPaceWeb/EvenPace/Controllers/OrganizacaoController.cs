using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Core.Service;
using Models;

namespace EvenPaceWeb.Controllers
{
    public class OrganizacaoController : Controller
    {
        private readonly IOrganizacaoService _organizacaoService;
        private readonly IMapper _mapper;

        public OrganizacaoController(IOrganizacaoService organizacaoService, IMapper mapper)
        {
            _organizacaoService = organizacaoService;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            var organizacoes = _organizacaoService.GetAll();
            var organizacaoViewModels = _mapper.Map<List<OrganizacaoViewModel>>(organizacoes);
            return View(organizacaoViewModels);
        }

        public ActionResult Details(int id)
        {
            var organizacao = _organizacaoService.Get((int)id);
            var organizacaoViewModel = _mapper.Map<OrganizacaoViewModel>(organizacao);
            return View(organizacaoViewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrganizacaoViewModel organizacaoViewModel)
        {
            if (ModelState.IsValid)
            {
                var organizacao = _mapper.Map<Core.Organizacao>(organizacaoViewModel);
                _organizacaoService.Create(organizacao);
                return RedirectToAction(nameof(Index));
            }
            return View(organizacaoViewModel);
        }

        public ActionResult Edit(int id)
        {
            var organizacao = _organizacaoService.Get((int)id);
            var organizacaoViewModel = _mapper.Map<OrganizacaoViewModel>(organizacao);
            return View(organizacaoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrganizacaoViewModel organizacaoViewModel)
        {
            organizacaoViewModel.Id = 3;

            ModelState.Remove("Senha");

            if (ModelState.IsValid)
            {
                var organizacao = _mapper.Map<Core.Organizacao>(organizacaoViewModel);
                _organizacaoService.Edit(organizacao);

                return RedirectToAction(nameof(Index));
            }
            return View(organizacaoViewModel);
        }

        public ActionResult Delete(int id)
        {
            var organizacao = _organizacaoService.Get((int)id);
            var organizacaoViewModel = _mapper.Map<OrganizacaoViewModel>(organizacao);
            return View(organizacaoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, OrganizacaoViewModel organizacaoViewModel)
        {
            _organizacaoService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
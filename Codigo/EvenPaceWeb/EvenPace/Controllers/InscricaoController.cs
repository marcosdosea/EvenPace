using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models;

namespace EvenPace.Controllers;

public class InscricaoController : Controller
{
      private IInscricao _inscricaoService;
      private IMapper _mapper;

      public InscricaoController(IInscricao inscricao, IMapper mapper)
      {
          _inscricaoService = inscricao;
          _mapper = mapper;
      }

      public ActionResult Index()
      {
          var listaIncricao = _inscricaoService.GetAll();
          var listaInscricaoModel = _mapper.Map<List<InscricaoModel>>(listaIncricao);
          return View(listaInscricaoModel);
      }
      
      public ActionResult Details(int id)
      {
          Inscricao inscricao = _inscricaoService.Get(id);
          InscricaoModel inscricaoModel = _mapper.Map<InscricaoModel>(inscricao);
          return View(inscricaoModel);
      }
      
      public ActionResult Create()
      {
          return View();
      }
      
      public ActionResult Create(InscricaoModel inscricaoModel)
      {
          if (ModelState.IsValid)
          {
              var inscricao = _mapper.Map<Inscricao>(inscricaoModel);
              _inscricaoService.Create(inscricao); 
          }
          return View(_inscricaoService);
      }

      public ActionResult Edit(int id)
      {
          Inscricao inscricao = _inscricaoService.Get(id);
          InscricaoModel inscricaoModel = _mapper.Map<InscricaoModel>(inscricao);
          return View(inscricaoModel);
      }
      
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit(int id, InscricaoModel inscricaoModel)
      {
          if (ModelState.IsValid)
          {
              var inscricao = _mapper.Map<Inscricao>(inscricaoModel);
              _inscricaoService.Edit(inscricao);
          }
          return RedirectToAction(nameof(Index));
      }

      public ActionResult Delete(int id)
      {
          Inscricao inscricao = _inscricaoService.Get(id);
          InscricaoModel inscricaoModel = _mapper.Map<InscricaoModel>(inscricao);
          return View(inscricaoModel);
      }
        
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Delete(int id, InscricaoModel inscricaoModel)
      {
        _inscricaoService.Delete(id);
        return RedirectToAction(nameof(Index));
      }
}
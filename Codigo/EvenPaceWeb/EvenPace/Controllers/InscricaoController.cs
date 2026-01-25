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

      // Get: InscricaoController/Index
      public ActionResult Index()
      {
          var listaIncricao = _inscricaoService.GetAll();
          var listaInscricaoModel = _mapper.Map<List<InscricaoViewModel>>(listaIncricao);
          return View(listaInscricaoModel);
      }
      
      // Get: InscricaoController/Get/2
      public ActionResult Get(int id)
      {
          Inscricao inscricao = _inscricaoService.Get(id);
          InscricaoViewModel inscricaoModel = _mapper.Map<InscricaoViewModel>(inscricao);
          return View(inscricaoModel);
      }
      
      // Get: InscricaoController/Create
      public ActionResult Create()
      {
          return View();
      }
      
      // Post: InscricaoController/Create
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create(InscricaoViewModel inscricaoModel)
      {
          if (ModelState.IsValid)
          {
              var inscricao = _mapper.Map<Inscricao>(inscricaoModel);
              _inscricaoService.Create(inscricao); 
          }
          return View(_inscricaoService);
      }

      // Get: InscricaoController/Edit/4
      public ActionResult Edit(int id)
      {
          Inscricao inscricao = _inscricaoService.Get(id);
          InscricaoViewModel inscricaoModel = _mapper.Map<InscricaoViewModel>(inscricao);
          return View(inscricaoModel);
      }
      
      // Post: InscricaoController/Edit/1
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit(int id, InscricaoViewModel inscricaoModel)
      {
          if (ModelState.IsValid)
          {
              var inscricao = _mapper.Map<Inscricao>(inscricaoModel);
              _inscricaoService.Edit(inscricao);
          }
          return RedirectToAction(nameof(Index));
      }
      // Get: InscricaoController/Delete/1
      public ActionResult Delete(int id)
      {
          Inscricao inscricao = _inscricaoService.Get(id);
          InscricaoViewModel inscricaoModel = _mapper.Map<InscricaoViewModel>(inscricao);
          return View(inscricaoModel);
      }
      // Post: InscricaoController/Delete/1    
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Delete(int id, InscricaoViewModel inscricaoModel)
      {
        _inscricaoService.Delete(id);
        return RedirectToAction(nameof(Index));
      }
}
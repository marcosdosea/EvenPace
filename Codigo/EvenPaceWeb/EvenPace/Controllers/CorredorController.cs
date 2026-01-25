using AutoMapper;
using Core;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace EvenPace.Controllers;

public class CorredorController : Controller
{
      private ICorredorService _corredorService;
      private IMapper _mapper;

      public CorredorController(ICorredorService corredor, IMapper mapper)
      {
          _corredorService = corredor;
          _mapper = mapper;
      }

      // Get: CorredorController/Index
      public ActionResult Index()
      {
          var listaCorredor = _corredorService.GetAll();
          var listaCorredorModel = _mapper.Map<List<InscricaoViewModel>>(listaCorredor);
          return View(listaCorredorModel);
      }
      
      // Get: CorredorController/Get/4
      public ActionResult Get(int id)
      {
          Corredor corredor = _corredorService.Get(id);
          CorredorViewModel corredorModel = _mapper.Map<CorredorViewModel>(corredor);
          return View(corredorModel);
      }
      
      // Get: CorredorController/Create
      public ActionResult Create()
      {
          return View();
      }
      
      // Post: CorredorController/Create
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create(CorredorViewModel corredorModel)
      {
          if (ModelState.IsValid)
          {
              var corredor = _mapper.Map<Inscricao>(corredorModel);
              _corredorService.Create(corredor); 
          }
          return View(corredorModel);
      }

      // Get: CorredorController/Edit/5
      public ActionResult Edit(int id)
      {
          Corredor corredor = _corredorService.Get(id);
          CorredorViewModel corredorModel = _mapper.Map<CorredorViewModel>(corredor);
          return View(corredorModel);
      }
      
      // Post: CorredorController/Edit/4
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit(int id, CorredorViewModel corredorModel)
      {
          if (ModelState.IsValid)
          {
              var corredor = _mapper.Map<Inscricao>(corredorModel);
              _corredorService.Edit(corredor);
          }
          return RedirectToAction(nameof(Index));
      }

      // Get: CorredorController/Delite/5
      public ActionResult Delete(int id)
      {
          Corredor corredor = _corredorService.Get(id);
          CorredorViewModel corredorModel = _mapper.Map<CorredorViewModel>(corredor);
          return View(corredorModel);
      }
        
      // Post: CorredorController/Delete/1
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Delete(int id, CorredorViewModel corredorModel)
      { 
          _corredorService.Delete(id);
          return RedirectToAction(nameof(Index));
      }
}
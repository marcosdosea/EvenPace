using AutoMapper;
using Core;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace EvenPace.Controllers;

public class CorredorController : Controller
{
      private ICorredor _corredorService;
      private IMapper _mapper;

      public CorredorController(ICorredor corredor, IMapper mapper)
      {
          _corredorService = corredor;
          _mapper = mapper;
      }

      public ActionResult Index()
      {
          var listaCorredor = _corredorService.GetAll();
          var listaCorredorModel = _mapper.Map<List<InscricaoModel>>(listaCorredor);
          return View(listaCorredorModel);
      }
      
      public ActionResult Details(int id)
      {
          Corredor corredor = _corredorService.Get(id);
          CorredorModel corredorModel = _mapper.Map<CorredorModel>(corredor);
          return View(corredorModel);
      }
      
      public ActionResult Create()
      {
          return View();
      }
      
      public ActionResult Create(CorredorModel corredorModel)
      {
          if (ModelState.IsValid)
          {
              var corredor = _mapper.Map<Inscricao>(corredorModel);
              _corredorService.Create(corredor); 
          }
          return View(corredorModel);
      }

      public ActionResult Edit(int id)
      {
          Corredor corredor = _corredorService.Get(id);
          CorredorModel corredorModel = _mapper.Map<CorredorModel>(corredor);
          return View(corredorModel);
      }
      
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit(int id, CorredorModel corredorModel)
      {
          if (ModelState.IsValid)
          {
              var corredor = _mapper.Map<Inscricao>(corredorModel);
              _corredorService.Edit(corredor);
          }
          return RedirectToAction(nameof(Index));
      }

      public ActionResult Delete(int id)
      {
          Corredor corredor = _corredorService.Get(id);
          CorredorModel corredorModel = _mapper.Map<CorredorModel>(corredor);
          return View(corredorModel);
      }
        
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Delete(int id, CorredorModel corredorModel)
      { 
          _corredorService.Delete(id);
          return RedirectToAction(nameof(Index));
      }
}
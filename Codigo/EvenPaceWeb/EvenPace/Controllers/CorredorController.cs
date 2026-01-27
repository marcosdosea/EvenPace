using AutoMapper;
using Core;
using Core.Service;
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
      
      // Post: CorredorController/Login
      public ActionResult Login(string email, string senha)
      {
          Corredor corredor = _corredorService.Login(email, senha);
          CorredorViewModel corredorModel = _mapper.Map<CorredorViewModel>(corredor);
          return View(corredorModel);
      }
      
      // Get: CorredorController/Login/4
      public ActionResult Get(uint id)
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
              var corredor = _mapper.Map<Corredor>(corredorModel);
              _corredorService.Create(corredor); 
          }
          return View(corredorModel);
      }

      // Get: CorredorController/Edit/5
      public ActionResult Edit(uint id)
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
              var corredor = _mapper.Map<Corredor>(corredorModel);
              _corredorService.Edit(corredor);
          }
          return RedirectToAction(nameof(Index));
      }

      // Get: CorredorController/Delite/5
      public ActionResult Delete(uint id)
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
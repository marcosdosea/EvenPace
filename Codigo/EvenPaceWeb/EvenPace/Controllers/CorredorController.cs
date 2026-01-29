using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace EvenPace.Controllers;

public class CorredorController : Controller
{
      private ICorredorService _corredorService;
      private IAvaliacaoEventoService _avaliacaoEventoService;
      private IMapper _mapper;

      public CorredorController(
          ICorredorService corredor,
          IAvaliacaoEventoService avaliacaoEventoService,
          IMapper mapper)
      {
          _corredorService = corredor;
          _avaliacaoEventoService = avaliacaoEventoService;
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
              var corredor = _mapper.Map<Corredor>(corredorModel);
              _corredorService.Create(corredor); 
          }
          return View(corredorModel);
      }

      // Get: CorredorController/Edit/5
      public ActionResult Edit(int id)
      {
          Console.WriteLine($"[DEBUG] Edit chamado com Id={id}");
          Corredor corredor = _corredorService.Get(id);

          // se vier null, pelo menos não quebra a view
          if (corredor == null)
          {
              return View(new CorredorViewModel());
          }

          // se não vier null, mapear o corredor para o ViewModel e retornar a view
          CorredorViewModel corredorModel = _mapper.Map<CorredorViewModel>(corredor);
          Console.WriteLine($"[DEBUG] ViewModel Edit - Id={corredorModel.Id}, Nome={corredorModel.Nome}, Email={corredorModel.Email}, CPF={corredorModel.CPF}");
          return View(corredorModel);
      }
      
      // Post: CorredorController/Edit/4
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit(CorredorViewModel corredorModel)
      {
          if (ModelState.IsValid)
          {
              var corredor = _mapper.Map<Corredor>(corredorModel);
              _corredorService.Edit(corredor);
              return RedirectToAction(nameof(Get), new { id = corredorModel.Id });
          }

          // Se houver erro de validação, volta para a mesma tela com as mensagens
          return View(corredorModel);
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


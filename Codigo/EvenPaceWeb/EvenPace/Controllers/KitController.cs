using AutoMapper;
using Core;
using EvenPace.Models;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace EvenPace.Controllers;

public class KitController : Controller
{
    private IKits _kitsService;
    private IMapper _mapper;

    public KitController(IKits kits, IMapper mapper)
    {
        _kitsService = kits;
        _mapper = mapper;
    }

    // Get: KitController/Get/1
    public ActionResult Get(int id)
    {
        Kit kit = _kitsService.Get(id);
        KitModel kitModel = _mapper.Map<KitModel>(kit);
        return View(kitModel);
    }

    // Get: KitController/Create
    public ActionResult Create()
    {
        return View();
    }

    // Post: KitController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(KitModel kitModel)
    {
        if (ModelState.IsValid)
        {
            var kit = _mapper.Map<Kit>(kitModel);
            _kitsService.Create(kit);
        }
        return RedirectToAction(nameof(Index));
    }

    // Get: KitController/Edit/1
    public ActionResult Edit(int id)
    {
        Kit kit = _kitsService.Get(id);
        KitModel kitModel = _mapper.Map<KitModel>(kit);
        return View(kitModel);
    }

    // Post: KitController/Edit/1
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(KitModel kitModel)
    {
        if (ModelState.IsValid)
        {
            var kit = _mapper.Map<Kit>(kitModel);
            _kitsService.Edit(kit);
        }
        return RedirectToAction(nameof(Index));
    }

    // Get: KitController/Delete/2
    public ActionResult Delete(int id)
    {
        Kit kit = _kitsService.Get(id);
        KitModel kitModel = _mapper.Map<KitModel>(kit);
        return View(kitModel);
    }

    // Post: KitController/Delete/1
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, KitModel kitModel)
    {
        _kitsService.Delete(id);
        return RedirectToAction(nameof(Index));
    }
    
}
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

    public ActionResult Details(int id)
    {
        Kit kit = _kitsService.Get(id);
        KitModel kitModel = _mapper.Map<KitModel>(kit);
        return View(kitModel);
    }

    public ActionResult Create()
    {
        return View();
    }

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

    public ActionResult Edit(int id)
    {
        Kit kit = _kitsService.Get(id);
        KitModel kitModel = _mapper.Map<KitModel>(kit);
        return View(kitModel);
    }

    public ActionResult Edit(KitModel kitModel)
    {
        if (ModelState.IsValid)
        {
            var kit = _mapper.Map<Kit>(kitModel);
            _kitsService.Edit(kit);
        }
        return RedirectToAction(nameof(Index));
    }

    public ActionResult Delete(int id)
    {
        Kit kit = _kitsService.Get(id);
        KitModel kitModel = _mapper.Map<KitModel>(kit);
        return View(kitModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, KitModel kitModel)
    {
        _kitsService.Delete(id);
        return RedirectToAction(nameof(Index));
    }
    
}
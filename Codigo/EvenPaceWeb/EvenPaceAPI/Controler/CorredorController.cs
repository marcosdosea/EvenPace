using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace EvenPaceAPI.Controler;

[Route("api/[controller]")]
[ApiController]
public class CorredorController : ControllerBase
{
    private readonly ICorredorService _corredorService;
    private readonly IMapper _mapper;

    public CorredorController(ICorredorService corredorService, IMapper mapper)
    {
        _corredorService = corredorService;
        _mapper = mapper;
    }
        
    [HttpGet]
    public ActionResult Get()
    {
        var corredors = _corredorService.GetAll();
        return Ok(corredors);
    }
        
    [HttpGet("{id}")]
    public ActionResult Get(int id)
    {
        Corredor corredor = _corredorService.Get(id);
        return Ok(corredor);
    }
        
    [HttpPost]
    public ActionResult Post([FromBody] CorredorViewModel corredorViewModel)
    {
        var corredor = _mapper.Map<Corredor>(corredorViewModel);
        _corredorService.Create(corredor);
        return Ok();
    }
    
    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] CorredorViewModel  corredorViewModel)
    {
        var corredor = _mapper.Map<Corredor>(corredorViewModel);
        if (corredor == null)
            return NotFound();
            
        _corredorService.Edit(corredor);
        return Ok();
    }
        
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        _corredorService.Delete(id);
        return Ok();
    }
}
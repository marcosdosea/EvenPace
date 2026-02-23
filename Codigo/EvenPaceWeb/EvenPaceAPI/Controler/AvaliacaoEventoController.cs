using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace EvenPaceAPI.Controler;

[Route("api/[controller]")]
[ApiController]
public class AvaliacaoEventoController : ControllerBase
{
    private readonly IAvaliacaoEventoService _avaliacaoService;
    private readonly IMapper _mapper;

    public AvaliacaoEventoController(IAvaliacaoEventoService avaliacaoService, IMapper mapper)
    {
        _avaliacaoService = avaliacaoService;
        _mapper = mapper;
    }

    [HttpPost]
    public ActionResult Post([FromBody] AvaliacaoViewModel avaliacaoViewModel)
    {
        var avaliacao = _mapper.Map<AvaliacaoEvento>(avaliacaoViewModel);
        _avaliacaoService.Create(avaliacao);
        return Ok();
    }
}

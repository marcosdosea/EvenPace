using AutoMapper;
using Core;
using Core.Service;
using EvenPaceAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EvenPaceAPI.Controler;

[Route("api/[controller]")]
[ApiController]
public class InscricaoController : ControllerBase
{
    private readonly IInscricaoService _inscricaoService;
    private readonly IMapper _mapper;

    public InscricaoController(IInscricaoService inscricaoService, IMapper mapper)
    {
        _inscricaoService = inscricaoService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Inscricao>>> Get()
    {
        var inscricoes = await _inscricaoService.GetAllAsync();
        return Ok(inscricoes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Inscricao>> Get(int id)
    {
        var inscricao = await _inscricaoService.GetAsync(id);

        if (inscricao is null)
            return NotFound();

        return Ok(inscricao);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] InscricaoViewModel model)
    {
        var inscricao = _mapper.Map<Inscricao>(model);
        var idGerado = await _inscricaoService.CreateAsync(inscricao);

        return CreatedAtAction(nameof(Get), new { id = idGerado }, inscricao);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] InscricaoViewModel model)
    {
        var inscricao = await _inscricaoService.GetAsync(id);

        if (inscricao is null)
            return NotFound();

        inscricao.Status = model.Status;
        inscricao.Distancia = model.Distancia;
        inscricao.TamanhoCamisa = model.TamanhoCamisa;
        inscricao.DataInscricao = model.DataInscricao;
        inscricao.IdEvento = model.IdEvento;
        inscricao.IdCorredor = model.IdCorredor;
        inscricao.IdKit = model.IdKit;
        inscricao.StatusRetiradaKit = model.StatusRetiradaKit;
        inscricao.Tempo = model.Tempo;
        inscricao.Posicao = model.Posicao;
        inscricao.IdAvaliacaoEvento = model.IdAvaliacaoEvento;

        await _inscricaoService.EditAsync(inscricao);

        return Ok(inscricao);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _inscricaoService.DeleteAsync(id);
        return Ok();
    }
}

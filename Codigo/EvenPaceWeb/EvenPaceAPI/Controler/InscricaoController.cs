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
    public ActionResult Get()
    {
        var inscricoes = _inscricaoService.GetAll();
        return Ok(inscricoes);
    }

    [HttpGet("{id}")]
    public ActionResult Get(int id)
    {
        var inscricao = _inscricaoService.Get(id);

        if (inscricao == null)
            return NotFound();

        return Ok(inscricao);
    }

    [HttpPost]
    public ActionResult Post([FromBody] InscricaoViewModel model)
    {
        var inscricao = _mapper.Map<Inscricao>(model);

        var idGerado = _inscricaoService.Create(inscricao);

        return CreatedAtAction(nameof(Get), new { id = idGerado }, inscricao);
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] InscricaoViewModel model)
    {
        var inscricao = _inscricaoService.Get(id);

        if (inscricao == null)
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

        _inscricaoService.Edit(inscricao);

        return Ok(inscricao);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        _inscricaoService.Delete(id);
        return Ok();
    }

}
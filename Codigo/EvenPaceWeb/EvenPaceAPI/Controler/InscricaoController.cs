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

    // GET: api/Inscricao
    [HttpGet]
    public ActionResult Get()
    {
        var inscricoes = _inscricaoService.GetAll();
        return Ok(inscricoes);
    }

    // GET: api/Inscricao/5
    [HttpGet("{id}")]
    public ActionResult Get(int id)
    {
        var inscricao = _inscricaoService.Get(id);

        if (inscricao == null)
            return NotFound();

        return Ok(inscricao);
    }

    // POST: api/Inscricao
    [HttpPost]
    public ActionResult Post([FromBody] InscricaoViewModel model)
    {
        var inscricao = _mapper.Map<Inscricao>(model);

        var idGerado = _inscricaoService.Create(inscricao);

        return CreatedAtAction(nameof(Get), new { id = idGerado }, inscricao);
    }

    // PUT: api/Inscricao/5
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

    // DELETE: api/Inscricao/5
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        _inscricaoService.Delete(id);
        return Ok();
    }

    // GET: api/Inscricao/dados-tela/3
    [HttpGet("dados-tela/{idEvento}")]
    public ActionResult GetDadosTela(int idEvento)
    {
        var dados = _inscricaoService.GetDadosTelaInscricao(idEvento);
        return Ok(dados);
    }
}
using AutoMapper;
using Core;
using Core.Service;
using Core.Service.Dtos;
using EvenPaceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Models;

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

    // GET: api/Inscricao/evento/3
    [HttpGet("evento/{idEvento}")]
    public ActionResult GetByEvento(int idEvento)
    {
        var inscricoes = _inscricaoService.GetAllByEvento(idEvento);
        return Ok(inscricoes);
    }

    // POST: api/Inscricao
    [HttpPost]
    public ActionResult Post([FromBody] InscricaoViewModel inscricaoViewModel)
    {
        var inscricao = _mapper.Map<Inscricao>(inscricaoViewModel);

        var idGerado = _inscricaoService.Create(inscricao);

        return CreatedAtAction(nameof(Get),new { id = idGerado }, inscricao           );
    }

    // PUT: api/Inscricao/5
    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] InscricaoViewModel inscricaoViewModel)
    {
        var inscricao = _mapper.Map<Inscricao>(inscricaoViewModel);

        if (inscricao == null)
            return NotFound();

        _inscricaoService.Edit(inscricao);

        return Ok();
    }

    // DELETE: api/Inscricao/5
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        _inscricaoService.Delete(id);
        return Ok();
    }

    // POST: api/Inscricao/5/cancelar/3
    [HttpPost("{idInscricao}/cancelar/{idCorredor}")]
    public ActionResult Cancelar(int idInscricao, int idCorredor)
    {
        try
        {
            _inscricaoService.Cancelar(idInscricao, idCorredor);
            return Ok("Inscrição cancelada com sucesso.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // POST: api/Inscricao/5/confirmar-retirada
    [HttpPost("{idInscricao}/confirmar-retirada")]
    public ActionResult ConfirmarRetirada(int idInscricao)
    {
        _inscricaoService.ConfirmarRetiradaKit(idInscricao);
        return Ok("Retirada confirmada.");
    }

    // GET: api/Inscricao/dados-tela/3
    [HttpGet("dados-tela/{idEvento}")]
    public ActionResult GetDadosTela(int idEvento)
    {
        var dados = _inscricaoService.GetDadosTelaInscricao(idEvento);
        return Ok(dados);
    }

    // GET: api/Inscricao/dados-delete/5
    [HttpGet("dados-delete/{idInscricao}")]
    public ActionResult GetDadosDelete(int idInscricao)
    {
        var result = _inscricaoService.GetDadosTelaDelete(idInscricao);

        if (!result.Success)
            return BadRequest(result.ErrorType);

        return Ok(result.Data);
    }
}
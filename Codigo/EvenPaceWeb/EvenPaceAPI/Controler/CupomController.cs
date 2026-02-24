using Core.Service;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Service;
using Core;
using Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EvenPaceAPI.Controler
{
    [Route("api/[controller]")]
    [ApiController]
    public class CupomController : ControllerBase
    {
        private readonly ICupomService _cupomService;
        private readonly IMapper _mapper;

        public CupomController(ICupomService cupomService, IMapper mapper)
        {
            _cupomService = cupomService;
            _mapper = mapper;
        }
      
        [HttpGet]
        public ActionResult Get()
        {
            var cupons = _cupomService.GetAll();
            return Ok(cupons);
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            Cupom cupom = _cupomService.Get(id);
            return Ok(cupom);
        }

        [HttpPost]
        public ActionResult Post([FromBody] CupomViewModel cupomViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("Dados inválidos.");

            var cupom = _mapper.Map<Cupom>(cupomViewModel);
            _cupomService.Create(cupom);
            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] CupomViewModel cupomModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("Dados inválidos.");

            var cupom = _mapper.Map<Cupom>(cupomModel);
            if (cupom == null)
                return NotFound("Cupom não encontrado.");

            _cupomService.Edit(cupom);

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Cupom? cupom = _cupomService.Get(id);

            if (cupom == null)
                return NotFound("Cupom não encontrado.");

            _cupomService.Delete(id);
            return Ok();
        }
    }
}

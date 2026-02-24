using Microsoft.AspNetCore.Mvc;
using Core.Service;
using Core;

[Route("api/[controller]")]
[ApiController]
public class KitController : ControllerBase
{
    private readonly IKitService _kitService;

    public KitController(IKitService kitService)
    {
        _kitService = kitService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Kit>> Get() => Ok(_kitService.GetAll());

    [HttpGet("{id}")]
    public ActionResult<Kit> Get(int id)
    {
        var kit = _kitService.Get(id);
        if (kit == null) return NotFound();
        return Ok(kit);
    }

    [HttpPost]
    public IActionResult Post([FromBody] Kit kit)
    {
        var id = _kitService.Create(kit);
        return CreatedAtAction(nameof(Get), new { id = id }, kit);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Kit kit)
    {
        if (id != kit.Id) return BadRequest();
        _kitService.Edit(kit);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _kitService.Delete(id);
        return NoContent();
    }
}
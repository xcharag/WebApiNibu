using Microsoft.AspNetCore.Mvc;
using WebApiNibu.Data.Dto;
using WebApiNibu.Services.Interface;
using WebApiNibu.Services.Interface.Commands;
using WebApiNibu.Services.Interface.Queries;

namespace WebApiNibu.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController(IContact service) : ControllerBase
{
    // GET: api/Contact
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContactReadDto>>> GetAll([FromQuery] ContactQuery query, CancellationToken ct)
        => Ok(await service.QueryAsync(query, ct));

    // GET: api/Contact/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ContactReadDto>> GetById(int id, CancellationToken ct)
    {
        var item = await service.GetByIdAsync(id, ct);
        return item is null ? NotFound() : Ok(item);
    }

    // POST: api/Contact
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContactCommand command, CancellationToken ct)
    {
        var result = await service.CreateAsync(command, ct);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Message, errors = result.Errors });

        var created = result.Value!;
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT: api/Contact/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateContactCommand command, CancellationToken ct)
    {
        var result = await service.UpdateAsync(id, command, ct);
        if (result.IsSuccess) return NoContent();

        if (string.Equals(result.Message, "Not found", StringComparison.OrdinalIgnoreCase))
            return NotFound(new { message = result.Message, errors = result.Errors });

        return BadRequest(new { message = result.Message, errors = result.Errors });
    }

    // DELETE: api/Contact/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var result = await service.DeleteAsync(id, soft, ct);
        if (result.IsSuccess) return NoContent();

        return NotFound(new { message = result.Message, errors = result.Errors });
    }
}

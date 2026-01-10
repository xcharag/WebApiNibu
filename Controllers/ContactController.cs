using Microsoft.AspNetCore.Mvc;
using WebApiNibu.Data.Dto;
using WebApiNibu.Services.Interface;
using WebApiNibu.Services.Interface.Commands;
using WebApiNibu.Services.Interface.Common;
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
    public async Task<ActionResult<ContactReadDto>> Create([FromBody] CreateContactCommand command, CancellationToken ct)
    {
        try
        {
            var created = await service.CreateAsync(command, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (DomainValidationException ex)
        {
            return BadRequest(new { message = ex.Message, errors = ex.Errors });
        }
    }

    // PUT: api/Contact/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateContactCommand command, CancellationToken ct)
    {
        var updated = await service.UpdateAsync(id, command, ct);
        return updated ? NoContent() : NotFound();
    }

    // DELETE: api/Contact/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var deleted = await service.DeleteAsync(id, soft, ct);
        return deleted ? NoContent() : NotFound();
    }
}

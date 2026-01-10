using Microsoft.AspNetCore.Mvc;
using WebApiNibu.Data.Dto;
using WebApiNibu.Services.Interface;
using WebApiNibu.Services.Interface.Queries;

namespace WebApiNibu.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController(IContact service) : ControllerBase
{
    // GET: api/Contact
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContactReadDto>>> GetAll([FromQuery] ContactQuery query, CancellationToken ct)
    {
        var items = await service.QueryAsync(query, ct);
        return Ok(items);
    }

    // GET: api/Contact/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ContactReadDto>> GetById(int id, CancellationToken ct)
    {
        var item = await service.GetByIdAsync(id, ct);
        return item is null ? NotFound() : Ok(item);
    }

    // POST: api/Contact?schoolId=123
    [HttpPost]
    public async Task<ActionResult<ContactReadDto>> Create([FromQuery] int schoolId, [FromBody] ContactCreateDto dto, CancellationToken ct)
    {
        try
        {
            var created = await service.CreateAsync(schoolId, dto, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT: api/Contact/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ContactUpdateDto dto, CancellationToken ct)
    {
        var updated = await service.UpdateAsync(id, dto, ct);
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

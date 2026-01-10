using Microsoft.AspNetCore.Mvc;
using WebApiNibu.Data.Dto;
using WebApiNibu.Services.Interface;
using WebApiNibu.Services.Interface.Queries;

namespace WebApiNibu.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchoolStudentController(ISchoolStudent service) : ControllerBase
{
    // GET: api/SchoolStudent
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SchoolStudentReadDto>>> GetAll([FromQuery] SchoolStudentQuery query, CancellationToken ct)
    {
        var items = await service.QueryAsync(query, ct);
        return Ok(items);
    }

    // GET: api/SchoolStudent/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<SchoolStudentReadDto>> GetById(int id, CancellationToken ct)
    {
        var item = await service.GetByIdAsync(id, ct);
        return item is null ? NotFound() : Ok(item);
    }

    // POST: api/SchoolStudent
    [HttpPost]
    public async Task<ActionResult<SchoolStudentReadDto>> Create([FromBody] SchoolStudentCreateDto dto, CancellationToken ct)
    {
        try
        {
            var created = await service.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT: api/SchoolStudent/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] SchoolStudentUpdateDto dto, CancellationToken ct)
    {
        try
        {
            var updated = await service.UpdateAsync(id, dto, ct);
            return updated ? NoContent() : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE: api/SchoolStudent/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var deleted = await service.DeleteAsync(id, soft, ct);
        return deleted ? NoContent() : NotFound();
    }
}

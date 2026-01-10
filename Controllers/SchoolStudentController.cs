using Microsoft.AspNetCore.Mvc;
using WebApiNibu.Data.Dto;
using WebApiNibu.Services.Interface;
using WebApiNibu.Services.Interface.Commands;
using WebApiNibu.Services.Interface.Queries;

namespace WebApiNibu.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchoolStudentController(ISchoolStudent service) : ControllerBase
{
    // GET: api/SchoolStudent
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SchoolStudentReadDto>>> GetAll([FromQuery] SchoolStudentQuery query, CancellationToken ct)
        => Ok(await service.QueryAsync(query, ct));

    // GET: api/SchoolStudent/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<SchoolStudentReadDto>> GetById(int id, CancellationToken ct)
    {
        var item = await service.GetByIdAsync(id, ct);
        return item is null ? NotFound() : Ok(item);
    }

    // POST: api/SchoolStudent
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSchoolStudentCommand command, CancellationToken ct)
    {
        var result = await service.CreateAsync(command, ct);
        if (!result.IsSuccess)
            return BadRequest(new { message = result.Message, errors = result.Errors });

        var created = result.Value!;
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT: api/SchoolStudent/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSchoolStudentCommand command, CancellationToken ct)
    {
        var result = await service.UpdateAsync(id, command, ct);
        if (result.IsSuccess) return NoContent();

        // Convention: "Not found" -> 404
        if (string.Equals(result.Message, "Not found", StringComparison.OrdinalIgnoreCase))
            return NotFound(new { message = result.Message, errors = result.Errors });

        return BadRequest(new { message = result.Message, errors = result.Errors });
    }

    // DELETE: api/SchoolStudent/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var result = await service.DeleteAsync(id, soft, ct);
        if (result.IsSuccess) return NoContent();

        return NotFound(new { message = result.Message, errors = result.Errors });
    }
}

using Microsoft.AspNetCore.Mvc;
using WebApiNibu.Data.Dto;
using WebApiNibu.Services.Interface;
using WebApiNibu.Services.Interface.Commands;
using WebApiNibu.Services.Interface.Common;
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
    public async Task<ActionResult<SchoolStudentReadDto>> Create([FromBody] CreateSchoolStudentCommand command, CancellationToken ct)
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

    // PUT: api/SchoolStudent/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSchoolStudentCommand command, CancellationToken ct)
    {
        try
        {
            var updated = await service.UpdateAsync(id, command, ct);
            return updated ? NoContent() : NotFound();
        }
        catch (DomainValidationException ex)
        {
            return BadRequest(new { message = ex.Message, errors = ex.Errors });
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

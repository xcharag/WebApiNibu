using Microsoft.AspNetCore.Mvc;
using WebApiNibu.Data.Dto;
using WebApiNibu.Services.Interface;
using WebApiNibu.Services.Interface.Commands;
using WebApiNibu.Services.Interface.Queries;

namespace WebApiNibu.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchoolTableController(ISchoolTable service) : ControllerBase
{
    // GET: api/SchoolTable
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SchoolTableReadDto>>> GetAll([FromQuery] SchoolTableQuery query, CancellationToken ct)
        => Ok(await service.QueryAsync(query, ct));

    // GET: api/SchoolTable/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<SchoolTableReadDto>> GetById(
        int id,
        [FromQuery] bool includeContacts = true,
        [FromQuery] bool includeStudents = true,
        CancellationToken ct = default)
    {
        var item = await service.GetByIdAsync(id, includeContacts, includeStudents, ct);
        return item is null ? NotFound() : Ok(item);
    }

    // POST: api/SchoolTable
    [HttpPost]
    public async Task<ActionResult<SchoolTableReadDto>> Create([FromBody] CreateSchoolTableCommand command, CancellationToken ct)
    {
        var created = await service.CreateAsync(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT: api/SchoolTable/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSchoolTableCommand command, CancellationToken ct)
    {
        var updated = await service.UpdateAsync(id, command, ct);
        return updated ? NoContent() : NotFound();
    }

    // DELETE: api/SchoolTable/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var deleted = await service.DeleteAsync(id, soft, ct);
        return deleted ? NoContent() : NotFound();
    }
}

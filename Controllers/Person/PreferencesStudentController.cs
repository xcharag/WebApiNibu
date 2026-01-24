using Microsoft.AspNetCore.Mvc;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Services.Contract;

namespace WebApiNibu.Controllers.Person;

[ApiController]
[Route("api/[controller]")]
public class PreferencesStudentController(IPreferencesStudent service) : ControllerBase
{
    // GET: api/PreferencesStudent
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await service.GetAllAsync(ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }

    // GET: api/PreferencesStudent/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await service.GetByIdAsync(id, ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Errors);
    }

    // GET: api/PreferencesStudent/filter
    [HttpGet("filter")]
    public async Task<IActionResult> GetFiltered([FromQuery] PreferencesStudentFilter filter, CancellationToken ct)
    {
        var result = await service.GetFilteredAsync(filter, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }

    // POST: api/PreferencesStudent
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PreferencesStudentCreateDto dto, CancellationToken ct)
    {
        var result = await service.CreateAsync(dto, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value)
            : BadRequest(result.Errors);
    }

    // PUT: api/PreferencesStudent/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] PreferencesStudentUpdateDto dto, CancellationToken ct)
    {
        var result = await service.UpdateAsync(id, dto, ct);
        return result.IsSuccess ? NoContent() : NotFound(result.Errors);
    }

    // DELETE: api/PreferencesStudent/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var result = await service.DeleteAsync(id, soft, ct);
        return result.IsSuccess ? NoContent() : NotFound(result.Errors);
    }
}

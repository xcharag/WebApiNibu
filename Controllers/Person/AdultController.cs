using Microsoft.AspNetCore.Mvc;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Services.Contract;

namespace WebApiNibu.Controllers.Person;

[ApiController]
[Route("api/[controller]")]
public class AdultController(IAdult service) : ControllerBase
{
    // GET: api/Adult
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await service.GetAllAsync(ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }

    // GET: api/Adult/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await service.GetByIdAsync(id, ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Errors);
    }

    // GET: api/Adult/filter
    [HttpGet("filter")]
    public async Task<IActionResult> GetFiltered([FromQuery] AdultFilter filter, CancellationToken ct)
    {
        var result = await service.GetFilteredAsync(filter, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }

    // POST: api/Adult
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AdultCreateDto dto, CancellationToken ct)
    {
        var result = await service.CreateAsync(dto, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value)
            : BadRequest(result.Errors);
    }

    // PUT: api/Adult/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] AdultUpdateDto dto, CancellationToken ct)
    {
        var result = await service.UpdateAsync(id, dto, ct);
        return result.IsSuccess ? NoContent() : NotFound(result.Errors);
    }

    // DELETE: api/Adult/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var result = await service.DeleteAsync(id, soft, ct);
        return result.IsSuccess ? NoContent() : NotFound(result.Errors);
    }
}

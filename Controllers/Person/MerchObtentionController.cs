using Microsoft.AspNetCore.Mvc;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.Person;

namespace WebApiNibu.Controllers.Person;

[ApiController]
[Route("api/[controller]")]
public class MerchObtentionController(IMerchObtention service) : ControllerBase
{
    // GET: api/MerchObtention
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] MerchObtentionFilter filter,
        [FromQuery] PaginationParams pagination,
        CancellationToken ct)
    {
        var result = await service.GetAllAsync(filter, pagination, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }

    // GET: api/MerchObtention/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await service.GetByIdAsync(id, ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Errors);
    }

    // POST: api/MerchObtention
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MerchObtentionCreateDto dto, CancellationToken ct)
    {
        var result = await service.CreateAsync(dto, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value)
            : BadRequest(result.Errors);
    }

    // PUT: api/MerchObtention/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] MerchObtentionCreateDto dto, CancellationToken ct)
    {
        var result = await service.UpdateAsync(id, dto, ct);
        return result.IsSuccess ? NoContent() : NotFound(result.Errors);
    }

    // DELETE: api/MerchObtention/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var result = await service.DeleteAsync(id, soft, ct);
        return result.IsSuccess ? NoContent() : NotFound(result.Errors);
    }
}

using Microsoft.AspNetCore.Mvc;
using WebApiNibu.Data.Dto.UsersAndAccess;
using WebApiNibu.Data.Dto.UsersAndAccess.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.UsersAndAccess;

namespace WebApiNibu.Controllers.UsersAndAccess;

[ApiController]
[Route("api/[controller]")]
public class QrAccessController(IQrAccess service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] QrAccessFilter filter,
        [FromQuery] PaginationParams pagination,
        CancellationToken ct)
    {
        var result = await service.GetAllAsync(filter, pagination, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await service.GetByIdAsync(id, ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Errors);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] QrAccessCreateDto dto, CancellationToken ct)
    {
        var result = await service.CreateAsync(dto, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value)
            : BadRequest(result.Errors);
    }

    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] QrAccessGenerateDto dto, CancellationToken ct)
    {
        var result = await service.GenerateAsync(dto, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value)
            : BadRequest(result.Errors);
    }

    [HttpPost("generate-mark-used")]
    public async Task<IActionResult> GenerateMarkUsedQr([FromBody] QrAccessGenerateDto dto, CancellationToken ct)
    {
        var result = await service.GenerateMarkUsedQrAsync(dto, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value)
            : BadRequest(result.Errors);
    }

    [HttpPost("validate")]
    public async Task<IActionResult> Validate([FromBody] QrValidateDto dto, CancellationToken ct)
    {
        var result = await service.ValidateAsync(dto, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] QrAccessUpdateDto dto, CancellationToken ct)
    {
        var result = await service.UpdateAsync(id, dto, ct);
        return result.IsSuccess ? NoContent() : NotFound(result.Errors);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var result = await service.DeleteAsync(id, soft, ct);
        return result.IsSuccess ? NoContent() : NotFound(result.Errors);
    }
}

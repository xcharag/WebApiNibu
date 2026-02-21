using Microsoft.AspNetCore.Mvc;
using WebApiNibu.Data.Dto.Feed.News;
using WebApiNibu.Data.Dto.Feed.News.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.Feed.News;

namespace WebApiNibu.Controllers.Feed.News;

[ApiController]
[Route("api/[controller]")]
public class NewsDetailController(INewsDetail service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] NewsDetailFilter filter,
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
    public async Task<IActionResult> Create([FromBody] NewsDetailCreateDto dto, CancellationToken ct)
    {
        var result = await service.CreateAsync(dto, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value)
            : BadRequest(result.Errors);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] NewsDetailUpdateDto dto, CancellationToken ct)
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

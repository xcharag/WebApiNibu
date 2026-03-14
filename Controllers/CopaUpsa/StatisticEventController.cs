using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiNibu.Authorization;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.CopaUpsa;

namespace WebApiNibu.Controllers.CopaUpsa;

[ApiController]
[Route("api/[controller]")]
public class StatisticEventController(IStatisticEvent service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] StatisticEventFilter filter, [FromQuery] PaginationParams pagination, CancellationToken ct)
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

    [HttpGet("timeline")]
    public async Task<IActionResult> GetTimeline(
        [FromQuery] int? matchId,
        [FromQuery] int? tournamentId,
        [FromQuery] int? statisticId,
        CancellationToken ct)
    {
        var result = await service.GetTimelineAsync(matchId, tournamentId, statisticId, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }

    [HttpGet("ranking")]
    public async Task<IActionResult> GetRanking(
        [FromQuery] int statisticId,
        [FromQuery] int? tournamentId,
        [FromQuery] int top = 10,
        CancellationToken ct = default)
    {
        var result = await service.GetRankingAsync(statisticId, tournamentId, top, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }

    [HttpPost]
    [Authorize]
    [DynamicPermission]
    public async Task<IActionResult> Create([FromBody] StatisticEventCreateDto dto, CancellationToken ct)
    {
        var result = await service.CreateAsync(dto, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value)
            : BadRequest(result.Errors);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    [DynamicPermission]
    public async Task<IActionResult> Update(int id, [FromBody] StatisticEventUpdateDto dto, CancellationToken ct)
    {
        var result = await service.UpdateAsync(id, dto, ct);
        return result.IsSuccess ? NoContent() : NotFound(result.Errors);
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    [DynamicPermission]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var result = await service.DeleteAsync(id, soft, ct);
        return result.IsSuccess ? NoContent() : NotFound(result.Errors);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiNibu.Authorization;
using WebApiNibu.Data.Dto;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.CopaUpsa;

namespace WebApiNibu.Controllers.CopaUpsa;

[ApiController]
[Route("api/[controller]")]
public class MatchController(IMatch service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] MatchFilter filter, [FromQuery] PaginationParams pagination, CancellationToken ct)
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

    [HttpGet("{id:int}/detail")]
    public async Task<IActionResult> GetDetail(int id, CancellationToken ct)
    {
        var result = await service.GetDetailAsync(id, ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Errors);
    }

    [HttpGet("schedule")]
    public async Task<IActionResult> GetSchedule(
        [FromQuery] DateTime? startDateFrom,
        [FromQuery] DateTime? startDateTo,
        [FromQuery] int? tournamentId,
        CancellationToken ct)
    {
        var result = await service.GetScheduleAsync(startDateFrom, startDateTo, tournamentId, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }

    [HttpGet("available-dates")]
    public async Task<IActionResult> GetAvailableDates(
        [FromQuery] int? tournamentId,
        CancellationToken ct)
    {
        var result = await service.GetAvailableDatesAsync(tournamentId, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }

    [HttpGet("standings")]
    public async Task<IActionResult> GetStandings(
        [FromQuery] int tournamentId,
        [FromQuery] int? phaseTypeId,
        CancellationToken ct)
    {
        var result = await service.GetStandingsAsync(tournamentId, phaseTypeId, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }

    [HttpPost]
    [Authorize]
    [DynamicPermission]
    public async Task<IActionResult> Create([FromBody] MatchCreateDto dto, CancellationToken ct)
    {
        var result = await service.CreateAsync(dto, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value)
            : BadRequest(result.Errors);
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [Authorize]
    [DynamicPermission]
    public async Task<IActionResult> UploadFromExcel([FromForm] FileUploadDto dto, CancellationToken ct)
    {
        var result = await service.UploadFromExcel(dto.File, ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(result.Errors);
    }

    [HttpPost("upload-results")]
    [Consumes("multipart/form-data")]
    [Authorize]
    [DynamicPermission]
    public async Task<IActionResult> UploadResultsFromExcel([FromForm] FileUploadDto dto, CancellationToken ct)
    {
        var result = await service.UploadResultsFromExcel(dto.File, ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(result.Errors);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    [DynamicPermission]
    public async Task<IActionResult> Update(int id, [FromBody] MatchUpdateDto dto, CancellationToken ct)
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

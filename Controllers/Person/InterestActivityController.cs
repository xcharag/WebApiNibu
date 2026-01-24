using Microsoft.AspNetCore.Mvc;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Entity.Person;

namespace WebApiNibu.Controllers.Person;

[ApiController]
[Route("api/[controller]")]
public class InterestActivityController(IBaseCrud<InterestActivity> service) : ControllerBase
{
    // GET: api/InterestActivity
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InterestActivitieReadDto>>> GetAll(CancellationToken ct)
    {
        var items = await service.GetAllAsync(true, ct);
        var dtos = items.Select(MapToReadDto).ToList();
        return Ok(dtos);
    }

    // GET: api/InterestActivity/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<InterestActivitieReadDto>> GetById(int id, CancellationToken ct)
    {
        var item = await service.GetByIdAsync(id, true, ct);
        return item is null ? NotFound() : Ok(MapToReadDto(item));
    }

    // POST: api/InterestActivity
    [HttpPost]
    public async Task<ActionResult<InterestActivitieReadDto>> Create([FromBody] InterestActivitieCreateDto dto, CancellationToken ct)
    {
        var entity = MapFromCreateDto(dto);
        var created = await service.CreateAsync(entity, ct);
        var readDto = MapToReadDto(created);
        return CreatedAtAction(nameof(GetById), new { id = readDto.Id }, readDto);
    }

    // PUT: api/InterestActivity/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] InterestActivitieUpdateDto dto, CancellationToken ct)
    {
        var updated = await service.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated ? NoContent() : NotFound();
    }

    // DELETE: api/InterestActivity/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var deleted = await service.DeleteAsync(id, soft, ct);
        return deleted ? NoContent() : NotFound();
    }

    private static InterestActivitieReadDto MapToReadDto(InterestActivity activity) => new()
    {
        Id = activity.Id,
        Name = activity.Name,
        Description = activity.Description,
        Icon = activity.Icon
    };

    private static InterestActivity MapFromCreateDto(InterestActivitieCreateDto dto) => new()
    {
        Name = dto.Name,
        Description = dto.Description,
        Icon = dto.Icon,
        Active = true
    };

    private static void ApplyUpdateDto(InterestActivity target, InterestActivitieUpdateDto dto)
    {
        target.Name = dto.Name;
        target.Description = dto.Description;
        target.Icon = dto.Icon;
    }
}

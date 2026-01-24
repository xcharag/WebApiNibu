using Microsoft.AspNetCore.Mvc;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Entity.Person;

namespace WebApiNibu.Controllers.Person;

[ApiController]
[Route("api/[controller]")]
public class AdultTypeController(IBaseCrud<AdultType> service) : ControllerBase
{
    // GET: api/AdultType
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AdultTypeReadDto>>> GetAll(CancellationToken ct)
    {
        var items = await service.GetAllAsync(true, ct);
        var dtos = items.Select(MapToReadDto).ToList();
        return Ok(dtos);
    }

    // GET: api/AdultType/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AdultTypeReadDto>> GetById(int id, CancellationToken ct)
    {
        var item = await service.GetByIdAsync(id, true, ct);
        return item is null ? NotFound() : Ok(MapToReadDto(item));
    }

    // POST: api/AdultType
    [HttpPost]
    public async Task<ActionResult<AdultTypeReadDto>> Create([FromBody] AdultTypeCreateDto dto, CancellationToken ct)
    {
        var entity = MapFromCreateDto(dto);
        var created = await service.CreateAsync(entity, ct);
        var readDto = MapToReadDto(created);
        return CreatedAtAction(nameof(GetById), new { id = readDto.Id }, readDto);
    }

    // PUT: api/AdultType/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] AdultTypeUpdateDto dto, CancellationToken ct)
    {
        var updated = await service.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated ? NoContent() : NotFound();
    }

    // DELETE: api/AdultType/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var deleted = await service.DeleteAsync(id, soft, ct);
        return deleted ? NoContent() : NotFound();
    }

    private static AdultTypeReadDto MapToReadDto(AdultType type) => new()
    {
        Id = type.Id,
        Name = type.Name
    };

    private static AdultType MapFromCreateDto(AdultTypeCreateDto dto) => new()
    {
        Name = dto.Name,
        Active = true
    };

    private static void ApplyUpdateDto(AdultType target, AdultTypeUpdateDto dto)
    {
        target.Name = dto.Name;
    }
}

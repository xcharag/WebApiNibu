using Microsoft.AspNetCore.Mvc;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Entity.Person;

namespace WebApiNibu.Controllers.Person;

[ApiController]
[Route("api/[controller]")]
public class MerchTypeController(IBaseCrud<MerchType> service) : ControllerBase
{
    // GET: api/MerchType
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MerchTypeReadDto>>> GetAll(CancellationToken ct)
    {
        var items = await service.GetAllAsync(true, ct);
        var dtos = items.Select(MapToReadDto).ToList();
        return Ok(dtos);
    }

    // GET: api/MerchType/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<MerchTypeReadDto>> GetById(int id, CancellationToken ct)
    {
        var item = await service.GetByIdAsync(id, true, ct);
        return item is null ? NotFound() : Ok(MapToReadDto(item));
    }

    // POST: api/MerchType
    [HttpPost]
    public async Task<ActionResult<MerchTypeReadDto>> Create([FromBody] MerchTypeCreateDto dto, CancellationToken ct)
    {
        var entity = MapFromCreateDto(dto);
        var created = await service.CreateAsync(entity, ct);
        var readDto = MapToReadDto(created);
        return CreatedAtAction(nameof(GetById), new { id = readDto.Id }, readDto);
    }

    // PUT: api/MerchType/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] MerchTypeUpdateDto dto, CancellationToken ct)
    {
        var updated = await service.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated ? NoContent() : NotFound();
    }

    // DELETE: api/MerchType/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var deleted = await service.DeleteAsync(id, soft, ct);
        return deleted ? NoContent() : NotFound();
    }

    private static MerchTypeReadDto MapToReadDto(MerchType merchType) => new()
    {
        Id = merchType.Id,
        Name = merchType.Name
    };

    private static MerchType MapFromCreateDto(MerchTypeCreateDto dto) => new()
    {
        Name = dto.Name,
        Active = true
    };

    private static void ApplyUpdateDto(MerchType target, MerchTypeUpdateDto dto)
    {
        target.Name = dto.Name;
    }
}

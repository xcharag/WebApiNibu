using Microsoft.AspNetCore.Mvc;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Entity.Person;

namespace WebApiNibu.Controllers.Person;

[ApiController]
[Route("api/[controller]")]
public class CarreerController(IBaseCrud<Carreer> service) : ControllerBase
{
    // GET: api/Carreer
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CarreerReadDto>>> GetAll(CancellationToken ct)
    {
        var items = await service.GetAllAsync(true, ct);
        var dtos = items.Select(MapToReadDto).ToList();
        return Ok(dtos);
    }

    // GET: api/Carreer/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CarreerReadDto>> GetById(int id, CancellationToken ct)
    {
        var item = await service.GetByIdAsync(id, true, ct);
        return item is null ? NotFound() : Ok(MapToReadDto(item));
    }

    // POST: api/Carreer
    [HttpPost]
    public async Task<ActionResult<CarreerReadDto>> Create([FromBody] CarreerCreateDto dto, CancellationToken ct)
    {
        var entity = MapFromCreateDto(dto);
        var created = await service.CreateAsync(entity, ct);
        var readDto = MapToReadDto(created);
        return CreatedAtAction(nameof(GetById), new { id = readDto.Id }, readDto);
    }

    // PUT: api/Carreer/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CarreerUpdateDto dto, CancellationToken ct)
    {
        var updated = await service.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated ? NoContent() : NotFound();
    }

    // DELETE: api/Carreer/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var deleted = await service.DeleteAsync(id, soft, ct);
        return deleted ? NoContent() : NotFound();
    }

    private static CarreerReadDto MapToReadDto(Carreer carreer) => new()
    {
        Id = carreer.Id,
        Name = carreer.Name,
        BannerImage = carreer.BannerImage,
        Description = carreer.Description,
        AreaFormacionId = carreer.AreaFormacionId,
        CodCarrRel = carreer.CodCarrRel,
        OrdenEventos = carreer.OrdenEventos
    };

    private static Carreer MapFromCreateDto(CarreerCreateDto dto) => new()
    {
        Name = dto.Name,
        BannerImage = dto.BannerImage ?? string.Empty,
        Description = dto.Description,
        AreaFormacionId = dto.AreaFormacionId,
        CodCarrRel = dto.CodCarrRel,
        OrdenEventos = dto.OrdenEventos,
        Active = true
    };

    private static void ApplyUpdateDto(Carreer target, CarreerUpdateDto dto)
    {
        target.Name = dto.Name;
        target.BannerImage = dto.BannerImage ?? string.Empty;
        target.Description = dto.Description;
        target.AreaFormacionId = dto.AreaFormacionId;
        target.CodCarrRel = dto.CodCarrRel;
        target.OrdenEventos = dto.OrdenEventos;
    }
}

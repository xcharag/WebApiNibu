using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Entity.Person;

namespace WebApiNibu.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UniversityController : ControllerBase
{
    private readonly IBaseCrud<University> _service;

    public UniversityController(IBaseCrud<University> service)
    {
        _service = service;
    }

    // GET: api/University
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UniversityReadDto>>> GetAll(CancellationToken ct)
    {
        var items = await _service.GetAllAsync(true, ct);
        var dtos = items.Select(MapToReadDto).ToList();
        return Ok(dtos);
    }

    // GET: api/University/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UniversityReadDto>> GetById(int id, CancellationToken ct)
    {
        var item = await _service.GetByIdAsync(id, true, ct);
        return item is null ? NotFound() : Ok(MapToReadDto(item));
    }

    // POST: api/University
    [HttpPost]
    public async Task<ActionResult<UniversityReadDto>> Create([FromBody] UniversityCreateDto dto, CancellationToken ct)
    {
        var entity = MapFromCreateDto(dto);
        var created = await _service.CreateAsync(entity, ct);
        var readDto = MapToReadDto(created);
        return CreatedAtAction(nameof(GetById), new { id = readDto.Id }, readDto);
    }

    // PUT: api/University/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UniversityUpdateDto dto, CancellationToken ct)
    {
        var updated = await _service.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated ? NoContent() : NotFound();
    }

    // DELETE: api/University/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var deleted = await _service.DeleteAsync(id, soft, ct);
        return deleted ? NoContent() : NotFound();
    }

    private static UniversityReadDto MapToReadDto(University university) => new()
    {
        Id = university.Id,
        Name = university.Name,
        Sigla = university.Sigla ?? string.Empty,
        Dpto = university.Dpto ?? string.Empty,
        IdEventos = university.IdEventos ?? 0,
        OrdenEventos = university.OrdenEventos ?? 0,
        NivelCompetencia = university.NivelCompetencia ?? string.Empty
    };

    private static University MapFromCreateDto(UniversityCreateDto dto) => new()
    {
        Name = dto.Name,
        Sigla = dto.Sigla,
        Dpto = dto.Dpto,
        IdEventos = dto.IdEventos,
        OrdenEventos = dto.OrdenEventos,
        NivelCompetencia = dto.NivelCompetencia,
        Active = true
    };

    private static void ApplyUpdateDto(University target, UniversityUpdateDto dto)
    {
        target.Name = dto.Name;
        target.Sigla = dto.Sigla;
        target.Dpto = dto.Dpto;
        target.IdEventos = dto.IdEventos;
        target.OrdenEventos = dto.OrdenEventos;
        target.NivelCompetencia = dto.NivelCompetencia;
    }
}

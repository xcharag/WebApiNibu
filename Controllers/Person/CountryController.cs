using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Entity.Person;

namespace WebApiNibu.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountryController : ControllerBase
{
    private readonly IBaseCrud<Country> _service;

    public CountryController(IBaseCrud<Country> service)
    {
        _service = service;
    }

    // GET: api/Country
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CountryReadDto>>> GetAll(CancellationToken ct)
    {
        var items = await _service.GetAllAsync(true, ct);
        var dtos = items.Select(MapToReadDto).ToList();
        return Ok(dtos);
    }

    // GET: api/Country/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CountryReadDto>> GetById(int id, CancellationToken ct)
    {
        var item = await _service.GetByIdAsync(id, true, ct);
        return item is null ? NotFound() : Ok(MapToReadDto(item));
    }

    // POST: api/Country
    [HttpPost]
    public async Task<ActionResult<CountryReadDto>> Create([FromBody] CountryCreateDto dto, CancellationToken ct)
    {
        var entity = MapFromCreateDto(dto);
        var created = await _service.CreateAsync(entity, ct);
        var readDto = MapToReadDto(created);
        return CreatedAtAction(nameof(GetById), new { id = readDto.Id }, readDto);
    }

    // PUT: api/Country/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CountryUpdateDto dto, CancellationToken ct)
    {
        var updated = await _service.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated ? NoContent() : NotFound();
    }

    // DELETE: api/Country/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var deleted = await _service.DeleteAsync(id, soft, ct);
        return deleted ? NoContent() : NotFound();
    }

    private static CountryReadDto MapToReadDto(Country country) => new()
    {
        Id = country.Id,
        Name = country.Name
    };

    private static Country MapFromCreateDto(CountryCreateDto dto) => new()
    {
        Name = dto.Name,
        Active = true
    };

    private static void ApplyUpdateDto(Country target, CountryUpdateDto dto)
    {
        target.Name = dto.Name;
    }
}

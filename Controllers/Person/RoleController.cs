using Microsoft.AspNetCore.Mvc;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Entity.Person;

namespace WebApiNibu.Controllers.Person;

[ApiController]
[Route("api/[controller]")]
public class RoleController(IBaseCrud<Role> service) : ControllerBase
{
    // GET: api/Role
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleReadDto>>> GetAll(CancellationToken ct)
    {
        var items = await service.GetAllAsync(true, ct);
        var dtos = items.Select(MapToReadDto).ToList();
        return Ok(dtos);
    }

    // GET: api/Role/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<RoleReadDto>> GetById(int id, CancellationToken ct)
    {
        var item = await service.GetByIdAsync(id, true, ct);
        return item is null ? NotFound() : Ok(MapToReadDto(item));
    }

    // POST: api/Role
    [HttpPost]
    public async Task<ActionResult<RoleReadDto>> Create([FromBody] RoleCreateDto dto, CancellationToken ct)
    {
        var entity = MapFromCreateDto(dto);
        var created = await service.CreateAsync(entity, ct);
        var readDto = MapToReadDto(created);
        return CreatedAtAction(nameof(GetById), new { id = readDto.Id }, readDto);
    }

    // PUT: api/Role/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] RoleUpdateDto dto, CancellationToken ct)
    {
        var updated = await service.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated ? NoContent() : NotFound();
    }

    // DELETE: api/Role/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var deleted = await service.DeleteAsync(id, soft, ct);
        return deleted ? NoContent() : NotFound();
    }

    private static RoleReadDto MapToReadDto(Role role) => new()
    {
        Id = role.Id,
        Name = role.Name,
        Department = role.Department
    };

    private static Role MapFromCreateDto(RoleCreateDto dto) => new()
    {
        Name = dto.Name,
        Department = dto.Department,
        Active = true
    };

    private static void ApplyUpdateDto(Role target, RoleUpdateDto dto)
    {
        target.Name = dto.Name;
        target.Department = dto.Department;
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Entity.Person;

namespace WebApiNibu.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkerController : ControllerBase
{
    private readonly IBaseCrud<Worker> _service;
    private readonly OracleDbContext _db;

    public WorkerController(IBaseCrud<Worker> service, OracleDbContext db)
    {
        _service = service;
        _db = db;
    }

    // GET: api/Worker
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkerReadDto>>> GetAll(CancellationToken ct)
    {
        var items = await _service.GetAllAsync(true, ct);
        var dtos = items.Select(MapToReadDto).ToList();
        return Ok(dtos);
    }

    // GET: api/Worker/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<WorkerReadDto>> GetById(int id, CancellationToken ct)
    {
        var item = await _service.GetByIdAsync(id, true, ct);
        return item is null ? NotFound() : Ok(MapToReadDto(item));
    }

    // POST: api/Worker
    [HttpPost]
    public async Task<ActionResult<WorkerReadDto>> Create([FromBody] WorkerCreateDto dto, CancellationToken ct)
    {
        var fkErrors = await ValidateFksAsync(dto.RoleId, ct);
        if (fkErrors.Count > 0)
        {
            return BadRequest(new { message = "Invalid foreign keys", errors = fkErrors });
        }

        var entity = MapFromCreateDto(dto);
        var created = await _service.CreateAsync(entity, ct);
        var readDto = MapToReadDto(created);
        return CreatedAtAction(nameof(GetById), new { id = readDto.Id }, readDto);
    }

    // PUT: api/Worker/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] WorkerUpdateDto dto, CancellationToken ct)
    {
        var fkErrors = await ValidateFksAsync(dto.RoleId, ct);
        if (fkErrors.Count > 0)
        {
            return BadRequest(new { message = "Invalid foreign keys", errors = fkErrors });
        }

        var updated = await _service.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated ? NoContent() : NotFound();
    }

    // DELETE: api/Worker/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var deleted = await _service.DeleteAsync(id, soft, ct);
        return deleted ? NoContent() : NotFound();
    }

    private async Task<List<string>> ValidateFksAsync(int roleId, CancellationToken ct)
    {
        var errors = new List<string>();
        if (!await _db.Roles.AnyAsync(r => r.Id == roleId, ct))
        {
            errors.Add($"RoleId ({roleId}) not found");
        }
        return errors;
    }

    private static WorkerReadDto MapToReadDto(Worker worker) => new()
    {
        Id = worker.Id,
        WorkEmail = worker.WorkEmail,
        RoleId = worker.IdRole
    };

    private static Worker MapFromCreateDto(WorkerCreateDto dto) => new()
    {
        WorkEmail = dto.WorkEmail,
        IdRole = dto.RoleId,
        Active = true,
        Role = null!,
        Country = null!,
        DoucmentType = null!
    };

    private static void ApplyUpdateDto(Worker target, WorkerUpdateDto dto)
    {
        target.WorkEmail = dto.WorkEmail;
        target.IdRole = dto.RoleId;
    }
}

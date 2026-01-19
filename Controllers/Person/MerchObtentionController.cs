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
public class MerchObtentionController : ControllerBase
{
    private readonly IBaseCrud<MerchObtention> _service;
    private readonly OracleDbContext _db;

    public MerchObtentionController(IBaseCrud<MerchObtention> service, OracleDbContext db)
    {
        _service = service;
        _db = db;
    }

    // GET: api/MerchObtention
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MerchObtentionReadDto>>> GetAll(CancellationToken ct)
    {
        var items = await _service.GetAllAsync(true, ct);
        var dtos = items.Select(MapToReadDto).ToList();
        return Ok(dtos);
    }

    // GET: api/MerchObtention/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<MerchObtentionReadDto>> GetById(int id, CancellationToken ct)
    {
        var item = await _service.GetByIdAsync(id, true, ct);
        return item is null ? NotFound() : Ok(MapToReadDto(item));
    }

    // POST: api/MerchObtention
    [HttpPost]
    public async Task<ActionResult<MerchObtentionReadDto>> Create([FromBody] MerchObtentionCreateDto dto, CancellationToken ct)
    {
        var fkErrors = await ValidateFksAsync(dto.SchoolStudentId, dto.MerchId, ct);
        if (fkErrors.Count > 0)
        {
            return BadRequest(new { message = "Invalid foreign keys", errors = fkErrors });
        }

        var entity = MapFromCreateDto(dto);
        var created = await _service.CreateAsync(entity, ct);
        var readDto = MapToReadDto(created);
        return CreatedAtAction(nameof(GetById), new { id = readDto.Id }, readDto);
    }

    // PUT: api/MerchObtention/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] MerchObtentionCreateDto dto, CancellationToken ct)
    {
        var fkErrors = await ValidateFksAsync(dto.SchoolStudentId, dto.MerchId, ct);
        if (fkErrors.Count > 0)
        {
            return BadRequest(new { message = "Invalid foreign keys", errors = fkErrors });
        }

        var updated = await _service.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated ? NoContent() : NotFound();
    }

    // DELETE: api/MerchObtention/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var deleted = await _service.DeleteAsync(id, soft, ct);
        return deleted ? NoContent() : NotFound();
    }

    private async Task<List<string>> ValidateFksAsync(int schoolStudentId, int merchId, CancellationToken ct)
    {
        var errors = new List<string>();
        if (!await _db.SchoolStudents.AnyAsync(s => s.Id == schoolStudentId, ct))
        {
            errors.Add($"SchoolStudentId ({schoolStudentId}) not found");
        }
        if (!await _db.Merchs.AnyAsync(m => m.Id == merchId, ct))
        {
            errors.Add($"MerchId ({merchId}) not found");
        }
        return errors;
    }

    private static MerchObtentionReadDto MapToReadDto(MerchObtention obtention) => new()
    {
        Id = obtention.Id,
        Reason = obtention.Reason,
        Claimed = obtention.Claimed,
        SchoolStudentId = obtention.IdSchoolStudent,
        MerchId = obtention.IdMerch
    };

    private static MerchObtention MapFromCreateDto(MerchObtentionCreateDto dto) => new()
    {
        Reason = dto.Reason ?? string.Empty,
        Claimed = dto.Claimed,
        IdSchoolStudent = dto.SchoolStudentId,
        IdMerch = dto.MerchId,
        Active = true,
        SchoolStudent = null!,
        Merch = null!
    };

    private static void ApplyUpdateDto(MerchObtention target, MerchObtentionCreateDto dto)
    {
        target.Reason = dto.Reason ?? string.Empty;
        target.Claimed = dto.Claimed;
        target.IdSchoolStudent = dto.SchoolStudentId;
        target.IdMerch = dto.MerchId;
    }
}

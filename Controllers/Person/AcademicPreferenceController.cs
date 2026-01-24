using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Entity.Person;

namespace WebApiNibu.Controllers.Person;

[ApiController]
[Route("api/[controller]")]
public class AcademicPreferenceController(IBaseCrud<AcademicPreference> service, OracleDbContext db) : ControllerBase
{
    // GET: api/AcademicPreference
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AcademicPreferenceReadDto>>> GetAll(CancellationToken ct)
    {
        var items = await service.GetAllAsync(true, ct);
        var dtos = items.Select(MapToReadDto).ToList();
        return Ok(dtos);
    }

    // GET: api/AcademicPreference/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AcademicPreferenceReadDto>> GetById(int id, CancellationToken ct)
    {
        var item = await service.GetByIdAsync(id, true, ct);
        return item is null ? NotFound() : Ok(MapToReadDto(item));
    }

    // POST: api/AcademicPreference
    [HttpPost]
    public async Task<ActionResult<AcademicPreferenceReadDto>> Create([FromBody] AcademicPreferenceCreateDto dto, CancellationToken ct)
    {
        var fkErrors = await ValidateFksAsync(dto.UniversityId, dto.CarreerId, dto.PreferencesStudentId, ct);
        if (fkErrors.Count > 0)
        {
            return BadRequest(new { message = "Invalid foreign keys", errors = fkErrors });
        }

        var entity = MapFromCreateDto(dto);
        var created = await service.CreateAsync(entity, ct);
        var readDto = MapToReadDto(created);
        return CreatedAtAction(nameof(GetById), new { id = readDto.Id }, readDto);
    }

    // PUT: api/AcademicPreference/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] AcademicPreferenceUpdateDto dto, CancellationToken ct)
    {
        var fkErrors = await ValidateFksAsync(dto.UniversityId, dto.CarreerId, dto.PreferencesStudentId, ct);
        if (fkErrors.Count > 0)
        {
            return BadRequest(new { message = "Invalid foreign keys", errors = fkErrors });
        }

        var updated = await service.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated ? NoContent() : NotFound();
    }

    // DELETE: api/AcademicPreference/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var deleted = await service.DeleteAsync(id, soft, ct);
        return deleted ? NoContent() : NotFound();
    }

    private async Task<List<string>> ValidateFksAsync(int? universityId, int? carreerId, int preferencesStudentId, CancellationToken ct)
    {
        var errors = new List<string>();

        if (universityId is null)
        {
            errors.Add("UniversityId is required");
        }
        else if (!await db.Universities.AnyAsync(u => u.Id == universityId, ct))
        {
            errors.Add($"UniversityId ({universityId}) not found");
        }

        if (carreerId is null)
        {
            errors.Add("CarreerId is required");
        }
        else if (!await db.Carreers.AnyAsync(c => c.Id == carreerId, ct))
        {
            errors.Add($"CarreerId ({carreerId}) not found");
        }

        if (!await db.PreferencesStudents.AnyAsync(p => p.Id == preferencesStudentId, ct))
        {
            errors.Add($"PreferencesStudentId ({preferencesStudentId}) not found");
        }

        return errors;
    }

    private static AcademicPreferenceReadDto MapToReadDto(AcademicPreference preference) => new()
    {
        Id = preference.Id,
        UniversityId = preference.IdUniversitiy,
        CarreerId = preference.IdCarreer,
        PreferencesStudentId = preference.IdPreferencesStudent
    };

    private static AcademicPreference MapFromCreateDto(AcademicPreferenceCreateDto dto) => new()
    {
        IdUniversitiy = dto.UniversityId ?? 0,
        IdCarreer = dto.CarreerId ?? 0,
        IdPreferencesStudent = dto.PreferencesStudentId,
        Active = true,
        PreferencesStudent = null!,
        Universitiy = null!,
        Carreer = null!
    };

    private static void ApplyUpdateDto(AcademicPreference target, AcademicPreferenceUpdateDto dto)
    {
        target.IdUniversitiy = dto.UniversityId ?? target.IdUniversitiy;
        target.IdCarreer = dto.CarreerId ?? target.IdCarreer;
        target.IdPreferencesStudent = dto.PreferencesStudentId;
    }
}

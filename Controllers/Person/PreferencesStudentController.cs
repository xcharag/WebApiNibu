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
public class PreferencesStudentController : ControllerBase
{
    private readonly IBaseCrud<PreferencesStudent> _service;
    private readonly OracleDbContext _db;

    public PreferencesStudentController(IBaseCrud<PreferencesStudent> service, OracleDbContext db)
    {
        _service = service;
        _db = db;
    }

    // GET: api/PreferencesStudent
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PreferencesStudentReadDto>>> GetAll(CancellationToken ct)
    {
        var items = await _service.GetAllAsync(true, ct);
        var dtos = items.Select(MapToReadDto).ToList();
        return Ok(dtos);
    }

    // GET: api/PreferencesStudent/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PreferencesStudentReadDto>> GetById(int id, CancellationToken ct)
    {
        var item = await _service.GetByIdAsync(id, true, ct);
        return item is null ? NotFound() : Ok(MapToReadDto(item));
    }

    // POST: api/PreferencesStudent
    [HttpPost]
    public async Task<ActionResult<PreferencesStudentReadDto>> Create([FromBody] PreferencesStudentCreateDto dto, CancellationToken ct)
    {
        var fkErrors = await ValidateFksAsync(dto.SchoolStudentId, ct);
        if (fkErrors.Count > 0)
        {
            return BadRequest(new { message = "Invalid foreign keys", errors = fkErrors });
        }

        var entity = MapFromCreateDto(dto);
        var created = await _service.CreateAsync(entity, ct);
        var readDto = MapToReadDto(created);
        return CreatedAtAction(nameof(GetById), new { id = readDto.Id }, readDto);
    }

    // PUT: api/PreferencesStudent/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] PreferencesStudentUpdateDto dto, CancellationToken ct)
    {
        var fkErrors = await ValidateFksAsync(dto.SchoolStudentId, ct);
        if (fkErrors.Count > 0)
        {
            return BadRequest(new { message = "Invalid foreign keys", errors = fkErrors });
        }

        var updated = await _service.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated ? NoContent() : NotFound();
    }

    // DELETE: api/PreferencesStudent/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var deleted = await _service.DeleteAsync(id, soft, ct);
        return deleted ? NoContent() : NotFound();
    }

    private async Task<List<string>> ValidateFksAsync(int schoolStudentId, CancellationToken ct)
    {
        var errors = new List<string>();
        if (!await _db.SchoolStudents.AnyAsync(s => s.Id == schoolStudentId, ct))
        {
            errors.Add($"SchoolStudentId ({schoolStudentId}) not found");
        }
        return errors;
    }

    private static PreferencesStudentReadDto MapToReadDto(PreferencesStudent preferences) => new()
    {
        Id = preferences.Id,
        HaveVocationalTest = preferences.HaveVocationalTest,
        StudyAbroad = preferences.StudyAbroad,
        WhereHadTest = (WebApiNibu.Data.Dto.Person.WhereHadTest?)preferences.WhereHadTest,
        LevelInformation = (WebApiNibu.Data.Dto.Person.LevelInformation?)preferences.LevelInformation,
        SchoolStudentId = preferences.IdSchoolStudent
    };

    private static PreferencesStudent MapFromCreateDto(PreferencesStudentCreateDto dto) => new()
    {
        HaveVocationalTest = dto.HaveVocationalTest,
        StudyAbroad = dto.StudyAbroad,
        WhereHadTest = dto.WhereHadTest.HasValue
            ? (WebApiNibu.Data.Enum.WhereHadTest)dto.WhereHadTest.Value
            : WebApiNibu.Data.Enum.WhereHadTest.School,
        LevelInformation = dto.LevelInformation.HasValue
            ? (WebApiNibu.Data.Enum.LevelInformation)dto.LevelInformation.Value
            : WebApiNibu.Data.Enum.LevelInformation.Little,
        IdSchoolStudent = dto.SchoolStudentId,
        Active = true,
        SchoolStudent = null!
    };

    private static void ApplyUpdateDto(PreferencesStudent target, PreferencesStudentUpdateDto dto)
    {
        target.HaveVocationalTest = dto.HaveVocationalTest;
        target.StudyAbroad = dto.StudyAbroad;
        target.WhereHadTest = dto.WhereHadTest.HasValue
            ? (WebApiNibu.Data.Enum.WhereHadTest)dto.WhereHadTest.Value
            : target.WhereHadTest;
        target.LevelInformation = dto.LevelInformation.HasValue
            ? (WebApiNibu.Data.Enum.LevelInformation)dto.LevelInformation.Value
            : target.LevelInformation;
        target.IdSchoolStudent = dto.SchoolStudentId;
    }
}

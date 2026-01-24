using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Entity.Person;

namespace WebApiNibu.Controllers.Person;

[ApiController]
[Route("api/[controller]")]
public class SchoolStudentController(IBaseCrud<SchoolStudent> service, OracleDbContext db) : ControllerBase
{

    // GET: api/SchoolStudent
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SchoolStudentReadDto>>> GetAll(CancellationToken ct)
    {
        var items = await service.GetAllAsync(true, ct);
        var dtos = items.Select(MapToReadDto).ToList();
        return Ok(dtos);
    }

    // GET: api/SchoolStudent/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<SchoolStudentReadDto>> GetById(int id, CancellationToken ct)
    {
        var item = await service.GetByIdAsync(id, true, ct);
        return item is null ? NotFound() : Ok(MapToReadDto(item));
    }

    // POST: api/SchoolStudent
    [HttpPost]
    public async Task<ActionResult<SchoolStudentReadDto>> Create([FromBody] SchoolStudentCreateDto dto, CancellationToken ct)
    {
        var fkErrors = await ValidateFksAsync(dto.IdCountry, dto.IdDocumentType, dto.IdSchool, ct);
        if (fkErrors.Count > 0)
        {
            return BadRequest(new { message = "Invalid foreign keys", errors = fkErrors });
        }

        var entity = MapFromCreateDto(dto);
        var created = await service.CreateAsync(entity, ct);
        var readDto = MapToReadDto(created);
        return CreatedAtAction(nameof(GetById), new { id = readDto.Id }, readDto);
    }

    // PUT: api/SchoolStudent/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] SchoolStudentUpdateDto dto, CancellationToken ct)
    {
        var fkErrors = await ValidateFksAsync(dto.IdCountry, dto.IdDocumentType, dto.IdSchool, ct);
        if (fkErrors.Count > 0)
        {
            return BadRequest(new { message = "Invalid foreign keys", errors = fkErrors });
        }

        var updated = await service.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated ? NoContent() : NotFound();
    }

    // DELETE: api/SchoolStudent/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var deleted = await service.DeleteAsync(id, soft, ct);
        return deleted ? NoContent() : NotFound();
    }

    private async Task<List<string>> ValidateFksAsync(int idCountry, int idDocumentType, int idSchool, CancellationToken ct)
    {
        var errors = new List<string>();
        if (!await db.Countries.AnyAsync(c => c.Id == idCountry, ct))
        {
            errors.Add($"IdCountry ({idCountry}) not found");
        }
        if (!await db.DocumentTypes.AnyAsync(d => d.Id == idDocumentType, ct))
        {
            errors.Add($"IdDocumentType ({idDocumentType}) not found");
        }
        if (!await db.Schools.AnyAsync(s => s.Id == idSchool, ct))
        {
            errors.Add($"IdSchool ({idSchool}) not found");
        }
        return errors;
    }

    private static SchoolStudentReadDto MapToReadDto(SchoolStudent s) => new()
    {
        Id = s.Id,
        FirstName = s.FirstName,
        MiddleName = s.MiddleName,
        PaternalSurname = s.PaternalSurname,
        MaternalSurname = s.MaternalSurname,
        DocumentNumber = s.DocumentNumber,
        BirthDate = s.BirthDate,
        PhoneNumber = s.PhoneNumber,
        Email = s.Email,
        IdCountry = s.IdCountry,
        IdDocumentType = s.IdDocumentType,
        IdSchool = s.IdSchool,
        SchoolGrade = s.SchoolGrade,
        IsPlayer = s.IsPlayer,
        HasUpsaParents = s.HasUpsaParents
    };

    private static SchoolStudent MapFromCreateDto(SchoolStudentCreateDto dto) => new()
    {
        // Person
        FirstName = dto.FirstName,
        MiddleName = dto.MiddleName,
        PaternalSurname = dto.PaternalSurname,
        MaternalSurname = dto.MaternalSurname,
        DocumentNumber = dto.DocumentNumber,
        BirthDate = dto.BirthDate,
        PhoneNumber = dto.PhoneNumber,
        Email = dto.Email,
        IdCountry = dto.IdCountry,
        IdDocumentType = dto.IdDocumentType,
        // Student
        IdSchool = dto.IdSchool,
        SchoolGrade = dto.SchoolGrade,
        IsPlayer = dto.IsPlayer,
        HasUpsaParents = dto.HasUpsaParents,
        Active = true
    };

    private static void ApplyUpdateDto(SchoolStudent target, SchoolStudentUpdateDto dto)
    {
        // Person
        target.FirstName = dto.FirstName;
        target.MiddleName = dto.MiddleName;
        target.PaternalSurname = dto.PaternalSurname;
        target.MaternalSurname = dto.MaternalSurname;
        target.DocumentNumber = dto.DocumentNumber;
        target.BirthDate = dto.BirthDate;
        target.PhoneNumber = dto.PhoneNumber;
        target.Email = dto.Email;
        target.IdCountry = dto.IdCountry;
        target.IdDocumentType = dto.IdDocumentType;
        // Student
        target.IdSchool = dto.IdSchool;
        target.SchoolGrade = dto.SchoolGrade;
        target.IsPlayer = dto.IsPlayer;
        target.HasUpsaParents = dto.HasUpsaParents;
    }
}

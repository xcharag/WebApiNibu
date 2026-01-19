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
public class AdultController : ControllerBase
{
    private readonly IBaseCrud<Adult> _service;
    private readonly OracleDbContext _db;

    public AdultController(IBaseCrud<Adult> service, OracleDbContext db)
    {
        _service = service;
        _db = db;
    }

    // GET: api/Adult
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AdultReadDto>>> GetAll(CancellationToken ct)
    {
        var items = await _service.GetAllAsync(true, ct);
        var dtos = items.Select(MapToReadDto).ToList();
        return Ok(dtos);
    }

    // GET: api/Adult/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AdultReadDto>> GetById(int id, CancellationToken ct)
    {
        var item = await _service.GetByIdAsync(id, true, ct);
        return item is null ? NotFound() : Ok(MapToReadDto(item));
    }

    // POST: api/Adult
    [HttpPost]
    public async Task<ActionResult<AdultReadDto>> Create([FromBody] AdultCreateDto dto, CancellationToken ct)
    {
        var fkErrors = await ValidateFksAsync(dto.AdultTypeId, dto.SchoolStudentId, ct);
        if (fkErrors.Count > 0)
        {
            return BadRequest(new { message = "Invalid foreign keys", errors = fkErrors });
        }

        var entity = MapFromCreateDto(dto);
        var created = await _service.CreateAsync(entity, ct);
        var readDto = MapToReadDto(created);
        return CreatedAtAction(nameof(GetById), new { id = readDto.Id }, readDto);
    }

    // PUT: api/Adult/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] AdultUpdateDto dto, CancellationToken ct)
    {
        var fkErrors = await ValidateFksAsync(dto.AdultTypeId, dto.SchoolStudentId, ct);
        if (fkErrors.Count > 0)
        {
            return BadRequest(new { message = "Invalid foreign keys", errors = fkErrors });
        }

        var updated = await _service.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated ? NoContent() : NotFound();
    }

    // DELETE: api/Adult/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var deleted = await _service.DeleteAsync(id, soft, ct);
        return deleted ? NoContent() : NotFound();
    }

    private async Task<List<string>> ValidateFksAsync(int adultTypeId, int schoolStudentId, CancellationToken ct)
    {
        var errors = new List<string>();
        if (!await _db.AdultTypes.AnyAsync(a => a.Id == adultTypeId, ct))
        {
            errors.Add($"AdultTypeId ({adultTypeId}) not found");
        }
        if (!await _db.SchoolStudents.AnyAsync(s => s.Id == schoolStudentId, ct))
        {
            errors.Add($"SchoolStudentId ({schoolStudentId}) not found");
        }
        return errors;
    }

    private static AdultReadDto MapToReadDto(Adult adult) => new()
    {
        Id = adult.Id,
        WorkPhoneNumber = adult.WorkPhoneNumber,
        WorkEmail = adult.WorkEmail,
        AdultTypeId = adult.IdAdultType,
        SchoolStudentId = adult.IdSchoolStudent
    };

    private static Adult MapFromCreateDto(AdultCreateDto dto) => new()
    {
        WorkPhoneNumber = dto.WorkPhoneNumber,
        WorkEmail = dto.WorkEmail,
        IdAdultType = dto.AdultTypeId,
        IdSchoolStudent = dto.SchoolStudentId,
        Active = true,
        AdultType = null!,
        SchoolStudent = null!
    };

    private static void ApplyUpdateDto(Adult target, AdultUpdateDto dto)
    {
        target.WorkPhoneNumber = dto.WorkPhoneNumber;
        target.WorkEmail = dto.WorkEmail;
        target.IdAdultType = dto.AdultTypeId;
        target.IdSchoolStudent = dto.SchoolStudentId;
    }
}

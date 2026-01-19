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
public class StudentInterestController : ControllerBase
{
    private readonly IBaseCrud<StudentInterest> _service;
    private readonly OracleDbContext _db;

    public StudentInterestController(IBaseCrud<StudentInterest> service, OracleDbContext db)
    {
        _service = service;
        _db = db;
    }

    // GET: api/StudentInterest
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentInterestReadDto>>> GetAll(CancellationToken ct)
    {
        var items = await _service.GetAllAsync(true, ct);
        var dtos = items.Select(MapToReadDto).ToList();
        return Ok(dtos);
    }

    // GET: api/StudentInterest/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudentInterestReadDto>> GetById(int id, CancellationToken ct)
    {
        var item = await _service.GetByIdAsync(id, true, ct);
        return item is null ? NotFound() : Ok(MapToReadDto(item));
    }

    // POST: api/StudentInterest
    [HttpPost]
    public async Task<ActionResult<StudentInterestReadDto>> Create([FromBody] StudentInterestCreateDto dto, CancellationToken ct)
    {
        var fkErrors = await ValidateFksAsync(dto.SchoolStudentId, dto.InterestActivitieId, ct);
        if (fkErrors.Count > 0)
        {
            return BadRequest(new { message = "Invalid foreign keys", errors = fkErrors });
        }

        var entity = MapFromCreateDto(dto);
        var created = await _service.CreateAsync(entity, ct);
        var readDto = MapToReadDto(created);
        return CreatedAtAction(nameof(GetById), new { id = readDto.Id }, readDto);
    }

    // PUT: api/StudentInterest/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] StudentInterestUpdateDto dto, CancellationToken ct)
    {
        var fkErrors = await ValidateFksAsync(dto.SchoolStudentId, dto.InterestActivitieId, ct);
        if (fkErrors.Count > 0)
        {
            return BadRequest(new { message = "Invalid foreign keys", errors = fkErrors });
        }

        var updated = await _service.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated ? NoContent() : NotFound();
    }

    // DELETE: api/StudentInterest/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var deleted = await _service.DeleteAsync(id, soft, ct);
        return deleted ? NoContent() : NotFound();
    }

    private async Task<List<string>> ValidateFksAsync(int schoolStudentId, int interestActivitieId, CancellationToken ct)
    {
        var errors = new List<string>();
        if (!await _db.SchoolStudents.AnyAsync(s => s.Id == schoolStudentId, ct))
        {
            errors.Add($"SchoolStudentId ({schoolStudentId}) not found");
        }
        if (!await _db.InterestActivities.AnyAsync(i => i.Id == interestActivitieId, ct))
        {
            errors.Add($"InterestActivitieId ({interestActivitieId}) not found");
        }
        return errors;
    }

    private static StudentInterestReadDto MapToReadDto(StudentInterest interest) => new()
    {
        Id = interest.Id,
        MomentSelected = (WebApiNibu.Data.Dto.Person.MomentSelected?)interest.MomentSelected,
        Moment = interest.Moment,
        SchoolStudentId = interest.IdSchoolStudent,
        InterestActivitieId = interest.IdInterestActivity
    };

    private static StudentInterest MapFromCreateDto(StudentInterestCreateDto dto) => new()
    {
        MomentSelected = dto.MomentSelected.HasValue
            ? (WebApiNibu.Data.Enum.MomentSelected)dto.MomentSelected.Value
            : WebApiNibu.Data.Enum.MomentSelected.App,
        Moment = dto.Moment,
        IdSchoolStudent = dto.SchoolStudentId,
        IdInterestActivity = dto.InterestActivitieId,
        Active = true,
        SchoolStudent = null!,
        InterestActivity = null!
    };

    private static void ApplyUpdateDto(StudentInterest target, StudentInterestUpdateDto dto)
    {
        target.MomentSelected = dto.MomentSelected.HasValue
            ? (WebApiNibu.Data.Enum.MomentSelected)dto.MomentSelected.Value
            : target.MomentSelected;
        target.Moment = dto.Moment;
        target.IdSchoolStudent = dto.SchoolStudentId;
        target.IdInterestActivity = dto.InterestActivitieId;
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto;
using WebApiNibu.Data.Entity.School;

namespace WebApiNibu.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController(IBaseCrud<Contact> service, OracleDbContext db) : ControllerBase
{
    // GET: api/Contact
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContactReadDto>>> GetAll(CancellationToken ct)
    {
        var items = await db.Contacts.AsNoTracking().ToListAsync(ct);
        return Ok(items.Select(MapToReadDto));
    }

    // GET: api/Contact/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ContactReadDto>> GetById(int id, CancellationToken ct)
    {
        var item = await service.GetByIdAsync(id, true, ct);
        return item is null ? NotFound() : Ok(MapToReadDto(item));
    }

    // POST: api/Contact?schoolId=123
    [HttpPost]
    public async Task<ActionResult<ContactReadDto>> Create([FromQuery] int schoolId, [FromBody] ContactCreateDto dto, CancellationToken ct)
    {
        if (!await db.Schools.AnyAsync(s => s.Id == schoolId, ct))
        {
            return BadRequest(new { message = $"School (Id={schoolId}) not found" });
        }

        // Attach by PK without loading all columns
        var schoolProxy = new SchoolTable { Id = schoolId, Active = true };
        db.Attach(schoolProxy);

        var entity = new Contact
        {
            PersonName = dto.PersonName,
            PersonRole = dto.PersonRole,
            PersonPhoneNumber = dto.PersonPhoneNumber,
            PersonEmail = dto.PersonEmail,
            SchoolTable = schoolProxy,
            Active = true
        };

        var created = await service.CreateAsync(entity, ct);
        var readDto = MapToReadDto(created);
        return CreatedAtAction(nameof(GetById), new { id = readDto.Id }, readDto);
    }

    // PUT: api/Contact/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ContactUpdateDto dto, CancellationToken ct)
    {
        var updated = await service.UpdateAsync(id, entity =>
        {
            entity.PersonName = dto.PersonName;
            entity.PersonRole = dto.PersonRole;
            entity.PersonPhoneNumber = dto.PersonPhoneNumber;
            entity.PersonEmail = dto.PersonEmail;
        }, ct);

        return updated ? NoContent() : NotFound();
    }

    // DELETE: api/Contact/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var deleted = await service.DeleteAsync(id, soft, ct);
        return deleted ? NoContent() : NotFound();
    }

    private static ContactReadDto MapToReadDto(Contact c) => new()
    {
        Id = c.Id,
        PersonName = c.PersonName,
        PersonRole = c.PersonRole,
        PersonPhoneNumber = c.PersonPhoneNumber,
        PersonEmail = c.PersonEmail
    };
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto;
using WebApiNibu.Data.Entity.School;

namespace WebApiNibu.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchoolTableController(IBaseCrud<SchoolTable> service, OracleDbContext db) : ControllerBase
{
    // GET: api/SchoolTable
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SchoolTableReadDto>>> GetAll(CancellationToken ct)
    {
        var items = await db.Schools
            .AsNoTracking()
            .Include(s => s.Contacts)
            .ToListAsync(ct);

        return Ok(items.Select(MapToReadDto));
    }

    // GET: api/SchoolTable/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<SchoolTableReadDto>> GetById(int id, CancellationToken ct)
    {
        var item = await db.Schools
            .AsNoTracking()
            .Include(s => s.Contacts)
            .FirstOrDefaultAsync(s => s.Id == id, ct);

        return item is null ? NotFound() : Ok(MapToReadDto(item));
    }

    // POST: api/SchoolTable
    [HttpPost]
    public async Task<ActionResult<SchoolTableReadDto>> Create([FromBody] SchoolTableCreateDto dto, CancellationToken ct)
    {
        var entity = MapFromCreateDto(dto);

        // Create school
        var created = await service.CreateAsync(entity, ct);

        // Create contacts linked to the created school (if provided)
        if (dto.Contacts.Count > 0)
        {
            foreach (var c in dto.Contacts)
            {
                var contact = new Contact
                {
                    PersonName = c.PersonName,
                    PersonRole = c.PersonRole,
                    PersonPhoneNumber = c.PersonPhoneNumber,
                    PersonEmail = c.PersonEmail,
                    SchoolTable = created,
                    Active = true
                };
                db.Contacts.Add(contact);
            }

            await db.SaveChangesAsync(ct);
        }

        var read = await db.Schools
            .AsNoTracking()
            .Include(s => s.Contacts)
            .FirstAsync(s => s.Id == created.Id, ct);

        var readDto = MapToReadDto(read);
        return CreatedAtAction(nameof(GetById), new { id = readDto.Id }, readDto);
    }

    // PUT: api/SchoolTable/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] SchoolTableUpdateDto dto, CancellationToken ct)
    {
        var updated = await service.UpdateAsync(id, entity => ApplyUpdateDto(entity, dto), ct);
        return updated ? NoContent() : NotFound();
    }

    // DELETE: api/SchoolTable/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var deleted = await service.DeleteAsync(id, soft, ct);
        return deleted ? NoContent() : NotFound();
    }

    private static SchoolTableReadDto MapToReadDto(SchoolTable s) => new()
    {
        Id = s.Id,
        Name = s.Name,
        Tier = s.Tier,
        Address = s.Address,
        SportLogo = s.SportLogo,
        NormalLogo = s.NormalLogo,
        Rue = s.Rue,
        Delegada = s.Delegada,
        Tipo = s.Tipo,
        Ciudad = s.Ciudad,
        IdDepartamento = s.IdDepartamento,
        Ka = s.Ka,
        IdDelegada = s.IdDelegada,
        IdColegio = s.IdColegio,
        KaRectorada = s.KaRectorada,
        Segemento = s.Segemento,
        Contacts = s.Contacts?.Select(MapToReadDto).ToList() ?? new List<ContactReadDto>()
    };

    private static ContactReadDto MapToReadDto(Contact c) => new()
    {
        Id = c.Id,
        PersonName = c.PersonName,
        PersonRole = c.PersonRole,
        PersonPhoneNumber = c.PersonPhoneNumber,
        PersonEmail = c.PersonEmail
    };

    private static SchoolTable MapFromCreateDto(SchoolTableCreateDto dto) => new()
    {
        Name = dto.Name,
        Tier = dto.Tier,
        Address = dto.Address,
        SportLogo = dto.SportLogo,
        NormalLogo = dto.NormalLogo,
        Rue = dto.Rue,
        Delegada = dto.Delegada,
        Tipo = dto.Tipo,
        Ciudad = dto.Ciudad,
        IdDepartamento = dto.IdDepartamento,
        Ka = dto.Ka,
        IdDelegada = dto.IdDelegada,
        IdColegio = dto.IdColegio,
        KaRectorada = dto.KaRectorada,
        Segemento = dto.Segemento,
        Active = true
    };

    private static void ApplyUpdateDto(SchoolTable target, SchoolTableUpdateDto dto)
    {
        target.Name = dto.Name;
        target.Tier = dto.Tier;
        target.Address = dto.Address;
        target.SportLogo = dto.SportLogo;
        target.NormalLogo = dto.NormalLogo;
        target.Rue = dto.Rue;
        target.Delegada = dto.Delegada;
        target.Tipo = dto.Tipo;
        target.Ciudad = dto.Ciudad;
        target.IdDepartamento = dto.IdDepartamento;
        target.Ka = dto.Ka;
        target.IdDelegada = dto.IdDelegada;
        target.IdColegio = dto.IdColegio;
        target.KaRectorada = dto.KaRectorada;
        target.Segemento = dto.Segemento;
    }
}


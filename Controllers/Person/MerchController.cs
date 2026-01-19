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
public class MerchController : ControllerBase
{
    private readonly IBaseCrud<Merch> _service;
    private readonly OracleDbContext _db;

    public MerchController(IBaseCrud<Merch> service, OracleDbContext db)
    {
        _service = service;
        _db = db;
    }

    // GET: api/Merch
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MerchReadDto>>> GetAll(CancellationToken ct)
    {
        var items = await _service.GetAllAsync(true, ct);
        var dtos = items.Select(MapToReadDto).ToList();
        return Ok(dtos);
    }

    // GET: api/Merch/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<MerchReadDto>> GetById(int id, CancellationToken ct)
    {
        var item = await _service.GetByIdAsync(id, true, ct);
        return item is null ? NotFound() : Ok(MapToReadDto(item));
    }

    // POST: api/Merch
    [HttpPost]
    public async Task<ActionResult<MerchReadDto>> Create([FromBody] MerchCreateDto dto, CancellationToken ct)
    {
        var fkErrors = await ValidateFksAsync(dto.MerchTypeId, ct);
        if (fkErrors.Count > 0)
        {
            return BadRequest(new { message = "Invalid foreign keys", errors = fkErrors });
        }

        var entity = MapFromCreateDto(dto);
        var created = await _service.CreateAsync(entity, ct);
        var readDto = MapToReadDto(created);
        return CreatedAtAction(nameof(GetById), new { id = readDto.Id }, readDto);
    }

    // PUT: api/Merch/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] MerchUpdateDto dto, CancellationToken ct)
    {
        var fkErrors = await ValidateFksAsync(dto.MerchTypeId, ct);
        if (fkErrors.Count > 0)
        {
            return BadRequest(new { message = "Invalid foreign keys", errors = fkErrors });
        }

        var updated = await _service.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated ? NoContent() : NotFound();
    }

    // DELETE: api/Merch/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var deleted = await _service.DeleteAsync(id, soft, ct);
        return deleted ? NoContent() : NotFound();
    }

    private async Task<List<string>> ValidateFksAsync(int merchTypeId, CancellationToken ct)
    {
        var errors = new List<string>();
        if (!await _db.MerchTypes.AnyAsync(mt => mt.Id == merchTypeId, ct))
        {
            errors.Add($"MerchTypeId ({merchTypeId}) not found");
        }
        return errors;
    }

    private static MerchReadDto MapToReadDto(Merch merch) => new()
    {
        Id = merch.Id,
        Name = merch.Name,
        Description = merch.Description,
        Icon = merch.Icon ?? string.Empty,
        Image = merch.Image ?? string.Empty,
        Rarity = (WebApiNibu.Data.Dto.Person.Rarity)merch.Rarity,
        MaxQuantity = merch.MaxQuantity,
        MerchTypeId = merch.IdMerchType
    };

    private static Merch MapFromCreateDto(MerchCreateDto dto) => new()
    {
        Name = dto.Name,
        Description = dto.Description ?? string.Empty,
        Icon = dto.Icon,
        Image = dto.Image,
        Rarity = (WebApiNibu.Data.Enum.Rarity)dto.Rarity,
        MaxQuantity = dto.MaxQuantity,
        IdMerchType = dto.MerchTypeId,
        Active = true,
        MerchType = null!
    };

    private static void ApplyUpdateDto(Merch target, MerchUpdateDto dto)
    {
        target.Name = dto.Name;
        target.Description = dto.Description ?? string.Empty;
        target.Icon = dto.Icon;
        target.Image = dto.Image;
        target.Rarity = (WebApiNibu.Data.Enum.Rarity)dto.Rarity;
        target.MaxQuantity = dto.MaxQuantity;
        target.IdMerchType = dto.MerchTypeId;
    }
}

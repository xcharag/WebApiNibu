using Microsoft.AspNetCore.Mvc;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Entity.Person;

namespace WebApiNibu.Controllers.Person;

[ApiController]
[Route("api/[controller]")]
public class DocumentTypeController(IBaseCrud<DocumentType> service) : ControllerBase
{
    // GET: api/DocumentType
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DocumentTypeReadDto>>> GetAll(CancellationToken ct)
    {
        var items = await service.GetAllAsync(true, ct);
        var dtos = items.Select(MapToReadDto).ToList();
        return Ok(dtos);
    }

    // GET: api/DocumentType/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<DocumentTypeReadDto>> GetById(int id, CancellationToken ct)
    {
        var item = await service.GetByIdAsync(id, true, ct);
        return item is null ? NotFound() : Ok(MapToReadDto(item));
    }

    // POST: api/DocumentType
    [HttpPost]
    public async Task<ActionResult<DocumentTypeReadDto>> Create([FromBody] DocumentTypeCreateDto dto, CancellationToken ct)
    {
        var entity = MapFromCreateDto(dto);
        var created = await service.CreateAsync(entity, ct);
        var readDto = MapToReadDto(created);
        return CreatedAtAction(nameof(GetById), new { id = readDto.Id }, readDto);
    }

    // PUT: api/DocumentType/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] DocumentTypeUpdateDto dto, CancellationToken ct)
    {
        var updated = await service.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated ? NoContent() : NotFound();
    }

    // DELETE: api/DocumentType/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct, [FromQuery] bool soft = true)
    {
        var deleted = await service.DeleteAsync(id, soft, ct);
        return deleted ? NoContent() : NotFound();
    }

    private static DocumentTypeReadDto MapToReadDto(DocumentType documentType) => new()
    {
        Id = documentType.Id,
        Name = documentType.Name
    };

    private static DocumentType MapFromCreateDto(DocumentTypeCreateDto dto) => new()
    {
        Name = dto.Name,
        Active = true
    };

    private static void ApplyUpdateDto(DocumentType target, DocumentTypeUpdateDto dto)
    {
        target.Name = dto.Name;
    }
}

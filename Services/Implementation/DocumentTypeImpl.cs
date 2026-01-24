using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Data.Entity.Person;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract;

namespace WebApiNibu.Services.Implementation;

public class DocumentTypeImpl(IBaseCrud<DocumentType> baseCrud, OracleDbContext db) : IDocumentType
{
    // ─────────────────────────────── Queries ───────────────────────────────

    public async Task<Result<IEnumerable<DocumentTypeReadDto>>> GetAllAsync(CancellationToken ct)
    {
        var items = await baseCrud.GetAllAsync(true, ct);
        var dtos = items.Select(MapToReadDto);
        return Result<IEnumerable<DocumentTypeReadDto>>.Success(dtos);
    }

    public async Task<Result<DocumentTypeReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await baseCrud.GetByIdAsync(id, true, ct);
        return item is null
            ? Result<DocumentTypeReadDto>.Failure($"DocumentType with id {id} not found")
            : Result<DocumentTypeReadDto>.Success(MapToReadDto(item));
    }

    public async Task<Result<IEnumerable<DocumentTypeReadDto>>> GetFilteredAsync(DocumentTypeFilter filter, CancellationToken ct)
    {
        var query = db.DocumentTypes.AsQueryable();
        query = ApplyFilters(query, filter);
        var items = await query.ToListAsync(ct);
        var dtos = items.Select(MapToReadDto);
        return Result<IEnumerable<DocumentTypeReadDto>>.Success(dtos);
    }

    // ─────────────────────────────── Commands ───────────────────────────────

    public async Task<Result<DocumentTypeReadDto>> CreateAsync(DocumentTypeCreateDto dto, CancellationToken ct)
    {
        var entity = MapFromCreateDto(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<DocumentTypeReadDto>.Success(MapToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, DocumentTypeUpdateDto dto, CancellationToken ct)
    {
        var updated = await baseCrud.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"DocumentType with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"DocumentType with id {id} not found");
    }

    // ─────────────────────────────── Filters ───────────────────────────────

    private static IQueryable<DocumentType> ApplyFilters(IQueryable<DocumentType> query, DocumentTypeFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }

    // ─────────────────────────────── Mapping ───────────────────────────────

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

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

    public async Task<Result<PagedResult<DocumentTypeReadDto>>> GetAllAsync(DocumentTypeFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.DocumentTypes.AsQueryable();
        query = ApplyFilters(query, filter);
        
        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<DocumentTypeReadDto>>.Success(new PagedResult<DocumentTypeReadDto>
        {
            Items = items.Select(MapToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<DocumentTypeReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await baseCrud.GetByIdAsync(id, true, ct);
        return item is null
            ? Result<DocumentTypeReadDto>.Failure($"DocumentType with id {id} not found")
            : Result<DocumentTypeReadDto>.Success(MapToReadDto(item));
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

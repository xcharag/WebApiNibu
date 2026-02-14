using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.DocumentType;

public class DocumentTypeQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<DocumentTypeReadDto>>> GetAllAsync(
        DocumentTypeFilter filter, 
        PaginationParams pagination, 
        CancellationToken ct)
    {
        var query = db.DocumentTypes.AsQueryable();
        query = DocumentTypeFilterHandler.Apply(query, filter);
        
        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<DocumentTypeReadDto>>.Success(new PagedResult<DocumentTypeReadDto>
        {
            Items = items.Select(DocumentTypeMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<DocumentTypeReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.DocumentTypes.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<DocumentTypeReadDto>.Failure($"DocumentType with id {id} not found")
            : Result<DocumentTypeReadDto>.Success(DocumentTypeMapper.ToReadDto(item));
    }
}

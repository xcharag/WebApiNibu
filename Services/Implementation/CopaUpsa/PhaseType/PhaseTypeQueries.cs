using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.CopaUpsa.PhaseType;

public class PhaseTypeQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<PhaseTypeReadDto>>> GetAllAsync(
        PhaseTypeFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.PhaseTypes.AsQueryable();
        query = PhaseTypeFilterHandler.Apply(query, filter);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<PhaseTypeReadDto>>.Success(new PagedResult<PhaseTypeReadDto>
        {
            Items = items.Select(PhaseTypeMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<PhaseTypeReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.PhaseTypes.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<PhaseTypeReadDto>.Failure($"PhaseType with id {id} not found")
            : Result<PhaseTypeReadDto>.Success(PhaseTypeMapper.ToReadDto(item));
    }
}


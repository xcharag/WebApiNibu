using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Position;

public class PositionQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<PositionReadDto>>> GetAllAsync(
        PositionFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.Positions.AsQueryable();
        query = PositionFilterHandler.Apply(query, filter);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<PositionReadDto>>.Success(new PagedResult<PositionReadDto>
        {
            Items = items.Select(PositionMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<PositionReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.Positions.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<PositionReadDto>.Failure($"Position with id {id} not found")
            : Result<PositionReadDto>.Success(PositionMapper.ToReadDto(item));
    }
}


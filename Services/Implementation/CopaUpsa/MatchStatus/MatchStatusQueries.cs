using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.CopaUpsa.MatchStatus;

public class MatchStatusQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<MatchStatusReadDto>>> GetAllAsync(
        MatchStatusFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.MatchStatuses.AsQueryable();
        query = MatchStatusFilterHandler.Apply(query, filter);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<MatchStatusReadDto>>.Success(new PagedResult<MatchStatusReadDto>
        {
            Items = items.Select(MatchStatusMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<MatchStatusReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.MatchStatuses.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<MatchStatusReadDto>.Failure($"MatchStatus with id {id} not found")
            : Result<MatchStatusReadDto>.Success(MatchStatusMapper.ToReadDto(item));
    }
}


using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Match;

public class MatchQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<MatchReadDto>>> GetAllAsync(
        MatchFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.Matches.AsQueryable();
        query = MatchFilterHandler.Apply(query, filter);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<MatchReadDto>>.Success(new PagedResult<MatchReadDto>
        {
            Items = items.Select(MatchMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<MatchReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.Matches.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<MatchReadDto>.Failure($"Match with id {id} not found")
            : Result<MatchReadDto>.Success(MatchMapper.ToReadDto(item));
    }
}


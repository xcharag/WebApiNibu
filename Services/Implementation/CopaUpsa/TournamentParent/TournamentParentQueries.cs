using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.CopaUpsa.TournamentParent;

public class TournamentParentQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<TournamentParentReadDto>>> GetAllAsync(
        TournamentParentFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.TournamentParents.AsQueryable();
        query = TournamentParentFilterHandler.Apply(query, filter);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<TournamentParentReadDto>>.Success(new PagedResult<TournamentParentReadDto>
        {
            Items = items.Select(TournamentParentMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<TournamentParentReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.TournamentParents.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<TournamentParentReadDto>.Failure($"TournamentParent with id {id} not found")
            : Result<TournamentParentReadDto>.Success(TournamentParentMapper.ToReadDto(item));
    }
}


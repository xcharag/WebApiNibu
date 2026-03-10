using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Roster;

public class RosterQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<RosterReadDto>>> GetAllAsync(
        RosterFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.Rosters
            .Include(x => x.TournamentRoster)
            .Include(x => x.Match)
            .Include(x => x.Position)
            .AsQueryable();
        query = RosterFilterHandler.Apply(query, filter);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<RosterReadDto>>.Success(new PagedResult<RosterReadDto>
        {
            Items = items.Select(RosterMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<RosterReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.Rosters
            .Include(x => x.TournamentRoster)
            .Include(x => x.Match)
            .Include(x => x.Position)
            .FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<RosterReadDto>.Failure($"Roster with id {id} not found")
            : Result<RosterReadDto>.Success(RosterMapper.ToReadDto(item));
    }
}

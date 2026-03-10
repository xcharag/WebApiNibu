using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.CopaUpsa.TournamentRoster;

public class TournamentRosterQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<TournamentRosterReadDto>>> GetAllAsync(
        TournamentRosterFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.TournamentRosters
            .Include(x => x.Tournament)
            .Include(x => x.SchoolTable)
            .AsQueryable();
        query = TournamentRosterFilterHandler.Apply(query, filter);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<TournamentRosterReadDto>>.Success(new PagedResult<TournamentRosterReadDto>
        {
            Items = items.Select(TournamentRosterMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<TournamentRosterReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.TournamentRosters
            .Include(x => x.Tournament)
            .Include(x => x.SchoolTable)
            .FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<TournamentRosterReadDto>.Failure($"TournamentRoster with id {id} not found")
            : Result<TournamentRosterReadDto>.Success(TournamentRosterMapper.ToReadDto(item));
    }
}


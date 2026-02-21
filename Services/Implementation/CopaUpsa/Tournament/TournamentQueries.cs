using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Tournament;

public class TournamentQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<TournamentReadDto>>> GetAllAsync(
        TournamentFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.Tournaments.AsQueryable();
        query = TournamentFilterHandler.Apply(query, filter);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<TournamentReadDto>>.Success(new PagedResult<TournamentReadDto>
        {
            Items = items.Select(TournamentMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<TournamentReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.Tournaments.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<TournamentReadDto>.Failure($"Tournament with id {id} not found")
            : Result<TournamentReadDto>.Success(TournamentMapper.ToReadDto(item));
    }
}


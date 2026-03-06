using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Feed.Polls;
using WebApiNibu.Data.Dto.Feed.Polls.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Feed.Polls.Poll;

public class PollQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<PollReadDto>>> GetAllAsync(
        PollFilter filter,
        PaginationParams pagination,
        CancellationToken ct)
    {
        // If caller filtered by MatchId, resolve the related TournamentId to filter polls
        if (filter.MatchId.HasValue)
        {
            var tournamentId = await db.Matches
                .Where(m => m.Id == filter.MatchId.Value)
                .Select(m => m.Participation.TournamentId)
                .FirstOrDefaultAsync(ct);

            // set TournamentId to the resolved value (0 if not found) so PollFilterHandler can apply it
            filter.TournamentId = tournamentId;
        }

        var query = db.Polls
            .Include(x => x.Options)
            .Include(x => x.Tournament)
            .AsQueryable();
        query = PollFilterHandler.Apply(query, filter);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<PollReadDto>>.Success(new PagedResult<PollReadDto>
        {
            Items = items.Select(PollMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<PollReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.Polls
            .Include(x => x.Options)
            .Include(x => x.Tournament)
            .FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<PollReadDto>.Failure($"Poll with id {id} not found")
            : Result<PollReadDto>.Success(PollMapper.ToReadDto(item));
    }
}

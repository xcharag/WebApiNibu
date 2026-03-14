using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Feed.Polls;
using WebApiNibu.Data.Dto.Feed.Polls.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Feed.Polls.Poll;

public class PollQueries(CoreDbContext db)
{
    private sealed class UserRankingState
    {
        public required int UserId { get; init; }
        public required string Name { get; init; }
        public int Correct { get; set; }
        public int Participated { get; set; }
        public int Total { get; set; }
        public int CurrentStreak { get; set; }
        public int MaxStreak { get; set; }
        public double Percentage { get; set; }
        public Dictionary<int, bool> PollResult { get; } = new();
    }

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
                .Select(m => m.ParticipationA.TournamentId)
                .FirstOrDefaultAsync(ct);

            // set TournamentId to the resolved value (0 if not found) so PollFilterHandler can apply it
            filter.TournamentId = tournamentId;
        }

        var query = db.Polls
            .Include(x => x.Options)
                .ThenInclude(o => o.SelectedOptions)
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
                .ThenInclude(o => o.SelectedOptions)
            .Include(x => x.Tournament)
            .FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<PollReadDto>.Failure($"Poll with id {id} not found")
            : Result<PollReadDto>.Success(PollMapper.ToReadDto(item));
    }

    public async Task<Result<PredictionRankingDto>> GetPredictionRankingAsync(
        int? tournamentId,
        int page,
        int pageSize,
        int? currentUserId,
        CancellationToken ct)
    {
        var safePage = page <= 0 ? 1 : page;
        var safePageSize = pageSize <= 0 ? 10 : Math.Min(pageSize, 200);

        var pollsQuery = db.Polls
            .AsNoTracking()
            .Where(x => x.Active)
            .AsQueryable();

        if (tournamentId.HasValue)
            pollsQuery = pollsQuery.Where(x => x.TournamentId == tournamentId.Value);

        var polls = await pollsQuery
            .Select(p => new
            {
                p.Id,
                p.ExpirationDate,
                p.CreatedAt,
                HasCorrectOption = p.Options.Any(o => o.Active && o.Correct)
            })
            .ToListAsync(ct);

        var totalPredictionsTournament = polls.Count;
        var evaluatedPollIds = polls
            .Where(p => p.HasCorrectOption)
            .OrderBy(p => p.ExpirationDate ?? p.CreatedAt ?? DateTime.MinValue)
            .ThenBy(p => p.Id)
            .Select(p => p.Id)
            .ToList();
        var totalPredictionsEvaluated = evaluatedPollIds.Count;

        var selectedQuery = db.SelectedOptions
            .AsNoTracking()
            .Where(x => x.Active && x.Option.Active && x.Option.Poll.Active)
            .AsQueryable();

        if (tournamentId.HasValue)
            selectedQuery = selectedQuery.Where(x => x.Option.Poll.TournamentId == tournamentId.Value);

        var selectedRows = await selectedQuery
            .Select(x => new
            {
                PollId = x.Option.PollId,
                UserId = x.UserId,
                IsCorrect = x.Option.Correct,
                UserName = string.Join(" ",
                    new[]
                    {
                        x.User.PersonTable.FirstName,
                        x.User.PersonTable.MiddleName,
                        x.User.PersonTable.PaternalSurname,
                        x.User.PersonTable.MaternalSurname
                    }.Where(s => !string.IsNullOrWhiteSpace(s))),
                FallbackName = x.User.Name
            })
            .ToListAsync(ct);

        var users = new Dictionary<int, UserRankingState>();
        var byUserPoll = selectedRows
            .GroupBy(x => new { x.UserId, x.PollId })
            .Select(g => new
            {
                g.Key.UserId,
                g.Key.PollId,
                IsCorrect = g.Any(x => x.IsCorrect),
                Name = g.Select(x => string.IsNullOrWhiteSpace(x.UserName) ? x.FallbackName : x.UserName)
                    .FirstOrDefault() ?? $"Usuario {g.Key.UserId}"
            });

        foreach (var row in byUserPoll)
        {
            if (!users.TryGetValue(row.UserId, out var state))
            {
                state = new UserRankingState
                {
                    UserId = row.UserId,
                    Name = row.Name
                };
                users[row.UserId] = state;
            }
            state.PollResult[row.PollId] = row.IsCorrect;
            if (row.IsCorrect) state.Correct += 1;
        }

        foreach (var state in users.Values)
        {
            state.Total = totalPredictionsTournament;
            state.Participated = state.PollResult.Keys.Count(k => evaluatedPollIds.Contains(k));

            foreach (var pollId in evaluatedPollIds)
            {
                if (!state.PollResult.TryGetValue(pollId, out var ok))
                    continue;

                if (ok)
                {
                    state.CurrentStreak += 1;
                    if (state.CurrentStreak > state.MaxStreak)
                        state.MaxStreak = state.CurrentStreak;
                }
                else
                {
                    state.CurrentStreak = 0;
                }
            }

            state.Percentage = totalPredictionsTournament > 0
                ? Math.Round(state.Correct * 100.0 / totalPredictionsTournament, 1)
                : 0;
        }

        var ordered = users.Values
            .OrderByDescending(x => x.Percentage)
            .ThenByDescending(x => x.Correct)
            .ThenByDescending(x => x.Participated)
            .ThenBy(x => x.Name)
            .ToList();

        var totalUsers = ordered.Count;
        var start = (safePage - 1) * safePageSize;
        var pageItems = start < totalUsers
            ? ordered.Skip(start).Take(safePageSize).ToList()
            : [];

        var ranking = pageItems
            .Select((u, idx) => new PredictionRankingItemDto
            {
                Pos = start + idx + 1,
                UserId = u.UserId,
                Name = u.Name,
                Correct = u.Correct,
                Total = u.Total,
                Participated = u.Participated,
                Percentage = u.Percentage,
                Streak = u.CurrentStreak,
                MaxStreak = u.MaxStreak
            })
            .ToList();

        PredictionRankingItemDto? currentUser = null;
        if (currentUserId.HasValue)
        {
            var idx = ordered.FindIndex(x => x.UserId == currentUserId.Value);
            if (idx >= 0)
            {
                var u = ordered[idx];
                currentUser = new PredictionRankingItemDto
                {
                    Pos = idx + 1,
                    UserId = u.UserId,
                    Name = u.Name,
                    Correct = u.Correct,
                    Total = u.Total,
                    Participated = u.Participated,
                    Percentage = u.Percentage,
                    Streak = u.CurrentStreak,
                    MaxStreak = u.MaxStreak
                };
            }
        }

        return Result<PredictionRankingDto>.Success(new PredictionRankingDto
        {
            Ranking = ranking,
            Page = safePage,
            PageSize = safePageSize,
            TotalUsers = totalUsers,
            CurrentUser = currentUser,
            TotalPredictionsTournament = totalPredictionsTournament,
            TotalPredictionsEvaluated = totalPredictionsEvaluated
        });
    }
}

using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.CopaUpsa.StatisticEvent;

public class StatisticEventQueries(CoreDbContext db)
{
    private static string BuildFullName(params string?[] parts)
        => string.Join(" ", parts.Where(s => !string.IsNullOrWhiteSpace(s)));

    public async Task<Result<PagedResult<StatisticEventReadDto>>> GetAllAsync(
        StatisticEventFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.StatisticEvents
            .Include(x => x.Statistic)
            .Include(x => x.Roster).ThenInclude(r => r.TournamentRoster)
            .AsQueryable();
        query = StatisticEventFilterHandler.Apply(query, filter);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<StatisticEventReadDto>>.Success(new PagedResult<StatisticEventReadDto>
        {
            Items = items.Select(StatisticEventMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<StatisticEventReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.StatisticEvents
            .Include(x => x.Statistic)
            .Include(x => x.Roster).ThenInclude(r => r.TournamentRoster)
            .FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<StatisticEventReadDto>.Failure($"StatisticEvent with id {id} not found")
            : Result<StatisticEventReadDto>.Success(StatisticEventMapper.ToReadDto(item));
    }

    public async Task<Result<List<StatisticEventTimelineDto>>> GetTimelineAsync(
        int? matchId,
        int? tournamentId,
        int? statisticId,
        CancellationToken ct)
    {
        var query = db.StatisticEvents
            .AsNoTracking()
            .Where(x => x.Active)
            .Include(x => x.Statistic)
            .Include(x => x.Roster)
                .ThenInclude(r => r.TournamentRoster)
                    .ThenInclude(tr => tr.SchoolTable)
            .Include(x => x.Roster)
                .ThenInclude(r => r.Match)
                    .ThenInclude(m => m.ParticipationA)
            .Include(x => x.Roster)
                .ThenInclude(r => r.Match)
                    .ThenInclude(m => m.ParticipationB)
            .AsQueryable();

        if (matchId.HasValue)
            query = query.Where(x => x.Roster.MatchId == matchId.Value);

        if (statisticId.HasValue)
            query = query.Where(x => x.StatisticId == statisticId.Value);

        if (tournamentId.HasValue)
        {
            var tid = tournamentId.Value;
            query = query.Where(x =>
                x.Roster.Match.ParticipationA.TournamentId == tid ||
                x.Roster.Match.ParticipationB.TournamentId == tid);
        }

        var rows = await query
            .OrderBy(x => x.Moment)
            .ThenBy(x => x.Id)
            .Select(x => new
            {
                Id = x.Id,
                Moment = x.Moment,
                StatisticId = x.StatisticId,
                StatisticName = x.Statistic.Name,
                RosterId = x.RosterId,
                TournamentRosterId = x.Roster.TournamentRosterId,
                MatchId = x.Roster.MatchId,
                TournamentId = x.Roster.Match.ParticipationA.TournamentId,
                SchoolId = x.Roster.TournamentRoster.SchoolId,
                SchoolName = x.Roster.TournamentRoster.SchoolTable.Name,
                FirstName = x.Roster.TournamentRoster.FirstName,
                MiddleName = x.Roster.TournamentRoster.MiddleName,
                LastName = x.Roster.TournamentRoster.LastName,
                MaternalName = x.Roster.TournamentRoster.MaternalName
            })
            .ToListAsync(ct);

        var items = rows.Select(x => new StatisticEventTimelineDto
        {
            Id = x.Id,
            Moment = x.Moment,
            StatisticId = x.StatisticId,
            StatisticName = x.StatisticName,
            RosterId = x.RosterId,
            TournamentRosterId = x.TournamentRosterId,
            MatchId = x.MatchId,
            TournamentId = x.TournamentId,
            SchoolId = x.SchoolId,
            SchoolName = x.SchoolName,
            StudentName = BuildFullName(x.FirstName, x.MiddleName, x.LastName, x.MaternalName)
        }).ToList();

        return Result<List<StatisticEventTimelineDto>>.Success(items);
    }

    public async Task<Result<List<StatisticEventRankingDto>>> GetRankingAsync(
        int statisticId,
        int? tournamentId,
        int top,
        CancellationToken ct)
    {
        if (statisticId <= 0)
            return Result<List<StatisticEventRankingDto>>.Failure("statisticId is required and must be > 0");

        var safeTop = top <= 0 ? 10 : Math.Min(top, 200);

        var query = db.StatisticEvents
            .AsNoTracking()
            .Where(x => x.Active && x.StatisticId == statisticId)
            .Include(x => x.Roster)
                .ThenInclude(r => r.TournamentRoster)
                    .ThenInclude(tr => tr.SchoolTable)
            .Include(x => x.Roster)
                .ThenInclude(r => r.Match)
                    .ThenInclude(m => m.ParticipationA)
            .Include(x => x.Roster)
                .ThenInclude(r => r.Match)
                    .ThenInclude(m => m.ParticipationB)
            .AsQueryable();

        if (tournamentId.HasValue)
        {
            var tid = tournamentId.Value;
            query = query.Where(x =>
                x.Roster.Match.ParticipationA.TournamentId == tid ||
                x.Roster.Match.ParticipationB.TournamentId == tid);
        }

        var groupedRows = await query
            .GroupBy(x => new
            {
                TournamentRosterId = x.Roster.TournamentRosterId,
                StudentFirstName = x.Roster.TournamentRoster.FirstName,
                StudentMiddleName = x.Roster.TournamentRoster.MiddleName,
                StudentLastName = x.Roster.TournamentRoster.LastName,
                StudentMaternalName = x.Roster.TournamentRoster.MaternalName,
                SchoolId = x.Roster.TournamentRoster.SchoolId,
                SchoolName = x.Roster.TournamentRoster.SchoolTable.Name
            })
            .Select(g => new
            {
                g.Key.TournamentRosterId,
                g.Key.StudentFirstName,
                g.Key.StudentMiddleName,
                g.Key.StudentLastName,
                g.Key.StudentMaternalName,
                SchoolId = g.Key.SchoolId,
                SchoolName = g.Key.SchoolName,
                Count = g.Count()
            })
            .Take(safeTop)
            .ToListAsync(ct);

        var grouped = groupedRows
            .Select(x => new StatisticEventRankingDto
            {
                TournamentRosterId = x.TournamentRosterId,
                StudentName = BuildFullName(
                    x.StudentFirstName,
                    x.StudentMiddleName,
                    x.StudentLastName,
                    x.StudentMaternalName),
                SchoolId = x.SchoolId,
                SchoolName = x.SchoolName,
                Count = x.Count
            })
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.StudentName)
            .ToList();

        return Result<List<StatisticEventRankingDto>>.Success(grouped);
    }
}

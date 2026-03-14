using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.CopaUpsa.StatisticEvent;

public class StatisticEventQueries(CoreDbContext db)
{
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

        var items = await query
            .OrderBy(x => x.Moment)
            .ThenBy(x => x.Id)
            .Select(x => new StatisticEventTimelineDto
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
                SchoolStudentId = x.Roster.TournamentRosterId,
                StudentName = string.Join(" ",
                    new[]
                    {
                        x.Roster.TournamentRoster.FirstName,
                        x.Roster.TournamentRoster.MiddleName,
                        x.Roster.TournamentRoster.LastName,
                        x.Roster.TournamentRoster.MaternalName
                    }
                    .Where(s => !string.IsNullOrWhiteSpace(s)))
            })
            .ToListAsync(ct);

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

        var grouped = await query
            .GroupBy(x => new
            {
                SchoolStudentId = x.Roster.TournamentRosterId,
                StudentFirstName = x.Roster.TournamentRoster.FirstName,
                StudentMiddleName = x.Roster.TournamentRoster.MiddleName,
                StudentPaternalSurname = x.Roster.TournamentRoster.LastName,
                StudentMaternalSurname = x.Roster.TournamentRoster.MaternalName,
                x.Roster.TournamentRoster.SchoolId,
                SchoolName = x.Roster.TournamentRoster.SchoolTable.Name
            })
            .Select(g => new StatisticEventRankingDto
            {
                SchoolStudentId = g.Key.SchoolStudentId,
                StudentName = string.Join(" ",
                    new[]
                    {
                        g.Key.StudentFirstName,
                        g.Key.StudentMiddleName,
                        g.Key.StudentPaternalSurname,
                        g.Key.StudentMaternalSurname
                    }
                    .Where(s => !string.IsNullOrWhiteSpace(s))),
                SchoolId = g.Key.SchoolId,
                SchoolName = g.Key.SchoolName,
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.StudentName)
            .Take(safeTop)
            .ToListAsync(ct);

        return Result<List<StatisticEventRankingDto>>.Success(grouped);
    }
}

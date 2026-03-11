using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Match;

public class MatchQueries(CoreDbContext db)
{
    private IQueryable<Data.Entity.CopaUpsa.Match> BuildScheduleQuery()
    {
        return db.Matches
            .AsNoTracking()
            .Where(x => x.Active)
            .Include(x => x.MatchStatus)
            .Include(x => x.ParticipationA)
                .ThenInclude(p => p.SchoolTable)
            .Include(x => x.ParticipationA)
                .ThenInclude(p => p.Tournament)
            .Include(x => x.ParticipationA)
                .ThenInclude(p => p.PhaseType)
            .Include(x => x.ParticipationB)
                .ThenInclude(p => p.SchoolTable)
            .Include(x => x.ParticipationB)
                .ThenInclude(p => p.Tournament)
            .Include(x => x.ParticipationB)
                .ThenInclude(p => p.PhaseType)
            .AsQueryable();
    }

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

    public async Task<Result<MatchScheduleDto>> GetDetailAsync(int id, CancellationToken ct)
    {
        var item = await BuildScheduleQuery()
            .Where(x => x.Id == id)
            .Select(x => new MatchScheduleDto
            {
                Id = x.Id,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Location = x.Location,
                ScoreA = x.ScoreA,
                ScoreB = x.ScoreB,
                MatchStatusId = x.MatchStatusId,
                MatchStatusName = x.MatchStatus.Name,

                ParticipationAId = x.ParticipationAId,
                TeamAId = x.ParticipationA.SchoolId,
                TeamAName = x.ParticipationA.SchoolTable.Name,
                TeamALogo = x.ParticipationA.SchoolTable.SportLogo ?? x.ParticipationA.SchoolTable.NormalLogo,
                TournamentAId = x.ParticipationA.TournamentId,
                TournamentAName = x.ParticipationA.Tournament.Name,
                PhaseTypeAId = x.ParticipationA.PhaseTypeId,
                PhaseTypeAName = x.ParticipationA.PhaseType.Name,

                ParticipationBId = x.ParticipationBId,
                TeamBId = x.ParticipationB.SchoolId,
                TeamBName = x.ParticipationB.SchoolTable.Name,
                TeamBLogo = x.ParticipationB.SchoolTable.SportLogo ?? x.ParticipationB.SchoolTable.NormalLogo,
                TournamentBId = x.ParticipationB.TournamentId,
                TournamentBName = x.ParticipationB.Tournament.Name,
                PhaseTypeBId = x.ParticipationB.PhaseTypeId,
                PhaseTypeBName = x.ParticipationB.PhaseType.Name
            })
            .FirstOrDefaultAsync(ct);

        return item is null
            ? Result<MatchScheduleDto>.Failure($"Match with id {id} not found")
            : Result<MatchScheduleDto>.Success(item);
    }

    public async Task<Result<List<MatchScheduleDto>>> GetScheduleAsync(
        DateTime? startDateFrom,
        DateTime? startDateTo,
        int? tournamentId,
        CancellationToken ct)
    {
        var query = BuildScheduleQuery();

        if (startDateFrom.HasValue)
            query = query.Where(x => x.StartDate >= startDateFrom.Value);

        if (startDateTo.HasValue)
            query = query.Where(x => x.StartDate <= startDateTo.Value);

        if (tournamentId.HasValue)
        {
            var tid = tournamentId.Value;
            query = query.Where(x =>
                x.ParticipationA.TournamentId == tid ||
                x.ParticipationB.TournamentId == tid);
        }

        var items = await query
            .OrderBy(x => x.StartDate)
            .ThenBy(x => x.NumberMatch)
            .ThenBy(x => x.Id)
            .Select(x => new MatchScheduleDto
            {
                Id = x.Id,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Location = x.Location,
                ScoreA = x.ScoreA,
                ScoreB = x.ScoreB,
                MatchStatusId = x.MatchStatusId,
                MatchStatusName = x.MatchStatus.Name,

                ParticipationAId = x.ParticipationAId,
                TeamAId = x.ParticipationA.SchoolId,
                TeamAName = x.ParticipationA.SchoolTable.Name,
                TeamALogo = x.ParticipationA.SchoolTable.SportLogo ?? x.ParticipationA.SchoolTable.NormalLogo,
                TournamentAId = x.ParticipationA.TournamentId,
                TournamentAName = x.ParticipationA.Tournament.Name,
                PhaseTypeAId = x.ParticipationA.PhaseTypeId,
                PhaseTypeAName = x.ParticipationA.PhaseType.Name,

                ParticipationBId = x.ParticipationBId,
                TeamBId = x.ParticipationB.SchoolId,
                TeamBName = x.ParticipationB.SchoolTable.Name,
                TeamBLogo = x.ParticipationB.SchoolTable.SportLogo ?? x.ParticipationB.SchoolTable.NormalLogo,
                TournamentBId = x.ParticipationB.TournamentId,
                TournamentBName = x.ParticipationB.Tournament.Name,
                PhaseTypeBId = x.ParticipationB.PhaseTypeId,
                PhaseTypeBName = x.ParticipationB.PhaseType.Name
            })
            .ToListAsync(ct);

        return Result<List<MatchScheduleDto>>.Success(items);
    }

    public async Task<Result<List<string>>> GetAvailableDatesAsync(int? tournamentId, CancellationToken ct)
    {
        var query = db.Matches
            .AsNoTracking()
            .Where(x => x.Active)
            .AsQueryable();

        if (tournamentId.HasValue)
        {
            var tid = tournamentId.Value;
            query = query.Where(x =>
                x.ParticipationA.TournamentId == tid ||
                x.ParticipationB.TournamentId == tid);
        }

        var dates = await query
            .OrderBy(x => x.StartDate)
            .Select(x => x.StartDate.Date)
            .Distinct()
            .ToListAsync(ct);

        var result = dates
            .Select(d => d.ToString("yyyy-MM-dd"))
            .ToList();

        return Result<List<string>>.Success(result);
    }

    public async Task<Result<List<MatchStandingDto>>> GetStandingsAsync(int tournamentId, int? phaseTypeId, CancellationToken ct)
    {
        if (tournamentId <= 0)
            return Result<List<MatchStandingDto>>.Failure("tournamentId is required and must be > 0");

        var query = BuildScheduleQuery().Where(x =>
            x.ParticipationA.TournamentId == tournamentId ||
            x.ParticipationB.TournamentId == tournamentId);

        if (phaseTypeId.HasValue)
        {
            var pid = phaseTypeId.Value;
            query = query.Where(x =>
                x.ParticipationA.PhaseTypeId == pid ||
                x.ParticipationB.PhaseTypeId == pid);
        }

        var matches = await query
            .OrderBy(x => x.StartDate)
            .ThenBy(x => x.NumberMatch)
            .ToListAsync(ct);

        var standings = new Dictionary<int, MatchStandingDto>();

        void EnsureTeam(int schoolId, string schoolName, string? schoolLogo)
        {
            if (standings.ContainsKey(schoolId)) return;
            standings[schoolId] = new MatchStandingDto
            {
                SchoolId = schoolId,
                SchoolName = schoolName,
                SchoolLogo = schoolLogo,
                Played = 0,
                Won = 0,
                Drawn = 0,
                Lost = 0,
                GoalsFor = 0,
                GoalsAgainst = 0,
                GoalDifference = 0,
                Points = 0,
                Position = 0
            };
        }

        foreach (var match in matches)
        {
            var teamAId = match.ParticipationA.SchoolId;
            var teamBId = match.ParticipationB.SchoolId;
            var teamA = match.ParticipationA.SchoolTable;
            var teamB = match.ParticipationB.SchoolTable;

            EnsureTeam(teamAId, teamA.Name, teamA.SportLogo ?? teamA.NormalLogo);
            EnsureTeam(teamBId, teamB.Name, teamB.SportLogo ?? teamB.NormalLogo);

            var scoreA = (int)match.ScoreA;
            var scoreB = (int)match.ScoreB;

            standings[teamAId].Played++;
            standings[teamBId].Played++;
            standings[teamAId].GoalsFor += scoreA;
            standings[teamAId].GoalsAgainst += scoreB;
            standings[teamBId].GoalsFor += scoreB;
            standings[teamBId].GoalsAgainst += scoreA;

            if (scoreA > scoreB)
            {
                standings[teamAId].Won++;
                standings[teamAId].Points += 3;
                standings[teamBId].Lost++;
            }
            else if (scoreB > scoreA)
            {
                standings[teamBId].Won++;
                standings[teamBId].Points += 3;
                standings[teamAId].Lost++;
            }
            else
            {
                standings[teamAId].Drawn++;
                standings[teamAId].Points += 1;
                standings[teamBId].Drawn++;
                standings[teamBId].Points += 1;
            }
        }

        var ordered = standings.Values
            .Select(x =>
            {
                x.GoalDifference = x.GoalsFor - x.GoalsAgainst;
                return x;
            })
            .OrderByDescending(x => x.Points)
            .ThenByDescending(x => x.GoalDifference)
            .ThenByDescending(x => x.GoalsFor)
            .ThenBy(x => x.SchoolName)
            .ToList();

        for (var i = 0; i < ordered.Count; i++)
            ordered[i].Position = i + 1;

        return Result<List<MatchStandingDto>>.Success(ordered);
    }
}

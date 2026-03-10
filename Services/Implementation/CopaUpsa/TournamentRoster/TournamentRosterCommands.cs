using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.CopaUpsa.TournamentRoster;

public class TournamentRosterCommands(IBaseCrud<Data.Entity.CopaUpsa.TournamentRoster> baseCrud, CoreDbContext db)
{
    public async Task<Result<TournamentRosterReadDto>> CreateAsync(TournamentRosterCreateDto dto, CancellationToken ct)
    {
        var validation = await ValidateForeignKeysAsync(dto.TournamentId, dto.SchoolId, ct);
        if (!validation.IsSuccess)
            return Result<TournamentRosterReadDto>.Failure(validation.Errors);

        var entity = TournamentRosterMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);

        await db.Entry(created).Reference(tr => tr.Tournament).LoadAsync(ct);
        await db.Entry(created).Reference(tr => tr.SchoolTable).LoadAsync(ct);

        return Result<TournamentRosterReadDto>.Success(TournamentRosterMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, TournamentRosterUpdateDto dto, CancellationToken ct)
    {
        var errors = new List<string>();
        if (dto.TournamentId.HasValue && !await db.Tournaments.AnyAsync(x => x.Id == dto.TournamentId.Value, ct))
            errors.Add($"TournamentId ({dto.TournamentId.Value}) not found");
        if (dto.SchoolId.HasValue && !await db.Schools.AnyAsync(x => x.Id == dto.SchoolId.Value, ct))
            errors.Add($"SchoolId ({dto.SchoolId.Value}) not found");
        if (errors.Count > 0)
            return Result<bool>.Failure(errors);

        var updated = await baseCrud.UpdateAsync(id, e => TournamentRosterMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"TournamentRoster with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"TournamentRoster with id {id} not found");
    }

    private async Task<Result<bool>> ValidateForeignKeysAsync(int tournamentId, int schoolId, CancellationToken ct)
    {
        var errors = new List<string>();

        if (!await db.Tournaments.AnyAsync(x => x.Id == tournamentId, ct))
            errors.Add($"TournamentId ({tournamentId}) not found");
        if (!await db.Schools.AnyAsync(x => x.Id == schoolId, ct))
            errors.Add($"SchoolId ({schoolId}) not found");

        return errors.Count > 0 ? Result<bool>.Failure(errors) : Result<bool>.Success(true);
    }
}


using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Tournament;

public class TournamentCommands(IBaseCrud<Data.Entity.CopaUpsa.Tournament> baseCrud, CoreDbContext db)
{
    public async Task<Result<TournamentReadDto>> CreateAsync(TournamentCreateDto dto, CancellationToken ct)
    {
        var validation = await ValidateForeignKeysAsync(dto.TournamentParentId, dto.SportId, ct);
        if (!validation.IsSuccess)
            return Result<TournamentReadDto>.Failure(validation.Errors);

        var entity = TournamentMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<TournamentReadDto>.Success(TournamentMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, TournamentUpdateDto dto, CancellationToken ct)
    {
        var validation = await ValidateForeignKeysAsync(
            dto.TournamentParentId ?? 0, dto.SportId ?? 0, ct, dto.TournamentParentId.HasValue, dto.SportId.HasValue);
        if (!validation.IsSuccess)
            return Result<bool>.Failure(validation.Errors);

        var updated = await baseCrud.UpdateAsync(id, e => TournamentMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Tournament with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Tournament with id {id} not found");
    }

    private async Task<Result<bool>> ValidateForeignKeysAsync(
        int tournamentParentId, int sportId, CancellationToken ct,
        bool checkParent = true, bool checkSport = true)
    {
        var errors = new List<string>();

        if (checkParent && !await db.TournamentParents.AnyAsync(x => x.Id == tournamentParentId, ct))
            errors.Add($"TournamentParentId ({tournamentParentId}) not found");

        if (checkSport && !await db.Sports.AnyAsync(x => x.Id == sportId, ct))
            errors.Add($"SportId ({sportId}) not found");

        return errors.Count > 0 ? Result<bool>.Failure(errors) : Result<bool>.Success(true);
    }
}


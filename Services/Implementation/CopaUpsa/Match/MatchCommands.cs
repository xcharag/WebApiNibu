using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Match;

public class MatchCommands(IBaseCrud<Data.Entity.CopaUpsa.Match> baseCrud, CoreDbContext db)
{
    public async Task<Result<MatchReadDto>> CreateAsync(MatchCreateDto dto, CancellationToken ct)
    {
        var validation = await ValidateForeignKeysAsync(dto.ParticipationId, dto.MatchStatusId, ct);
        if (!validation.IsSuccess)
            return Result<MatchReadDto>.Failure(validation.Errors);

        var entity = MatchMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<MatchReadDto>.Success(MatchMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, MatchUpdateDto dto, CancellationToken ct)
    {
        var errors = new List<string>();
        if (dto.ParticipationId.HasValue && !await db.Participations.AnyAsync(x => x.Id == dto.ParticipationId.Value, ct))
            errors.Add($"ParticipationId ({dto.ParticipationId.Value}) not found");
        if (dto.MatchStatusId.HasValue && !await db.MatchStatuses.AnyAsync(x => x.Id == dto.MatchStatusId.Value, ct))
            errors.Add($"MatchStatusId ({dto.MatchStatusId.Value}) not found");
        if (errors.Count > 0)
            return Result<bool>.Failure(errors);

        var updated = await baseCrud.UpdateAsync(id, e => MatchMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Match with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Match with id {id} not found");
    }

    private async Task<Result<bool>> ValidateForeignKeysAsync(int participationId, int matchStatusId, CancellationToken ct)
    {
        var errors = new List<string>();

        if (!await db.Participations.AnyAsync(x => x.Id == participationId, ct))
            errors.Add($"ParticipationId ({participationId}) not found");
        if (!await db.MatchStatuses.AnyAsync(x => x.Id == matchStatusId, ct))
            errors.Add($"MatchStatusId ({matchStatusId}) not found");

        return errors.Count > 0 ? Result<bool>.Failure(errors) : Result<bool>.Success(true);
    }
}


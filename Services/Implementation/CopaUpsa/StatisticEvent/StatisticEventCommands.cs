using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.CopaUpsa.StatisticEvent;

public class StatisticEventCommands(IBaseCrud<Data.Entity.CopaUpsa.StatisticEvent> baseCrud, CoreDbContext db)
{
    public async Task<Result<StatisticEventReadDto>> CreateAsync(StatisticEventCreateDto dto, CancellationToken ct)
    {
        var validation = await ValidateForeignKeysAsync(dto.StatisticId, dto.RosterId, ct);
        if (!validation.IsSuccess)
            return Result<StatisticEventReadDto>.Failure(validation.Errors);

        var entity = StatisticEventMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<StatisticEventReadDto>.Success(StatisticEventMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, StatisticEventUpdateDto dto, CancellationToken ct)
    {
        var errors = new List<string>();
        if (dto.StatisticId.HasValue && !await db.Statistics.AnyAsync(x => x.Id == dto.StatisticId.Value, ct))
            errors.Add($"StatisticId ({dto.StatisticId.Value}) not found");
        if (dto.RosterId.HasValue && !await db.Rosters.AnyAsync(x => x.Id == dto.RosterId.Value, ct))
            errors.Add($"RosterId ({dto.RosterId.Value}) not found");
        if (errors.Count > 0)
            return Result<bool>.Failure(errors);

        var updated = await baseCrud.UpdateAsync(id, e => StatisticEventMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"StatisticEvent with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"StatisticEvent with id {id} not found");
    }

    private async Task<Result<bool>> ValidateForeignKeysAsync(int statisticId, int rosterId, CancellationToken ct)
    {
        var errors = new List<string>();

        if (!await db.Statistics.AnyAsync(x => x.Id == statisticId, ct))
            errors.Add($"StatisticId ({statisticId}) not found");
        if (!await db.Rosters.AnyAsync(x => x.Id == rosterId, ct))
            errors.Add($"RosterId ({rosterId}) not found");

        return errors.Count > 0 ? Result<bool>.Failure(errors) : Result<bool>.Success(true);
    }
}


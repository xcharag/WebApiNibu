using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Statistic;

public class StatisticCommands(IBaseCrud<Data.Entity.CopaUpsa.Statistic> baseCrud, CoreDbContext db)
{
    public async Task<Result<StatisticReadDto>> CreateAsync(StatisticCreateDto dto, CancellationToken ct)
    {
        if (!await db.Sports.AnyAsync(x => x.Id == dto.SportId, ct))
            return Result<StatisticReadDto>.Failure($"SportId ({dto.SportId}) not found");

        var entity = StatisticMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<StatisticReadDto>.Success(StatisticMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, StatisticUpdateDto dto, CancellationToken ct)
    {
        if (dto.SportId.HasValue && !await db.Sports.AnyAsync(x => x.Id == dto.SportId.Value, ct))
            return Result<bool>.Failure($"SportId ({dto.SportId.Value}) not found");

        var updated = await baseCrud.UpdateAsync(id, e => StatisticMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Statistic with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Statistic with id {id} not found");
    }
}


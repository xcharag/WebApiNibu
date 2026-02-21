using WebApiNibu.Abstraction;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.CopaUpsa.MatchStatus;

public class MatchStatusCommands(IBaseCrud<Data.Entity.CopaUpsa.MatchStatus> baseCrud)
{
    public async Task<Result<MatchStatusReadDto>> CreateAsync(MatchStatusCreateDto dto, CancellationToken ct)
    {
        var entity = MatchStatusMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<MatchStatusReadDto>.Success(MatchStatusMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, MatchStatusUpdateDto dto, CancellationToken ct)
    {
        var updated = await baseCrud.UpdateAsync(id, e => MatchStatusMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"MatchStatus with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"MatchStatus with id {id} not found");
    }
}


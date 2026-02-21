using WebApiNibu.Abstraction;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Position;

public class PositionCommands(IBaseCrud<Data.Entity.CopaUpsa.Position> baseCrud)
{
    public async Task<Result<PositionReadDto>> CreateAsync(PositionCreateDto dto, CancellationToken ct)
    {
        var entity = PositionMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<PositionReadDto>.Success(PositionMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, PositionUpdateDto dto, CancellationToken ct)
    {
        var updated = await baseCrud.UpdateAsync(id, e => PositionMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Position with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Position with id {id} not found");
    }
}


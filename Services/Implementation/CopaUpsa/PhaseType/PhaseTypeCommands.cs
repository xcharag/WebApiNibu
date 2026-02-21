using WebApiNibu.Abstraction;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.CopaUpsa.PhaseType;

public class PhaseTypeCommands(IBaseCrud<Data.Entity.CopaUpsa.PhaseType> baseCrud)
{
    public async Task<Result<PhaseTypeReadDto>> CreateAsync(PhaseTypeCreateDto dto, CancellationToken ct)
    {
        var entity = PhaseTypeMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<PhaseTypeReadDto>.Success(PhaseTypeMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, PhaseTypeUpdateDto dto, CancellationToken ct)
    {
        var updated = await baseCrud.UpdateAsync(id, e => PhaseTypeMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"PhaseType with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"PhaseType with id {id} not found");
    }
}


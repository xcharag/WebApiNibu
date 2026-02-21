using WebApiNibu.Abstraction;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.CopaUpsa.Sport;

public class SportCommands(IBaseCrud<Data.Entity.CopaUpsa.Sport> baseCrud)
{
    public async Task<Result<SportReadDto>> CreateAsync(SportCreateDto dto, CancellationToken ct)
    {
        var entity = SportMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<SportReadDto>.Success(SportMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, SportUpdateDto dto, CancellationToken ct)
    {
        var updated = await baseCrud.UpdateAsync(id, e => SportMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Sport with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Sport with id {id} not found");
    }
}


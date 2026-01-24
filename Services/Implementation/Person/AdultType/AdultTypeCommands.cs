using WebApiNibu.Abstraction;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.AdultType;

public class AdultTypeCommands(IBaseCrud<Data.Entity.Person.AdultType> baseCrud)
{
    public async Task<Result<AdultTypeReadDto>> CreateAsync(AdultTypeCreateDto dto, CancellationToken ct)
    {
        var entity = AdultTypeMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<AdultTypeReadDto>.Success(AdultTypeMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, AdultTypeUpdateDto dto, CancellationToken ct)
    {
        var updated = await baseCrud.UpdateAsync(id, e => AdultTypeMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"AdultType with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"AdultType with id {id} not found");
    }
}

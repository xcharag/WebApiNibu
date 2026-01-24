using WebApiNibu.Abstraction;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.Carreer;

public class CarreerCommands(IBaseCrud<Data.Entity.Person.Carreer> baseCrud)
{
    public async Task<Result<CarreerReadDto>> CreateAsync(CarreerCreateDto dto, CancellationToken ct)
    {
        var entity = CarreerMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<CarreerReadDto>.Success(CarreerMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, CarreerUpdateDto dto, CancellationToken ct)
    {
        var updated = await baseCrud.UpdateAsync(id, e => CarreerMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Carreer with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Carreer with id {id} not found");
    }
}

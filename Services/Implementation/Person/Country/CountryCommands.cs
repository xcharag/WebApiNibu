using WebApiNibu.Abstraction;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.Country;

public class CountryCommands(IBaseCrud<Data.Entity.Person.Country> baseCrud)
{
    public async Task<Result<CountryReadDto>> CreateAsync(CountryCreateDto dto, CancellationToken ct)
    {
        var entity = CountryMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<CountryReadDto>.Success(CountryMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, CountryUpdateDto dto, CancellationToken ct)
    {
        var updated = await baseCrud.UpdateAsync(id, e => CountryMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Country with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Country with id {id} not found");
    }
}

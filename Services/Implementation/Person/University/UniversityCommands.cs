using WebApiNibu.Abstraction;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.University;

public class UniversityCommands(IBaseCrud<Data.Entity.Person.University> baseCrud)
{
    public async Task<Result<UniversityReadDto>> CreateAsync(UniversityCreateDto dto, CancellationToken ct)
    {
        var entity = UniversityMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<UniversityReadDto>.Success(UniversityMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, UniversityUpdateDto dto, CancellationToken ct)
    {
        var updated = await baseCrud.UpdateAsync(id, e => UniversityMapper.ApplyUpdate(e, dto), ct);
        return updated ? Result<bool>.Success(true) : Result<bool>.Failure($"University with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted ? Result<bool>.Success(true) : Result<bool>.Failure($"University with id {id} not found");
    }
}

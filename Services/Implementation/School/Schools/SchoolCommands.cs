using WebApiNibu.Abstraction;
using WebApiNibu.Data.Dto.School;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.School.Schools;

public class SchoolCommands(IBaseCrud<Data.Entity.School.SchoolTable> baseCrud)
{
    public async Task<Result<SchoolReadDto>> CreateAsync(SchoolCreateDto dto, CancellationToken ct)
    {
        var entity = SchoolMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<SchoolReadDto>.Success(SchoolMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, SchoolUpdateDto dto, CancellationToken ct)
    {
        var updated = await baseCrud.UpdateAsync(id, e => SchoolMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"School with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"School with id {id} not found");
    }
}
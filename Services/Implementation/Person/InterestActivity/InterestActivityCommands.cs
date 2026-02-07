using WebApiNibu.Abstraction;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.InterestActivity;

public class InterestActivityCommands(IBaseCrud<Data.Entity.Person.InterestActivity> baseCrud)
{
    public async Task<Result<InterestActivitieReadDto>> CreateAsync(InterestActivitieCreateDto dto, CancellationToken ct)
    {
        var entity = InterestActivityMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<InterestActivitieReadDto>.Success(InterestActivityMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, InterestActivitieUpdateDto dto, CancellationToken ct)
    {
        var updated = await baseCrud.UpdateAsync(id, e => InterestActivityMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"InterestActivity with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"InterestActivity with id {id} not found");
    }
}

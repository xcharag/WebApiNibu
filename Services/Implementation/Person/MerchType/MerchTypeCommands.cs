using WebApiNibu.Abstraction;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.MerchType;

public class MerchTypeCommands(IBaseCrud<Data.Entity.Person.MerchType> baseCrud)
{
    public async Task<Result<MerchTypeReadDto>> CreateAsync(MerchTypeCreateDto dto, CancellationToken ct)
    {
        var entity = MerchTypeMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<MerchTypeReadDto>.Success(MerchTypeMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, MerchTypeUpdateDto dto, CancellationToken ct)
    {
        var updated = await baseCrud.UpdateAsync(id, e => MerchTypeMapper.ApplyUpdate(e, dto), ct);
        return updated ? Result<bool>.Success(true) : Result<bool>.Failure($"MerchType with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted ? Result<bool>.Success(true) : Result<bool>.Failure($"MerchType with id {id} not found");
    }
}

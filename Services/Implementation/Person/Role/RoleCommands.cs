using WebApiNibu.Abstraction;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.Role;

public class RoleCommands(IBaseCrud<Data.Entity.Person.Role> baseCrud)
{
    public async Task<Result<RoleReadDto>> CreateAsync(RoleCreateDto dto, CancellationToken ct)
    {
        var entity = RoleMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<RoleReadDto>.Success(RoleMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, RoleUpdateDto dto, CancellationToken ct)
    {
        var updated = await baseCrud.UpdateAsync(id, e => RoleMapper.ApplyUpdate(e, dto), ct);
        return updated ? Result<bool>.Success(true) : Result<bool>.Failure($"Role with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted ? Result<bool>.Success(true) : Result<bool>.Failure($"Role with id {id} not found");
    }
}

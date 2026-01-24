using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.Worker;

public class WorkerCommands(IBaseCrud<Data.Entity.Person.Worker> baseCrud, OracleDbContext db)
{
    public async Task<Result<WorkerReadDto>> CreateAsync(WorkerCreateDto dto, CancellationToken ct)
    {
        var validation = await ValidateForeignKeysAsync(dto.RoleId, ct);
        if (!validation.IsSuccess)
            return Result<WorkerReadDto>.Failure(validation.Errors);

        var entity = WorkerMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<WorkerReadDto>.Success(WorkerMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, WorkerUpdateDto dto, CancellationToken ct)
    {
        var validation = await ValidateForeignKeysAsync(dto.RoleId, ct);
        if (!validation.IsSuccess)
            return Result<bool>.Failure(validation.Errors);

        var updated = await baseCrud.UpdateAsync(id, e => WorkerMapper.ApplyUpdate(e, dto), ct);
        return updated ? Result<bool>.Success(true) : Result<bool>.Failure($"Worker with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted ? Result<bool>.Success(true) : Result<bool>.Failure($"Worker with id {id} not found");
    }

    private async Task<Result<bool>> ValidateForeignKeysAsync(int roleId, CancellationToken ct)
    {
        if (!await db.Roles.AnyAsync(r => r.Id == roleId, ct))
            return Result<bool>.Failure($"RoleId ({roleId}) not found");
        return Result<bool>.Success(true);
    }
}

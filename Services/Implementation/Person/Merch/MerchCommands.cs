using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.Merch;

public class MerchCommands(IBaseCrud<Data.Entity.Person.Merch> baseCrud, OracleDbContext db)
{
    public async Task<Result<MerchReadDto>> CreateAsync(MerchCreateDto dto, CancellationToken ct)
    {
        var validation = await ValidateForeignKeysAsync(dto.MerchTypeId, ct);
        if (!validation.IsSuccess)
            return Result<MerchReadDto>.Failure(validation.Errors);

        var entity = MerchMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<MerchReadDto>.Success(MerchMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, MerchUpdateDto dto, CancellationToken ct)
    {
        var validation = await ValidateForeignKeysAsync(dto.MerchTypeId, ct);
        if (!validation.IsSuccess)
            return Result<bool>.Failure(validation.Errors);

        var updated = await baseCrud.UpdateAsync(id, e => MerchMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Merch with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Merch with id {id} not found");
    }

    // ─────────────────────────────── Validation ───────────────────────────────

    private async Task<Result<bool>> ValidateForeignKeysAsync(int merchTypeId, CancellationToken ct)
    {
        var errors = new List<string>();

        if (!await db.MerchTypes.AnyAsync(mt => mt.Id == merchTypeId, ct))
            errors.Add($"MerchTypeId ({merchTypeId}) not found");

        return errors.Count > 0
            ? Result<bool>.Failure(errors)
            : Result<bool>.Success(true);
    }
}

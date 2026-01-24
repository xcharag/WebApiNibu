using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.MerchObtention;

public class MerchObtentionCommands(IBaseCrud<Data.Entity.Person.MerchObtention> baseCrud, OracleDbContext db)
{
    public async Task<Result<MerchObtentionReadDto>> CreateAsync(MerchObtentionCreateDto dto, CancellationToken ct)
    {
        var validation = await ValidateForeignKeysAsync(dto.SchoolStudentId, dto.MerchId, ct);
        if (!validation.IsSuccess)
            return Result<MerchObtentionReadDto>.Failure(validation.Errors);

        var entity = MerchObtentionMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<MerchObtentionReadDto>.Success(MerchObtentionMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, MerchObtentionCreateDto dto, CancellationToken ct)
    {
        var validation = await ValidateForeignKeysAsync(dto.SchoolStudentId, dto.MerchId, ct);
        if (!validation.IsSuccess)
            return Result<bool>.Failure(validation.Errors);

        var updated = await baseCrud.UpdateAsync(id, e => MerchObtentionMapper.ApplyUpdate(e, dto), ct);
        return updated ? Result<bool>.Success(true) : Result<bool>.Failure($"MerchObtention with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted ? Result<bool>.Success(true) : Result<bool>.Failure($"MerchObtention with id {id} not found");
    }

    // ─────────────────────────────── Validation ───────────────────────────────

    private async Task<Result<bool>> ValidateForeignKeysAsync(int schoolStudentId, int merchId, CancellationToken ct)
    {
        var errors = new List<string>();

        if (!await db.SchoolStudents.AnyAsync(s => s.Id == schoolStudentId, ct))
            errors.Add($"SchoolStudentId ({schoolStudentId}) not found");

        if (!await db.Merchs.AnyAsync(m => m.Id == merchId, ct))
            errors.Add($"MerchId ({merchId}) not found");

        return errors.Count > 0 ? Result<bool>.Failure(errors) : Result<bool>.Success(true);
    }
}

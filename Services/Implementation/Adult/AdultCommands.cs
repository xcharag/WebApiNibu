using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Adult;

public class AdultCommands(IBaseCrud<Data.Entity.Person.Adult> baseCrud, OracleDbContext db)
{
    public async Task<Result<AdultReadDto>> CreateAsync(AdultCreateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.AdultTypeId, dto.SchoolStudentId, ct);
        if (!validationResult.IsSuccess)
            return Result<AdultReadDto>.Failure(validationResult.Errors);

        var entity = AdultMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<AdultReadDto>.Success(AdultMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, AdultUpdateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.AdultTypeId, dto.SchoolStudentId, ct);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors);

        var updated = await baseCrud.UpdateAsync(id, e => AdultMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Adult with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Adult with id {id} not found");
    }

    private async Task<Result<bool>> ValidateForeignKeysAsync(int adultTypeId, int schoolStudentId, CancellationToken ct)
    {
        var errors = new List<string>();

        if (!await db.AdultTypes.AnyAsync(a => a.Id == adultTypeId, ct))
            errors.Add($"AdultTypeId ({adultTypeId}) not found");

        if (!await db.SchoolStudents.AnyAsync(s => s.Id == schoolStudentId, ct))
            errors.Add($"SchoolStudentId ({schoolStudentId}) not found");

        return errors.Count > 0
            ? Result<bool>.Failure(errors)
            : Result<bool>.Success(true);
    }
}

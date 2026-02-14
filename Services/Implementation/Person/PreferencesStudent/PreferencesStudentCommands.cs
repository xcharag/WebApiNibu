using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.PreferencesStudent;

public class PreferencesStudentCommands(IBaseCrud<Data.Entity.Person.PreferencesStudent> baseCrud, CoreDbContext db)
{
    public async Task<Result<PreferencesStudentReadDto>> CreateAsync(PreferencesStudentCreateDto dto, CancellationToken ct)
    {
        var validation = await ValidateForeignKeysAsync(dto.SchoolStudentId, ct);
        if (!validation.IsSuccess)
            return Result<PreferencesStudentReadDto>.Failure(validation.Errors);

        var entity = PreferencesStudentMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<PreferencesStudentReadDto>.Success(PreferencesStudentMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, PreferencesStudentUpdateDto dto, CancellationToken ct)
    {
        var validation = await ValidateForeignKeysAsync(dto.SchoolStudentId, ct);
        if (!validation.IsSuccess)
            return Result<bool>.Failure(validation.Errors);

        var updated = await baseCrud.UpdateAsync(id, e => PreferencesStudentMapper.ApplyUpdate(e, dto), ct);
        return updated ? Result<bool>.Success(true) : Result<bool>.Failure($"PreferencesStudent with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted ? Result<bool>.Success(true) : Result<bool>.Failure($"PreferencesStudent with id {id} not found");
    }

    private async Task<Result<bool>> ValidateForeignKeysAsync(int schoolStudentId, CancellationToken ct)
    {
        if (!await db.SchoolStudents.AnyAsync(s => s.Id == schoolStudentId, ct))
            return Result<bool>.Failure($"SchoolStudentId ({schoolStudentId}) not found");
        return Result<bool>.Success(true);
    }
}

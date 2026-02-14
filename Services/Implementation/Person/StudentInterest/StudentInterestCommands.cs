using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.StudentInterest;

public class StudentInterestCommands(IBaseCrud<Data.Entity.Person.StudentInterest> baseCrud, CoreDbContext db)
{
    public async Task<Result<StudentInterestReadDto>> CreateAsync(StudentInterestCreateDto dto, CancellationToken ct)
    {
        var validation = await ValidateForeignKeysAsync(dto.SchoolStudentId, dto.InterestActivitieId, ct);
        if (!validation.IsSuccess)
            return Result<StudentInterestReadDto>.Failure(validation.Errors);

        var entity = StudentInterestMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<StudentInterestReadDto>.Success(StudentInterestMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, StudentInterestUpdateDto dto, CancellationToken ct)
    {
        var validation = await ValidateForeignKeysAsync(dto.SchoolStudentId, dto.InterestActivitieId, ct);
        if (!validation.IsSuccess)
            return Result<bool>.Failure(validation.Errors);

        var updated = await baseCrud.UpdateAsync(id, e => StudentInterestMapper.ApplyUpdate(e, dto), ct);
        return updated ? Result<bool>.Success(true) : Result<bool>.Failure($"StudentInterest with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted ? Result<bool>.Success(true) : Result<bool>.Failure($"StudentInterest with id {id} not found");
    }

    private async Task<Result<bool>> ValidateForeignKeysAsync(int schoolStudentId, int interestActivityId, CancellationToken ct)
    {
        var errors = new List<string>();

        if (!await db.SchoolStudents.AnyAsync(s => s.Id == schoolStudentId, ct))
            errors.Add($"SchoolStudentId ({schoolStudentId}) not found");

        if (!await db.InterestActivities.AnyAsync(i => i.Id == interestActivityId, ct))
            errors.Add($"InterestActivitieId ({interestActivityId}) not found");

        return errors.Count > 0 ? Result<bool>.Failure(errors) : Result<bool>.Success(true);
    }
}

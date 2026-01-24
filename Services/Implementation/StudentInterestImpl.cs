using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Data.Entity.Person;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract;

namespace WebApiNibu.Services.Implementation;

public class StudentInterestImpl(IBaseCrud<StudentInterest> baseCrud, OracleDbContext db) : IStudentInterest
{
    // ─────────────────────────────── Queries ───────────────────────────────

    public async Task<Result<IEnumerable<StudentInterestReadDto>>> GetAllAsync(CancellationToken ct)
    {
        var items = await baseCrud.GetAllAsync(true, ct);
        var dtos = items.Select(MapToReadDto);
        return Result<IEnumerable<StudentInterestReadDto>>.Success(dtos);
    }

    public async Task<Result<StudentInterestReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await baseCrud.GetByIdAsync(id, true, ct);
        return item is null
            ? Result<StudentInterestReadDto>.Failure($"StudentInterest with id {id} not found")
            : Result<StudentInterestReadDto>.Success(MapToReadDto(item));
    }

    public async Task<Result<IEnumerable<StudentInterestReadDto>>> GetFilteredAsync(StudentInterestFilter filter, CancellationToken ct)
    {
        var query = db.StudentInterests.AsQueryable();
        query = ApplyFilters(query, filter);
        var items = await query.ToListAsync(ct);
        var dtos = items.Select(MapToReadDto);
        return Result<IEnumerable<StudentInterestReadDto>>.Success(dtos);
    }

    // ─────────────────────────────── Commands ───────────────────────────────

    public async Task<Result<StudentInterestReadDto>> CreateAsync(StudentInterestCreateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.SchoolStudentId, dto.InterestActivitieId, ct);
        if (!validationResult.IsSuccess)
            return Result<StudentInterestReadDto>.Failure(validationResult.Errors);

        var entity = MapFromCreateDto(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<StudentInterestReadDto>.Success(MapToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, StudentInterestUpdateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.SchoolStudentId, dto.InterestActivitieId, ct);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors);

        var updated = await baseCrud.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"StudentInterest with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"StudentInterest with id {id} not found");
    }

    // ─────────────────────────────── Validation ───────────────────────────────

    private async Task<Result<bool>> ValidateForeignKeysAsync(int schoolStudentId, int interestActivitieId, CancellationToken ct)
    {
        var errors = new List<string>();

        if (!await db.SchoolStudents.AnyAsync(s => s.Id == schoolStudentId, ct))
            errors.Add($"SchoolStudentId ({schoolStudentId}) not found");

        if (!await db.InterestActivities.AnyAsync(i => i.Id == interestActivitieId, ct))
            errors.Add($"InterestActivitieId ({interestActivitieId}) not found");

        return errors.Count > 0
            ? Result<bool>.Failure(errors)
            : Result<bool>.Success(true);
    }

    // ─────────────────────────────── Filters ───────────────────────────────

    private static IQueryable<StudentInterest> ApplyFilters(IQueryable<StudentInterest> query, StudentInterestFilter filter)
    {
        if (filter.SchoolStudentId.HasValue)
            query = query.Where(x => x.IdSchoolStudent == filter.SchoolStudentId.Value);

        if (filter.InterestActivityId.HasValue)
            query = query.Where(x => x.IdInterestActivity == filter.InterestActivityId.Value);

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }

    // ─────────────────────────────── Mapping ───────────────────────────────

    private static StudentInterestReadDto MapToReadDto(StudentInterest interest) => new()
    {
        Id = interest.Id,
        MomentSelected = (WebApiNibu.Data.Dto.Person.MomentSelected?)interest.MomentSelected,
        Moment = interest.Moment,
        SchoolStudentId = interest.IdSchoolStudent,
        InterestActivitieId = interest.IdInterestActivity
    };

    private static StudentInterest MapFromCreateDto(StudentInterestCreateDto dto) => new()
    {
        MomentSelected = dto.MomentSelected.HasValue
            ? (WebApiNibu.Data.Enum.MomentSelected)dto.MomentSelected.Value
            : WebApiNibu.Data.Enum.MomentSelected.App,
        Moment = dto.Moment,
        IdSchoolStudent = dto.SchoolStudentId,
        IdInterestActivity = dto.InterestActivitieId,
        Active = true,
        SchoolStudent = null!,
        InterestActivity = null!
    };

    private static void ApplyUpdateDto(StudentInterest target, StudentInterestUpdateDto dto)
    {
        target.MomentSelected = dto.MomentSelected.HasValue
            ? (WebApiNibu.Data.Enum.MomentSelected)dto.MomentSelected.Value
            : target.MomentSelected;
        target.Moment = dto.Moment;
        target.IdSchoolStudent = dto.SchoolStudentId;
        target.IdInterestActivity = dto.InterestActivitieId;
    }
}

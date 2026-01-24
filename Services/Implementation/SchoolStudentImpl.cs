using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Data.Entity.Person;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract;

namespace WebApiNibu.Services.Implementation;

public class SchoolStudentImpl(IBaseCrud<SchoolStudent> baseCrud, OracleDbContext db) : ISchoolStudent
{
    // ─────────────────────────────── Queries ───────────────────────────────

    public async Task<Result<PagedResult<SchoolStudentReadDto>>> GetAllAsync(SchoolStudentFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.SchoolStudents.AsQueryable();
        query = ApplyFilters(query, filter);
        var totalCount = await query.CountAsync(ct);
        var items = await query.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync(ct);
        return Result<PagedResult<SchoolStudentReadDto>>.Success(new PagedResult<SchoolStudentReadDto>
        {
            Items = items.Select(MapToReadDto), PageNumber = pagination.PageNumber, PageSize = pagination.PageSize, TotalCount = totalCount
        });
    }

    public async Task<Result<SchoolStudentReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await baseCrud.GetByIdAsync(id, true, ct);
        return item is null
            ? Result<SchoolStudentReadDto>.Failure($"SchoolStudent with id {id} not found")
            : Result<SchoolStudentReadDto>.Success(MapToReadDto(item));
    }

    // ─────────────────────────────── Commands ───────────────────────────────

    public async Task<Result<SchoolStudentReadDto>> CreateAsync(SchoolStudentCreateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.IdCountry, dto.IdDocumentType, dto.IdSchool, ct);
        if (!validationResult.IsSuccess)
            return Result<SchoolStudentReadDto>.Failure(validationResult.Errors);

        var entity = MapFromCreateDto(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<SchoolStudentReadDto>.Success(MapToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, SchoolStudentUpdateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.IdCountry, dto.IdDocumentType, dto.IdSchool, ct);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors);

        var updated = await baseCrud.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"SchoolStudent with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"SchoolStudent with id {id} not found");
    }

    // ─────────────────────────────── Validation ───────────────────────────────

    private async Task<Result<bool>> ValidateForeignKeysAsync(int idCountry, int idDocumentType, int idSchool, CancellationToken ct)
    {
        var errors = new List<string>();

        if (!await db.Countries.AnyAsync(c => c.Id == idCountry, ct))
            errors.Add($"IdCountry ({idCountry}) not found");

        if (!await db.DocumentTypes.AnyAsync(d => d.Id == idDocumentType, ct))
            errors.Add($"IdDocumentType ({idDocumentType}) not found");

        if (!await db.Schools.AnyAsync(s => s.Id == idSchool, ct))
            errors.Add($"IdSchool ({idSchool}) not found");

        return errors.Count > 0
            ? Result<bool>.Failure(errors)
            : Result<bool>.Success(true);
    }

    // ─────────────────────────────── Filters ───────────────────────────────

    private static IQueryable<SchoolStudent> ApplyFilters(IQueryable<SchoolStudent> query, SchoolStudentFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.FirstName))
            query = query.Where(x => x.FirstName.Contains(filter.FirstName));

        if (!string.IsNullOrWhiteSpace(filter.PaternalSurname))
            query = query.Where(x => x.PaternalSurname.Contains(filter.PaternalSurname));

        if (!string.IsNullOrWhiteSpace(filter.Email))
            query = query.Where(x => x.Email.Contains(filter.Email));

        if (filter.IdCountry.HasValue)
            query = query.Where(x => x.IdCountry == filter.IdCountry.Value);

        if (filter.IdDocumentType.HasValue)
            query = query.Where(x => x.IdDocumentType == filter.IdDocumentType.Value);

        if (filter.IdSchool.HasValue)
            query = query.Where(x => x.IdSchool == filter.IdSchool.Value);

        if (filter.IsPlayer.HasValue)
            query = query.Where(x => x.IsPlayer == filter.IsPlayer.Value);

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }

    // ─────────────────────────────── Mapping ───────────────────────────────

    private static SchoolStudentReadDto MapToReadDto(SchoolStudent s) => new()
    {
        Id = s.Id,
        FirstName = s.FirstName,
        MiddleName = s.MiddleName,
        PaternalSurname = s.PaternalSurname,
        MaternalSurname = s.MaternalSurname,
        DocumentNumber = s.DocumentNumber,
        BirthDate = s.BirthDate,
        PhoneNumber = s.PhoneNumber,
        Email = s.Email,
        IdCountry = s.IdCountry,
        IdDocumentType = s.IdDocumentType,
        IdSchool = s.IdSchool,
        SchoolGrade = s.SchoolGrade,
        IsPlayer = s.IsPlayer,
        HasUpsaParents = s.HasUpsaParents
    };

    private static SchoolStudent MapFromCreateDto(SchoolStudentCreateDto dto) => new()
    {
        // Person
        FirstName = dto.FirstName,
        MiddleName = dto.MiddleName,
        PaternalSurname = dto.PaternalSurname,
        MaternalSurname = dto.MaternalSurname,
        DocumentNumber = dto.DocumentNumber,
        BirthDate = dto.BirthDate,
        PhoneNumber = dto.PhoneNumber,
        Email = dto.Email,
        IdCountry = dto.IdCountry,
        IdDocumentType = dto.IdDocumentType,
        // Student
        IdSchool = dto.IdSchool,
        SchoolGrade = dto.SchoolGrade,
        IsPlayer = dto.IsPlayer,
        HasUpsaParents = dto.HasUpsaParents,
        Active = true
    };

    private static void ApplyUpdateDto(SchoolStudent target, SchoolStudentUpdateDto dto)
    {
        // Person
        target.FirstName = dto.FirstName;
        target.MiddleName = dto.MiddleName;
        target.PaternalSurname = dto.PaternalSurname;
        target.MaternalSurname = dto.MaternalSurname;
        target.DocumentNumber = dto.DocumentNumber;
        target.BirthDate = dto.BirthDate;
        target.PhoneNumber = dto.PhoneNumber;
        target.Email = dto.Email;
        target.IdCountry = dto.IdCountry;
        target.IdDocumentType = dto.IdDocumentType;
        // Student
        target.IdSchool = dto.IdSchool;
        target.SchoolGrade = dto.SchoolGrade;
        target.IsPlayer = dto.IsPlayer;
        target.HasUpsaParents = dto.HasUpsaParents;
    }
}

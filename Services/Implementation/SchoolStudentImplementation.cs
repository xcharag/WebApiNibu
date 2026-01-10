using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto;
using WebApiNibu.Data.Entity.Person;
using WebApiNibu.Services.Interface;
using WebApiNibu.Services.Interface.Commands;
using WebApiNibu.Services.Interface.Common;
using WebApiNibu.Services.Interface.Queries;

namespace WebApiNibu.Services.Implementation;

public class SchoolStudentImplementation(IBaseCrud<SchoolStudent> crud, OracleDbContext db) : ISchoolStudent
{
    public async Task<IReadOnlyList<SchoolStudentReadDto>> QueryAsync(SchoolStudentQuery query, CancellationToken ct = default)
    {
        var q = db.SchoolStudents.AsNoTracking().AsQueryable();

        if (query.IdSchool.HasValue) q = q.Where(s => s.IdSchool == query.IdSchool.Value);
        if (query.Grade.HasValue) q = q.Where(s => s.SchoolGrade == query.Grade.Value);
        if (query.IsPlayer.HasValue) q = q.Where(s => s.IsPlayer == query.IsPlayer.Value);
        if (query.HasUpsaParents.HasValue) q = q.Where(s => s.HasUpsaParents == query.HasUpsaParents.Value);
        if (query.IdCountry.HasValue) q = q.Where(s => s.IdCountry == query.IdCountry.Value);
        if (query.IdDocumentType.HasValue) q = q.Where(s => s.IdDocumentType == query.IdDocumentType.Value);

        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            var name = query.Name;
            q = q.Where(s =>
                s.FirstName.Contains(name) ||
                (s.MiddleName != null && s.MiddleName.Contains(name)) ||
                s.PaternalSurname.Contains(name) ||
                s.MaternalSurname.Contains(name));
        }

        if (query.Active.HasValue) q = q.Where(s => s.Active == query.Active.Value);

        var items = await q.ToListAsync(ct);
        return items.Select(MapToReadDto).ToList();
    }

    public async Task<SchoolStudentReadDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var item = await crud.GetByIdAsync(id, true, ct);
        return item is null ? null : MapToReadDto(item);
    }

    public async Task<Result<SchoolStudentReadDto>> CreateAsync(CreateSchoolStudentCommand command, CancellationToken ct = default)
    {
        var dto = command.ToDto();
        var fkErrors = await ValidateFksAsync(dto.IdCountry, dto.IdDocumentType, dto.IdSchool, ct);
        if (fkErrors.Count > 0)
        {
            return Result<SchoolStudentReadDto>.Fail("Invalid foreign keys", fkErrors);
        }

        var entity = MapFromCreateDto(dto);
        var created = await crud.CreateAsync(entity, ct);
        return Result<SchoolStudentReadDto>.Ok(MapToReadDto(created));
    }

    public async Task<Result> UpdateAsync(int id, UpdateSchoolStudentCommand command, CancellationToken ct = default)
    {
        var dto = command.ToDto();
        var fkErrors = await ValidateFksAsync(dto.IdCountry, dto.IdDocumentType, dto.IdSchool, ct);
        if (fkErrors.Count > 0)
        {
            return Result.Fail("Invalid foreign keys", fkErrors);
        }

        var updated = await crud.UpdateAsync(id, e => ApplyUpdateDto(e, dto), ct);
        return updated ? Result.Ok() : Result.Fail("Not found", new[] { $"SchoolStudent (Id={id}) not found" });
    }

    public async Task<Result> DeleteAsync(int id, bool softDelete = true, CancellationToken ct = default)
    {
        var deleted = await crud.DeleteAsync(id, softDelete, ct);
        return deleted ? Result.Ok() : Result.Fail("Not found", new[] { $"SchoolStudent (Id={id}) not found" });
    }

    private async Task<List<string>> ValidateFksAsync(int idCountry, int idDocumentType, int idSchool, CancellationToken ct)
    {
        var errors = new List<string>();
        if (!await db.Countries.AnyAsync(c => c.Id == idCountry, ct)) errors.Add($"IdCountry ({idCountry}) not found");
        if (!await db.DocumentTypes.AnyAsync(d => d.Id == idDocumentType, ct)) errors.Add($"IdDocumentType ({idDocumentType}) not found");
        if (!await db.Schools.AnyAsync(s => s.Id == idSchool, ct)) errors.Add($"IdSchool ({idSchool}) not found");
        return errors;
    }

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
        IdSchool = dto.IdSchool,
        SchoolGrade = dto.SchoolGrade,
        IsPlayer = dto.IsPlayer,
        HasUpsaParents = dto.HasUpsaParents,
        Active = true
    };

    private static void ApplyUpdateDto(SchoolStudent target, SchoolStudentUpdateDto dto)
    {
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
        target.IdSchool = dto.IdSchool;
        target.SchoolGrade = dto.SchoolGrade;
        target.IsPlayer = dto.IsPlayer;
        target.HasUpsaParents = dto.HasUpsaParents;
    }
}
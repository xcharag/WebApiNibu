using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.School;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.School.SchoolsContacts;

public class SchoolsContactsCommands(IBaseCrud<Data.Entity.School.Contact> baseCrud, CoreDbContext db)
{
    public async Task<Result<ContactReadDto>> CreateAsync(ContactCreateDto dto, CancellationToken ct)
    {
        var school = await db.Schools.FirstOrDefaultAsync(s => s.Id == dto.School.Id && s.Active, ct);
        if (school is null)
            return Result<ContactReadDto>.Failure($"School with id {dto.School.Id} not found");

        var entity = SchoolsContactsMapper.ToEntity(dto, school);
        var created = await baseCrud.CreateAsync(entity, ct);

        // Reload with includes to return full DTO
        var loaded = await db.Contacts
            .Include(c => c.SchoolTable)
            .FirstOrDefaultAsync(c => c.Id == created.Id, ct);

        return Result<ContactReadDto>.Success(SchoolsContactsMapper.ToReadDto(loaded!));
    }

    public async Task<Result<bool>> UpdateAsync(int id, ContactUpdateDto dto, CancellationToken ct)
    {
        var updated = await baseCrud.UpdateAsync(id, e => SchoolsContactsMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Contact with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Contact with id {id} not found");
    }
}
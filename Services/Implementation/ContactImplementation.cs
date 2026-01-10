using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto;
using WebApiNibu.Data.Entity.School;
using WebApiNibu.Services.Interface;
using WebApiNibu.Services.Interface.Commands;
using WebApiNibu.Services.Interface.Common;
using WebApiNibu.Services.Interface.Queries;

namespace WebApiNibu.Services.Implementation;

public class ContactImplementation(IBaseCrud<Contact> crud, OracleDbContext db) : IContact
{
    public async Task<IReadOnlyList<ContactReadDto>> QueryAsync(ContactQuery query, CancellationToken ct = default)
    {
        var q = db.Contacts.AsNoTracking().AsQueryable();

        if (query.SchoolId.HasValue) q = q.Where(c => c.SchoolTable.Id == query.SchoolId.Value);
        if (!string.IsNullOrWhiteSpace(query.Name)) q = q.Where(c => c.PersonName.Contains(query.Name));
        if (!string.IsNullOrWhiteSpace(query.Role)) q = q.Where(c => c.PersonRole.Contains(query.Role));
        if (!string.IsNullOrWhiteSpace(query.Email)) q = q.Where(c => c.PersonEmail != null && c.PersonEmail.Contains(query.Email));
        if (!string.IsNullOrWhiteSpace(query.Phone)) q = q.Where(c => c.PersonPhoneNumber != null && c.PersonPhoneNumber.Contains(query.Phone));
        if (query.Active.HasValue) q = q.Where(c => c.Active == query.Active.Value);

        var items = await q.ToListAsync(ct);
        return items.Select(MapToReadDto).ToList();
    }

    public async Task<ContactReadDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var item = await crud.GetByIdAsync(id, true, ct);
        return item is null ? null : MapToReadDto(item);
    }

    public async Task<ContactReadDto> CreateAsync(CreateContactCommand command, CancellationToken ct = default)
    {
        if (!await db.Schools.AnyAsync(s => s.Id == command.SchoolId, ct))
        {
            throw new DomainValidationException("Invalid foreign keys", new[] { $"SchoolId ({command.SchoolId}) not found" });
        }

        var schoolProxy = new SchoolTable { Id = command.SchoolId, Active = true };
        db.Attach(schoolProxy);

        var entity = new Contact
        {
            PersonName = command.PersonName,
            PersonRole = command.PersonRole,
            PersonPhoneNumber = command.PersonPhoneNumber,
            PersonEmail = command.PersonEmail,
            SchoolTable = schoolProxy,
            Active = true
        };

        var created = await crud.CreateAsync(entity, ct);
        return MapToReadDto(created);
    }

    public Task<bool> UpdateAsync(int id, UpdateContactCommand command, CancellationToken ct = default)
        => crud.UpdateAsync(id, entity =>
        {
            entity.PersonName = command.PersonName;
            entity.PersonRole = command.PersonRole;
            entity.PersonPhoneNumber = command.PersonPhoneNumber;
            entity.PersonEmail = command.PersonEmail;
        }, ct);

    public Task<bool> DeleteAsync(int id, bool softDelete = true, CancellationToken ct = default)
        => crud.DeleteAsync(id, softDelete, ct);

    private static ContactReadDto MapToReadDto(Contact c) => new()
    {
        Id = c.Id,
        PersonName = c.PersonName,
        PersonRole = c.PersonRole,
        PersonPhoneNumber = c.PersonPhoneNumber,
        PersonEmail = c.PersonEmail
    };
}
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.School;
using WebApiNibu.Data.Dto.School.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.School;
using WebApiNibu.Services.Implementation.School.SchoolsContacts;

namespace WebApiNibu.Services.Implementation.School;

public class SchoolsContactsImpl(IBaseCrud<Data.Entity.School.Contact> baseCrud, CoreDbContext db)
    : ISchoolsContacts
{
    private readonly SchoolsContactsQueries _queries = new(db);
    private readonly SchoolsContactsCommands _commands = new(baseCrud, db);

    public Task<Result<PagedResult<ContactReadDto>>> GetAllAsync(ContactFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<ContactReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<ContactReadDto>> CreateAsync(ContactCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, ContactUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}
using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.School;
using WebApiNibu.Data.Dto.School.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.School.SchoolsContacts;

public class SchoolsContactsQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<ContactReadDto>>> GetAllAsync(
        ContactFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.Contacts
            .Include(c => c.SchoolTable)
            .AsQueryable();

        query = SchoolsContactsFilterHandler.Apply(query, filter);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<ContactReadDto>>.Success(new PagedResult<ContactReadDto>
        {
            Items = items.Select(SchoolsContactsMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<ContactReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.Contacts
            .Include(c => c.SchoolTable)
            .FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);

        return item is null
            ? Result<ContactReadDto>.Failure($"Contact with id {id} not found")
            : Result<ContactReadDto>.Success(SchoolsContactsMapper.ToReadDto(item));
    }
}
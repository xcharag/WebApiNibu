using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.School;
using WebApiNibu.Data.Dto.School.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.School.Schools;

public class SchoolQueries(CoreDbContext db)
{
    public async Task<Result<PagedResult<SchoolReadDto>>> GetAllAsync(
        SchoolFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.Schools
            .Include(s => s.SchoolStudents)
            .Include(s => s.Contacts)
            .AsQueryable();

        query = SchoolFilterHandler.Apply(query, filter);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return Result<PagedResult<SchoolReadDto>>.Success(new PagedResult<SchoolReadDto>
        {
            Items = items.Select(SchoolMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<SchoolReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.Schools
            .Include(s => s.SchoolStudents)
            .Include(s => s.Contacts)
            .FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);

        return item is null
            ? Result<SchoolReadDto>.Failure($"School with id {id} not found")
            : Result<SchoolReadDto>.Success(SchoolMapper.ToReadDto(item));
    }
}
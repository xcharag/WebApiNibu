using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.SchoolStudent;

public class SchoolStudentQueries(OracleDbContext db)
{
    public async Task<Result<PagedResult<SchoolStudentReadDto>>> GetAllAsync(
        SchoolStudentFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.SchoolStudents.AsQueryable();
        query = SchoolStudentFilterHandler.Apply(query, filter);
        var totalCount = await query.CountAsync(ct);
        var items = await query.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync(ct);

        return Result<PagedResult<SchoolStudentReadDto>>.Success(new PagedResult<SchoolStudentReadDto>
        {
            Items = items.Select(SchoolStudentMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<SchoolStudentReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.SchoolStudents.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<SchoolStudentReadDto>.Failure($"SchoolStudent with id {id} not found")
            : Result<SchoolStudentReadDto>.Success(SchoolStudentMapper.ToReadDto(item));
    }
}

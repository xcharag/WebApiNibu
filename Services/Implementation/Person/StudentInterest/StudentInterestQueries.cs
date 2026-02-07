using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.StudentInterest;

public class StudentInterestQueries(OracleDbContext db)
{
    public async Task<Result<PagedResult<StudentInterestReadDto>>> GetAllAsync(
        StudentInterestFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.StudentInterests.AsQueryable();
        query = StudentInterestFilterHandler.Apply(query, filter);
        var totalCount = await query.CountAsync(ct);
        var items = await query.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync(ct);

        return Result<PagedResult<StudentInterestReadDto>>.Success(new PagedResult<StudentInterestReadDto>
        {
            Items = items.Select(StudentInterestMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<StudentInterestReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.StudentInterests.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<StudentInterestReadDto>.Failure($"StudentInterest with id {id} not found")
            : Result<StudentInterestReadDto>.Success(StudentInterestMapper.ToReadDto(item));
    }
}

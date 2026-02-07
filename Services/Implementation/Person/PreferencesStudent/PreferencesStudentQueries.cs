using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.PreferencesStudent;

public class PreferencesStudentQueries(OracleDbContext db)
{
    public async Task<Result<PagedResult<PreferencesStudentReadDto>>> GetAllAsync(
        PreferencesStudentFilter filter, PaginationParams pagination, CancellationToken ct)
    {
        var query = db.PreferencesStudents.AsQueryable();
        query = PreferencesStudentFilterHandler.Apply(query, filter);
        var totalCount = await query.CountAsync(ct);
        var items = await query.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync(ct);

        return Result<PagedResult<PreferencesStudentReadDto>>.Success(new PagedResult<PreferencesStudentReadDto>
        {
            Items = items.Select(PreferencesStudentMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        });
    }

    public async Task<Result<PreferencesStudentReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.PreferencesStudents.FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
        return item is null
            ? Result<PreferencesStudentReadDto>.Failure($"PreferencesStudent with id {id} not found")
            : Result<PreferencesStudentReadDto>.Success(PreferencesStudentMapper.ToReadDto(item));
    }
}

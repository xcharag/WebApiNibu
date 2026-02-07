using Microsoft.EntityFrameworkCore;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.AcademicPreference;

public class AcademicPreferenceQueries(OracleDbContext db)
{
    public async Task<Result<PagedResult<AcademicPreferenceReadDto>>> GetAllAsync(
        AcademicPreferenceFilter filter, 
        PaginationParams pagination, 
        CancellationToken ct)
    {
        var query = db.AcademicPreferences.AsQueryable();
        
        // Apply filters
        query = AcademicPreferenceFilterHandler.Apply(query, filter);
        
        // Get total count before pagination
        var totalCount = await query.CountAsync(ct);
        
        // Apply pagination
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        var result = new PagedResult<AcademicPreferenceReadDto>
        {
            Items = items.Select(AcademicPreferenceMapper.ToReadDto),
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        };

        return Result<PagedResult<AcademicPreferenceReadDto>>.Success(result);
    }

    public async Task<Result<AcademicPreferenceReadDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await db.AcademicPreferences
            .FirstOrDefaultAsync(x => x.Id == id && x.Active, ct);
            
        return item is null
            ? Result<AcademicPreferenceReadDto>.Failure($"AcademicPreference with id {id} not found")
            : Result<AcademicPreferenceReadDto>.Success(AcademicPreferenceMapper.ToReadDto(item));
    }
}

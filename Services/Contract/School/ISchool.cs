using WebApiNibu.Data.Dto.School;
using WebApiNibu.Data.Dto.School.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract.School;

public interface ISchool
{
    //Queries
    Task<Result<PagedResult<SchoolReadDto>>> GetAllAsync(SchoolFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<SchoolReadDto>> GetByIdAsync(int id, CancellationToken ct);
    
    //Commands
    Task<Result<SchoolReadDto>> CreateAsync(SchoolCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, SchoolUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}
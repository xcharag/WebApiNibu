using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract;

public interface ISchoolStudent
{
    // Queries
    Task<Result<PagedResult<SchoolStudentReadDto>>> GetAllAsync(SchoolStudentFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<SchoolStudentReadDto>> GetByIdAsync(int id, CancellationToken ct);

    // Commands
    Task<Result<SchoolStudentReadDto>> CreateAsync(SchoolStudentCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, SchoolStudentUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}

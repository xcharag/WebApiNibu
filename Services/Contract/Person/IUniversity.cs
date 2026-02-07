using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract.Person;

public interface IUniversity
{
    // Queries
    Task<Result<PagedResult<UniversityReadDto>>> GetAllAsync(UniversityFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<UniversityReadDto>> GetByIdAsync(int id, CancellationToken ct);

    // Commands
    Task<Result<UniversityReadDto>> CreateAsync(UniversityCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, UniversityUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}

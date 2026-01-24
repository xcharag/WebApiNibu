using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract.Person;

public interface IAdultType
{
    // Queries
    Task<Result<PagedResult<AdultTypeReadDto>>> GetAllAsync(AdultTypeFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<AdultTypeReadDto>> GetByIdAsync(int id, CancellationToken ct);

    // Commands
    Task<Result<AdultTypeReadDto>> CreateAsync(AdultTypeCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, AdultTypeUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}
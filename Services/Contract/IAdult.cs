using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract;

public interface IAdult
{
    // Queries
    Task<Result<PagedResult<AdultReadDto>>> GetAllAsync(AdultFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<AdultReadDto>> GetByIdAsync(int id, CancellationToken ct);

    // Commands
    Task<Result<AdultReadDto>> CreateAsync(AdultCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, AdultUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}
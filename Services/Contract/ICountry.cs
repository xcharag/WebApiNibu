using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract;

public interface ICountry
{
    // Queries
    Task<Result<PagedResult<CountryReadDto>>> GetAllAsync(CountryFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<CountryReadDto>> GetByIdAsync(int id, CancellationToken ct);

    // Commands
    Task<Result<CountryReadDto>> CreateAsync(CountryCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, CountryUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}
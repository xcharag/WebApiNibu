using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract;

public interface ICountry
{
    // Queries
    Task<Result<IEnumerable<CountryReadDto>>> GetAllAsync(CancellationToken ct);
    Task<Result<CountryReadDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<IEnumerable<CountryReadDto>>> GetFilteredAsync(CountryFilter filter, CancellationToken ct);

    // Commands
    Task<Result<CountryReadDto>> CreateAsync(CountryCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, CountryUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}
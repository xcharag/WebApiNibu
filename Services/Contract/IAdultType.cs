using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract;

public interface IAdultType
{
    // Queries
    Task<Result<IEnumerable<AdultTypeReadDto>>> GetAllAsync(CancellationToken ct);
    Task<Result<AdultTypeReadDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<IEnumerable<AdultTypeReadDto>>> GetFilteredAsync(AdultTypeFilter filter, CancellationToken ct);

    // Commands
    Task<Result<AdultTypeReadDto>> CreateAsync(AdultTypeCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, AdultTypeUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}
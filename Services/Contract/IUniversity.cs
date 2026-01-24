using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract;

public interface IUniversity
{
    // Queries
    Task<Result<IEnumerable<UniversityReadDto>>> GetAllAsync(CancellationToken ct);
    Task<Result<UniversityReadDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<IEnumerable<UniversityReadDto>>> GetFilteredAsync(UniversityFilter filter, CancellationToken ct);

    // Commands
    Task<Result<UniversityReadDto>> CreateAsync(UniversityCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, UniversityUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}

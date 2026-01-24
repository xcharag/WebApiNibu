using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract;

public interface IPreferencesStudent
{
    // Queries
    Task<Result<PagedResult<PreferencesStudentReadDto>>> GetAllAsync(PreferencesStudentFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<PreferencesStudentReadDto>> GetByIdAsync(int id, CancellationToken ct);

    // Commands
    Task<Result<PreferencesStudentReadDto>> CreateAsync(PreferencesStudentCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, PreferencesStudentUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}

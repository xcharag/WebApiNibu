using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract.Person;

public interface IAcademicPreference
{
    // Queries
    Task<Result<PagedResult<AcademicPreferenceReadDto>>> GetAllAsync(AcademicPreferenceFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<AcademicPreferenceReadDto>> GetByIdAsync(int id, CancellationToken ct);

    // Commands
    Task<Result<AcademicPreferenceReadDto>> CreateAsync(AcademicPreferenceCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, AcademicPreferenceUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}
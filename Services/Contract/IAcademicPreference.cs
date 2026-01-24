using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract;

public interface IAcademicPreference
{
    // Queries
    Task<Result<IEnumerable<AcademicPreferenceReadDto>>> GetAllAsync(CancellationToken ct);
    Task<Result<AcademicPreferenceReadDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<IEnumerable<AcademicPreferenceReadDto>>> GetFilteredAsync(AcademicPreferenceFilter filter, CancellationToken ct);

    // Commands
    Task<Result<AcademicPreferenceReadDto>> CreateAsync(AcademicPreferenceCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, AcademicPreferenceUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}
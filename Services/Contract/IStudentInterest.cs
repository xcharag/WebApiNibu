using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract;

public interface IStudentInterest
{
    // Queries
    Task<Result<PagedResult<StudentInterestReadDto>>> GetAllAsync(StudentInterestFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<StudentInterestReadDto>> GetByIdAsync(int id, CancellationToken ct);

    // Commands
    Task<Result<StudentInterestReadDto>> CreateAsync(StudentInterestCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, StudentInterestUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}

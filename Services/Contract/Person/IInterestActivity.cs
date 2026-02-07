using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract.Person;

public interface IInterestActivity
{
    // Queries
    Task<Result<PagedResult<InterestActivitieReadDto>>> GetAllAsync(InterestActivityFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<InterestActivitieReadDto>> GetByIdAsync(int id, CancellationToken ct);

    // Commands
    Task<Result<InterestActivitieReadDto>> CreateAsync(InterestActivitieCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, InterestActivitieUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}

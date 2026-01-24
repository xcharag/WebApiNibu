using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract.Person;

public interface IRole
{
    // Queries
    Task<Result<PagedResult<RoleReadDto>>> GetAllAsync(RoleFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<RoleReadDto>> GetByIdAsync(int id, CancellationToken ct);

    // Commands
    Task<Result<RoleReadDto>> CreateAsync(RoleCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, RoleUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}

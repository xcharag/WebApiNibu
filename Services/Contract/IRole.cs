using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract;

public interface IRole
{
    // Queries
    Task<Result<IEnumerable<RoleReadDto>>> GetAllAsync(CancellationToken ct);
    Task<Result<RoleReadDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<IEnumerable<RoleReadDto>>> GetFilteredAsync(RoleFilter filter, CancellationToken ct);

    // Commands
    Task<Result<RoleReadDto>> CreateAsync(RoleCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, RoleUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}

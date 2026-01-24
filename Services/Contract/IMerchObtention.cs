using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract;

public interface IMerchObtention
{
    // Queries
    Task<Result<PagedResult<MerchObtentionReadDto>>> GetAllAsync(MerchObtentionFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<MerchObtentionReadDto>> GetByIdAsync(int id, CancellationToken ct);

    // Commands
    Task<Result<MerchObtentionReadDto>> CreateAsync(MerchObtentionCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, MerchObtentionCreateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}

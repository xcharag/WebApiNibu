using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract;

public interface IMerchObtention
{
    // Queries
    Task<Result<IEnumerable<MerchObtentionReadDto>>> GetAllAsync(CancellationToken ct);
    Task<Result<MerchObtentionReadDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<IEnumerable<MerchObtentionReadDto>>> GetFilteredAsync(MerchObtentionFilter filter, CancellationToken ct);

    // Commands
    Task<Result<MerchObtentionReadDto>> CreateAsync(MerchObtentionCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, MerchObtentionCreateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}

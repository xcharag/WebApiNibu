using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract.Person;

public interface ICarreer
{
    // Queries
    Task<Result<PagedResult<CarreerReadDto>>> GetAllAsync(CarreerFilter filter, PaginationParams pagination, CancellationToken ct);
    Task<Result<CarreerReadDto>> GetByIdAsync(int id, CancellationToken ct);

    // Commands
    Task<Result<CarreerReadDto>> CreateAsync(CarreerCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, CarreerUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}
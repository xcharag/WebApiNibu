using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract;

public interface ICarreer
{
    // Queries
    Task<Result<IEnumerable<CarreerReadDto>>> GetAllAsync(CancellationToken ct);
    Task<Result<CarreerReadDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<IEnumerable<CarreerReadDto>>> GetFilteredAsync(CarreerFilter filter, CancellationToken ct);

    // Commands
    Task<Result<CarreerReadDto>> CreateAsync(CarreerCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, CarreerUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}
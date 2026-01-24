using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.Person;
using WebApiNibu.Services.Implementation.Person.Carreer;

namespace WebApiNibu.Services.Implementation.Person;

public class CarreerImpl(IBaseCrud<Data.Entity.Person.Carreer> baseCrud, OracleDbContext db)
    : ICarreer
{
    private readonly CarreerQueries _queries = new(db);
    private readonly CarreerCommands _commands = new(baseCrud);

    // Queries
    public Task<Result<PagedResult<CarreerReadDto>>> GetAllAsync(CarreerFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<CarreerReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    // Commands
    public Task<Result<CarreerReadDto>> CreateAsync(CarreerCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, CarreerUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}
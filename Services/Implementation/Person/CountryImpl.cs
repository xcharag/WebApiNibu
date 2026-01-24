using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.Person;
using WebApiNibu.Services.Implementation.Person.Country;

namespace WebApiNibu.Services.Implementation.Person;

public class CountryImpl(IBaseCrud<Data.Entity.Person.Country> baseCrud, OracleDbContext db)
    : ICountry
{
    private readonly CountryQueries _queries = new(db);
    private readonly CountryCommands _commands = new(baseCrud);

    // Queries
    public Task<Result<PagedResult<CountryReadDto>>> GetAllAsync(CountryFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<CountryReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    // Commands
    public Task<Result<CountryReadDto>> CreateAsync(CountryCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, CountryUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}
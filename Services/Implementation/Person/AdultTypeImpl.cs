using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.Person;
using WebApiNibu.Services.Implementation.Person.AdultType;

namespace WebApiNibu.Services.Implementation.Person;

public class AdultTypeImpl : IAdultType
{
    private readonly AdultTypeQueries _queries;
    private readonly AdultTypeCommands _commands;

    public AdultTypeImpl(IBaseCrud<Data.Entity.Person.AdultType> baseCrud, OracleDbContext db)
    {
        _queries = new AdultTypeQueries(db);
        _commands = new AdultTypeCommands(baseCrud);
    }

    // Queries
    public Task<Result<PagedResult<AdultTypeReadDto>>> GetAllAsync(AdultTypeFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<AdultTypeReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    // Commands
    public Task<Result<AdultTypeReadDto>> CreateAsync(AdultTypeCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, AdultTypeUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}
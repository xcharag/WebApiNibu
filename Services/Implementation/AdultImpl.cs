using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract;
using WebApiNibu.Services.Implementation.Adult;

namespace WebApiNibu.Services.Implementation;

public class AdultImpl : IAdult
{
    private readonly AdultQueries _queries;
    private readonly AdultCommands _commands;

    public AdultImpl(IBaseCrud<Data.Entity.Person.Adult> baseCrud, OracleDbContext db)
    {
        _queries = new AdultQueries(db);
        _commands = new AdultCommands(baseCrud, db);
    }

    // Queries
    public Task<Result<PagedResult<AdultReadDto>>> GetAllAsync(AdultFilter filter, PaginationParams pagination, CancellationToken ct) 
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<AdultReadDto>> GetByIdAsync(int id, CancellationToken ct) 
        => _queries.GetByIdAsync(id, ct);

    // Commands
    public Task<Result<AdultReadDto>> CreateAsync(AdultCreateDto dto, CancellationToken ct) 
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, AdultUpdateDto dto, CancellationToken ct) 
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct) 
        => _commands.DeleteAsync(id, soft, ct);
}
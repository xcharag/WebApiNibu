using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Feed.Polls;
using WebApiNibu.Data.Dto.Feed.Polls.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.Feed.Polls;
using WebApiNibu.Services.Implementation.Feed.Polls.Poll;

namespace WebApiNibu.Services.Implementation.Feed.Polls;

public class PollImpl(IBaseCrud<Data.Entity.Feed.Polls.Poll> baseCrud, OracleDbContext db)
    : IPoll
{
    private readonly PollQueries _queries = new(db);
    private readonly PollCommands _commands = new(baseCrud, db);

    public Task<Result<PagedResult<PollReadDto>>> GetAllAsync(PollFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<PollReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<PollReadDto>> CreateAsync(PollCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, PollUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}

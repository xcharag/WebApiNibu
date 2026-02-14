using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Feed.News;
using WebApiNibu.Data.Dto.Feed.News.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.Feed.News;
using WebApiNibu.Services.Implementation.Feed.News.NewsReaction;

namespace WebApiNibu.Services.Implementation.Feed.News;

public class NewsReactionImpl(IBaseCrud<Data.Entity.Feed.News.NewsReaction> baseCrud, OracleDbContext db)
    : INewsReaction
{
    private readonly NewsReactionQueries _queries = new(db);
    private readonly NewsReactionCommands _commands = new(baseCrud, db);

    public Task<Result<PagedResult<NewsReactionReadDto>>> GetAllAsync(NewsReactionFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<NewsReactionReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<NewsReactionReadDto>> CreateAsync(NewsReactionCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, NewsReactionUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}

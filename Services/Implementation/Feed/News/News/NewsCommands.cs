using WebApiNibu.Abstraction;
using WebApiNibu.Data.Dto.Feed.News;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Feed.News.News;

public class NewsCommands(IBaseCrud<Data.Entity.Feed.News.News> baseCrud)
{
    public async Task<Result<NewsReadDto>> CreateAsync(NewsCreateDto dto, CancellationToken ct)
    {
        var entity = NewsMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<NewsReadDto>.Success(NewsMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, NewsUpdateDto dto, CancellationToken ct)
    {
        var updated = await baseCrud.UpdateAsync(id, e => NewsMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"News with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"News with id {id} not found");
    }
}

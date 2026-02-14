using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Feed.News;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Feed.News.NewsDetail;

public class NewsDetailCommands(IBaseCrud<Data.Entity.Feed.News.NewsDetail> baseCrud, OracleDbContext db)
{
    public async Task<Result<NewsDetailReadDto>> CreateAsync(NewsDetailCreateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.NewsId, ct);
        if (!validationResult.IsSuccess)
            return Result<NewsDetailReadDto>.Failure(validationResult.Errors);

        var entity = NewsDetailMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<NewsDetailReadDto>.Success(NewsDetailMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, NewsDetailUpdateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.NewsId, ct);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors);

        var updated = await baseCrud.UpdateAsync(id, e => NewsDetailMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"NewsDetail with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"NewsDetail with id {id} not found");
    }

    private async Task<Result<bool>> ValidateForeignKeysAsync(int newsId, CancellationToken ct)
    {
        var errors = new List<string>();

        if (!await db.News.AnyAsync(n => n.Id == newsId, ct))
            errors.Add($"NewsId ({newsId}) not found");

        return errors.Count > 0
            ? Result<bool>.Failure(errors)
            : Result<bool>.Success(true);
    }
}

using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Feed.Events;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Feed.Events.EventDetail;

public class EventDetailCommands(IBaseCrud<Data.Entity.Feed.Events.EventDetail> baseCrud, OracleDbContext db)
{
    public async Task<Result<EventDetailReadDto>> CreateAsync(EventDetailCreateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.EventId, ct);
        if (!validationResult.IsSuccess)
            return Result<EventDetailReadDto>.Failure(validationResult.Errors);

        var entity = EventDetailMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<EventDetailReadDto>.Success(EventDetailMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, EventDetailUpdateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.EventId, ct);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors);

        var updated = await baseCrud.UpdateAsync(id, e => EventDetailMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"EventDetail with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"EventDetail with id {id} not found");
    }

    private async Task<Result<bool>> ValidateForeignKeysAsync(int eventId, CancellationToken ct)
    {
        var errors = new List<string>();

        if (!await db.Events.AnyAsync(e => e.Id == eventId, ct))
            errors.Add($"EventId ({eventId}) not found");

        return errors.Count > 0
            ? Result<bool>.Failure(errors)
            : Result<bool>.Success(true);
    }
}

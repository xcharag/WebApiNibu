using Microsoft.EntityFrameworkCore;
using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Feed.Events;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Feed.Events.EventInteraction;

public class EventInteractionCommands(IBaseCrud<Data.Entity.Feed.Events.EventInteraction> baseCrud, CoreDbContext db)
{
    public async Task<Result<EventInteractionReadDto>> CreateAsync(EventInteractionCreateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.EventId, dto.UserId, dto.QrAccessId, dto.MerchId, ct);
        if (!validationResult.IsSuccess)
            return Result<EventInteractionReadDto>.Failure(validationResult.Errors);

        var entity = EventInteractionMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<EventInteractionReadDto>.Success(EventInteractionMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, EventInteractionUpdateDto dto, CancellationToken ct)
    {
        var validationResult = await ValidateForeignKeysAsync(dto.EventId, dto.UserId, dto.QrAccessId, dto.MerchId, ct);
        if (!validationResult.IsSuccess)
            return Result<bool>.Failure(validationResult.Errors);

        var updated = await baseCrud.UpdateAsync(id, e => EventInteractionMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"EventInteraction with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"EventInteraction with id {id} not found");
    }

    private async Task<Result<bool>> ValidateForeignKeysAsync(
        int eventId,
        int userId,
        int qrAccessId,
        int merchId,
        CancellationToken ct)
    {
        var errors = new List<string>();

        if (!await db.Events.AnyAsync(e => e.Id == eventId, ct))
            errors.Add($"EventId ({eventId}) not found");

        if (!await db.Users.AnyAsync(u => u.Id == userId, ct))
            errors.Add($"UserId ({userId}) not found");

        if (qrAccessId > 0 && !await db.QrAccesses.AnyAsync(q => q.Id == qrAccessId, ct))
            errors.Add($"QrAccessId ({qrAccessId}) not found");

        if (merchId > 0 && !await db.Merchs.AnyAsync(m => m.Id == merchId, ct))
            errors.Add($"MerchId ({merchId}) not found");

        return errors.Count > 0
            ? Result<bool>.Failure(errors)
            : Result<bool>.Success(true);
    }
}

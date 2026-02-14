using WebApiNibu.Abstraction;
using WebApiNibu.Data.Dto.Feed.Events;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Feed.Events.Event;

public class EventCommands(IBaseCrud<Data.Entity.Feed.Events.Event> baseCrud)
{
    public async Task<Result<EventReadDto>> CreateAsync(EventCreateDto dto, CancellationToken ct)
    {
        var entity = EventMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<EventReadDto>.Success(EventMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, EventUpdateDto dto, CancellationToken ct)
    {
        var updated = await baseCrud.UpdateAsync(id, e => EventMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Event with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Event with id {id} not found");
    }
}

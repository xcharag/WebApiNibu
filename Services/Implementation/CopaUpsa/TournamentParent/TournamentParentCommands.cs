using WebApiNibu.Abstraction;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.CopaUpsa.TournamentParent;

public class TournamentParentCommands(IBaseCrud<Data.Entity.CopaUpsa.TournamentParent> baseCrud)
{
    public async Task<Result<TournamentParentReadDto>> CreateAsync(TournamentParentCreateDto dto, CancellationToken ct)
    {
        var entity = TournamentParentMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<TournamentParentReadDto>.Success(TournamentParentMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, TournamentParentUpdateDto dto, CancellationToken ct)
    {
        var updated = await baseCrud.UpdateAsync(id, e => TournamentParentMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"TournamentParent with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"TournamentParent with id {id} not found");
    }
}


using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.CopaUpsa;
using WebApiNibu.Data.Dto.CopaUpsa.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.CopaUpsa;
using WebApiNibu.Services.Implementation.CopaUpsa.Participation;

namespace WebApiNibu.Services.Implementation.CopaUpsa;

public class ParticipationImpl(IBaseCrud<Data.Entity.CopaUpsa.Participation> baseCrud, CoreDbContext db)
    : IParticipation
{
    private readonly ParticipationQueries _queries = new(db);
    private readonly ParticipationCommands _commands = new(baseCrud, db);

    public Task<Result<PagedResult<ParticipationReadDto>>> GetAllAsync(ParticipationFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<ParticipationReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<ParticipationReadDto>> CreateAsync(ParticipationCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, ParticipationUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}


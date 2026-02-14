using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.Person;
using WebApiNibu.Services.Implementation.Person.AcademicPreference;

namespace WebApiNibu.Services.Implementation.Person;

public class AcademicPreferenceImpl(IBaseCrud<Data.Entity.Person.AcademicPreference> baseCrud, CoreDbContext db)
    : IAcademicPreference
{
    private readonly AcademicPreferenceQueries _queries = new(db);
    private readonly AcademicPreferenceCommands _commands = new(baseCrud, db);

    // ─────────────────────────────── Queries ───────────────────────────────

    public Task<Result<PagedResult<AcademicPreferenceReadDto>>> GetAllAsync(
        AcademicPreferenceFilter filter,
        PaginationParams pagination,
        CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<AcademicPreferenceReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    // ─────────────────────────────── Commands ───────────────────────────────

    public Task<Result<AcademicPreferenceReadDto>> CreateAsync(AcademicPreferenceCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, AcademicPreferenceUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}
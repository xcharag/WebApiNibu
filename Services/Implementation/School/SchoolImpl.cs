using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.School;
using WebApiNibu.Data.Dto.School.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.School;
using WebApiNibu.Services.Implementation.School.Schools;

namespace WebApiNibu.Services.Implementation.School;

public class SchoolImpl(IBaseCrud<Data.Entity.School.SchoolTable> baseCrud, CoreDbContext db)
    : ISchool
{
    private readonly SchoolQueries _queries = new(db);
    private readonly SchoolCommands _commands = new(baseCrud);

    public Task<Result<PagedResult<SchoolReadDto>>> GetAllAsync(SchoolFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<SchoolReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    public Task<Result<SchoolReadDto>> CreateAsync(SchoolCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, SchoolUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}
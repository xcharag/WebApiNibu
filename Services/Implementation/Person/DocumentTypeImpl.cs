using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.Person;
using WebApiNibu.Services.Implementation.Person.DocumentType;

namespace WebApiNibu.Services.Implementation.Person;

public class DocumentTypeImpl(IBaseCrud<Data.Entity.Person.DocumentType> baseCrud, CoreDbContext db)
    : IDocumentType
{
    private readonly DocumentTypeQueries _queries = new(db);
    private readonly DocumentTypeCommands _commands = new(baseCrud);

    // Queries
    public Task<Result<PagedResult<DocumentTypeReadDto>>> GetAllAsync(DocumentTypeFilter filter, PaginationParams pagination, CancellationToken ct)
        => _queries.GetAllAsync(filter, pagination, ct);

    public Task<Result<DocumentTypeReadDto>> GetByIdAsync(int id, CancellationToken ct)
        => _queries.GetByIdAsync(id, ct);

    // Commands
    public Task<Result<DocumentTypeReadDto>> CreateAsync(DocumentTypeCreateDto dto, CancellationToken ct)
        => _commands.CreateAsync(dto, ct);

    public Task<Result<bool>> UpdateAsync(int id, DocumentTypeUpdateDto dto, CancellationToken ct)
        => _commands.UpdateAsync(id, dto, ct);

    public Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
        => _commands.DeleteAsync(id, soft, ct);
}

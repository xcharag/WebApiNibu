using WebApiNibu.Abstraction;
using WebApiNibu.Data.Context.Oracle;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;
using WebApiNibu.Services.Contract.Person;
using WebApiNibu.Services.Implementation.Person.DocumentType;

namespace WebApiNibu.Services.Implementation.Person;

public class DocumentTypeImpl : IDocumentType
{
    private readonly DocumentTypeQueries _queries;
    private readonly DocumentTypeCommands _commands;

    public DocumentTypeImpl(IBaseCrud<Data.Entity.Person.DocumentType> baseCrud, OracleDbContext db)
    {
        _queries = new DocumentTypeQueries(db);
        _commands = new DocumentTypeCommands(baseCrud);
    }

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

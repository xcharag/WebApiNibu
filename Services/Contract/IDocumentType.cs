using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Data.Dto.Person.Filters;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Contract;

public interface IDocumentType
{
    // Queries
    Task<Result<IEnumerable<DocumentTypeReadDto>>> GetAllAsync(CancellationToken ct);
    Task<Result<DocumentTypeReadDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<IEnumerable<DocumentTypeReadDto>>> GetFilteredAsync(DocumentTypeFilter filter, CancellationToken ct);

    // Commands
    Task<Result<DocumentTypeReadDto>> CreateAsync(DocumentTypeCreateDto dto, CancellationToken ct);
    Task<Result<bool>> UpdateAsync(int id, DocumentTypeUpdateDto dto, CancellationToken ct);
    Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct);
}

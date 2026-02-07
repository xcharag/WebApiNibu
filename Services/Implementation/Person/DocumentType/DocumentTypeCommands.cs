using WebApiNibu.Abstraction;
using WebApiNibu.Data.Dto.Person;
using WebApiNibu.Helpers;

namespace WebApiNibu.Services.Implementation.Person.DocumentType;

public class DocumentTypeCommands(IBaseCrud<Data.Entity.Person.DocumentType> baseCrud)
{
    public async Task<Result<DocumentTypeReadDto>> CreateAsync(DocumentTypeCreateDto dto, CancellationToken ct)
    {
        var entity = DocumentTypeMapper.ToEntity(dto);
        var created = await baseCrud.CreateAsync(entity, ct);
        return Result<DocumentTypeReadDto>.Success(DocumentTypeMapper.ToReadDto(created));
    }

    public async Task<Result<bool>> UpdateAsync(int id, DocumentTypeUpdateDto dto, CancellationToken ct)
    {
        var updated = await baseCrud.UpdateAsync(id, e => DocumentTypeMapper.ApplyUpdate(e, dto), ct);
        return updated
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"DocumentType with id {id} not found");
    }

    public async Task<Result<bool>> DeleteAsync(int id, bool soft, CancellationToken ct)
    {
        var deleted = await baseCrud.DeleteAsync(id, soft, ct);
        return deleted
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"DocumentType with id {id} not found");
    }
}

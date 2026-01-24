using WebApiNibu.Data.Dto.Person.Filters;

namespace WebApiNibu.Services.Implementation.Person.DocumentType;

public static class DocumentTypeFilterHandler
{
    public static IQueryable<Data.Entity.Person.DocumentType> Apply(
        IQueryable<Data.Entity.Person.DocumentType> query, 
        DocumentTypeFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}

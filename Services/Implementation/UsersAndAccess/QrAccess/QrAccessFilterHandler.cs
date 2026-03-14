using WebApiNibu.Data.Dto.UsersAndAccess.Filters;

namespace WebApiNibu.Services.Implementation.UsersAndAccess.QrAccess;

public static class QrAccessFilterHandler
{
    public static IQueryable<Data.Entity.UsersAndAccess.QrAccess> Apply(
        IQueryable<Data.Entity.UsersAndAccess.QrAccess> query, QrAccessFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Reason))
            query = query.Where(x => x.Reason != null && x.Reason.Contains(filter.Reason));

        if (!string.IsNullOrWhiteSpace(filter.FirstName))
            query = query.Where(x => x.FirstName.Contains(filter.FirstName));

        if (!string.IsNullOrWhiteSpace(filter.LastName))
            query = query.Where(x => x.LastName.Contains(filter.LastName));

        if (!string.IsNullOrWhiteSpace(filter.DocumentNumber))
            query = query.Where(x => x.DocumentNumber.Contains(filter.DocumentNumber));

        if (!string.IsNullOrWhiteSpace(filter.PhoneNumber))
            query = query.Where(x => x.PhoneNumber.Contains(filter.PhoneNumber));

        if (!string.IsNullOrWhiteSpace(filter.Relationship))
            query = query.Where(x => x.Relationship.Contains(filter.Relationship));

        if (!string.IsNullOrWhiteSpace(filter.Value))
            query = query.Where(x => x.Value.Contains(filter.Value));

        if (filter.IsUsed.HasValue)
            query = query.Where(x => x.IsUsed == filter.IsUsed.Value);

        if (filter.WasUpsaStudent.HasValue)
            query = query.Where(x => x.WasUpsaStudent == filter.WasUpsaStudent.Value);

        if (filter.ExpirationFrom.HasValue)
            query = query.Where(x => x.ExpirationDate >= filter.ExpirationFrom.Value);

        if (filter.ExpirationTo.HasValue)
            query = query.Where(x => x.ExpirationDate <= filter.ExpirationTo.Value);

        if (filter.IsExpired.HasValue)
        {
            var now = DateTime.UtcNow;
            query = filter.IsExpired.Value
                ? query.Where(x => x.ExpirationDate < now)
                : query.Where(x => x.ExpirationDate >= now);
        }

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}

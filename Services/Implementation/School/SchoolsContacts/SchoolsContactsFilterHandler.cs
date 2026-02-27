using WebApiNibu.Data.Dto.School.Filters;

namespace WebApiNibu.Services.Implementation.School.SchoolsContacts;

public static class SchoolsContactsFilterHandler
{
    public static IQueryable<Data.Entity.School.Contact> Apply(
        IQueryable<Data.Entity.School.Contact> query, ContactFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.PersonName))
            query = query.Where(x => x.PersonName.Contains(filter.PersonName));

        if (!string.IsNullOrWhiteSpace(filter.PersonRole))
            query = query.Where(x => x.PersonRole.Contains(filter.PersonRole));

        if (!string.IsNullOrWhiteSpace(filter.PersonPhoneNumber))
            query = query.Where(x => x.PersonPhoneNumber != null && x.PersonPhoneNumber.Contains(filter.PersonPhoneNumber));

        if (!string.IsNullOrWhiteSpace(filter.PersonEmail))
            query = query.Where(x => x.PersonEmail != null && x.PersonEmail.Contains(filter.PersonEmail));

        if (filter.SchoolsId is not null && filter.SchoolsId.Count > 0)
            query = query.Where(x => filter.SchoolsId.Contains(x.SchoolTable.Id));

        return query;
    }
}
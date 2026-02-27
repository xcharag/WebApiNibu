using WebApiNibu.Data.Dto.School.Filters;

namespace WebApiNibu.Services.Implementation.School.Schools;

public static class SchoolFilterHandler
{
    public static IQueryable<Data.Entity.School.SchoolTable> Apply(
        IQueryable<Data.Entity.School.SchoolTable> query, SchoolFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));

        if (!string.IsNullOrWhiteSpace(filter.Tier))
            query = query.Where(x => x.Tier == filter.Tier);

        if (!string.IsNullOrWhiteSpace(filter.Address))
            query = query.Where(x => x.Address.Contains(filter.Address));

        if (!string.IsNullOrWhiteSpace(filter.City))
            query = query.Where(x => x.City.Contains(filter.City));

        if (!string.IsNullOrWhiteSpace(filter.Country))
            query = query.Where(x => x.Country.Contains(filter.Country));

        if (filter.MinQuantityStudents.HasValue)
            query = query.Where(x => x.SchoolStudents.Count >= filter.MinQuantityStudents.Value);

        if (filter.MaxQuantityStudents.HasValue)
            query = query.Where(x => x.SchoolStudents.Count <= filter.MaxQuantityStudents.Value);

        return query;
    }
}
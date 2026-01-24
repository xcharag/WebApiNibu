using WebApiNibu.Data.Dto.Person.Filters;

namespace WebApiNibu.Services.Implementation.Person.MerchObtention;

public static class MerchObtentionFilterHandler
{
    public static IQueryable<Data.Entity.Person.MerchObtention> Apply(
        IQueryable<Data.Entity.Person.MerchObtention> query, 
        MerchObtentionFilter filter)
    {
        if (filter.SchoolStudentId.HasValue)
            query = query.Where(x => x.IdSchoolStudent == filter.SchoolStudentId.Value);

        if (filter.MerchId.HasValue)
            query = query.Where(x => x.IdMerch == filter.MerchId.Value);

        if (filter.Claimed.HasValue)
            query = query.Where(x => x.Claimed == filter.Claimed.Value);

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}

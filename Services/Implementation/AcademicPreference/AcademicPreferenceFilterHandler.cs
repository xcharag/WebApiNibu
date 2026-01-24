using WebApiNibu.Data.Dto.Person.Filters;

namespace WebApiNibu.Services.Implementation.AcademicPreference;

public static class AcademicPreferenceFilterHandler
{
    public static IQueryable<Data.Entity.Person.AcademicPreference> Apply(
        IQueryable<Data.Entity.Person.AcademicPreference> query, 
        AcademicPreferenceFilter filter)
    {
        if (filter.UniversityId.HasValue)
            query = query.Where(x => x.IdUniversitiy == filter.UniversityId.Value);

        if (filter.CarreerId.HasValue)
            query = query.Where(x => x.IdCarreer == filter.CarreerId.Value);

        if (filter.PreferencesStudentId.HasValue)
            query = query.Where(x => x.IdPreferencesStudent == filter.PreferencesStudentId.Value);

        if (filter.Active.HasValue)
            query = query.Where(x => x.Active == filter.Active.Value);

        return query;
    }
}

using WebApiNibu.Data.Dto;

namespace WebApiNibu.Services.Interface.Commands;

public sealed record CreateSchoolTableCommand(
    string Name,
    string Tier,
    string Address,
    string? SportLogo,
    string? NormalLogo,
    int Rue,
    string Delegada,
    string Tipo,
    string Ciudad,
    int IdDepartamento,
    int Ka,
    int IdDelegada,
    int IdColegio,
    int KaRectorada,
    string? Segemento,
    List<ContactCreateDto> Contacts);

public sealed record UpdateSchoolTableCommand(
    string Name,
    string Tier,
    string Address,
    string? SportLogo,
    string? NormalLogo,
    int Rue,
    string Delegada,
    string Tipo,
    string Ciudad,
    int IdDepartamento,
    int Ka,
    int IdDelegada,
    int IdColegio,
    int KaRectorada,
    string? Segemento);

public static class SchoolTableCommandMappings
{
    public static SchoolTableCreateDto ToDto(this CreateSchoolTableCommand cmd) => new()
    {
        Name = cmd.Name,
        Tier = cmd.Tier,
        Address = cmd.Address,
        SportLogo = cmd.SportLogo,
        NormalLogo = cmd.NormalLogo,
        Rue = cmd.Rue,
        Delegada = cmd.Delegada,
        Tipo = cmd.Tipo,
        Ciudad = cmd.Ciudad,
        IdDepartamento = cmd.IdDepartamento,
        Ka = cmd.Ka,
        IdDelegada = cmd.IdDelegada,
        IdColegio = cmd.IdColegio,
        KaRectorada = cmd.KaRectorada,
        Segemento = cmd.Segemento,
        Contacts = cmd.Contacts ?? new List<ContactCreateDto>()
    };

    public static SchoolTableUpdateDto ToDto(this UpdateSchoolTableCommand cmd) => new()
    {
        Name = cmd.Name,
        Tier = cmd.Tier,
        Address = cmd.Address,
        SportLogo = cmd.SportLogo,
        NormalLogo = cmd.NormalLogo,
        Rue = cmd.Rue,
        Delegada = cmd.Delegada,
        Tipo = cmd.Tipo,
        Ciudad = cmd.Ciudad,
        IdDepartamento = cmd.IdDepartamento,
        Ka = cmd.Ka,
        IdDelegada = cmd.IdDelegada,
        IdColegio = cmd.IdColegio,
        KaRectorada = cmd.KaRectorada,
        Segemento = cmd.Segemento
    };
}


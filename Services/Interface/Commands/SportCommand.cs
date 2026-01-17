namespace WebApiNibu.Services.Interface.Commands;

using WebApiNibu.Data.Dto;

public sealed record CreateSportCommand(
    string Name,
    string? Description,
    string Icon);

public sealed record UpdateSportCommand(
    string Name,
    string? Description,
    string Icon);

public static class SportCommandMappings
{
    public static SportDto ToDto(this CreateSportCommand cmd) => new()
    {
        Name = cmd.Name,
        Description = cmd.Description,
        Icon = cmd.Icon
    };

    public static SportDto ToDto(this UpdateSportCommand cmd) => new()
    {
        Name = cmd.Name,
        Description = cmd.Description,
        Icon = cmd.Icon
    };
}
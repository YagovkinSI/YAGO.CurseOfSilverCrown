namespace YAGO.World.Host.Controllers.Colonies.Models
{
    public record ColonyParameterResponse(
        string Type,
        string? ParrentType,
        int Weight,
        string Name,
        string Value,
        string? Url = null);
}

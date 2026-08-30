using YAGO.World.Host.Controllers.Statistics;

namespace YAGO.World.Host.Controllers.Colonies.ColonyParameters
{
    public record ColonyParameterResponse(
        string Type,
        string Name,
        string Value,
        string? Url = null)
    {
        public string Status { get; set; } = ParameterStatusConstants.Neutral;
    }
}

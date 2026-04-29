using YAGO.World.Domain.Entities.GameEvents.Dataset;
using YAGO.World.Host.Controllers.Common;

namespace YAGO.World.Host.Controllers.Colonies.ColonyParameters
{
    public record ColonyParameterResponse(
        string Type,
        string? ParrentType,
        int Weight,
        string Name,
        string Value,
        string? Url = null)
    {
        public static ColonyParameterResponse ColonyName(string colonyName) =>
            new(ColonyParameterNames.Colony_Name, ParrentType: null, Weight: 0, "Колония", colonyName, Url: null);

        public static ColonyParameterResponse Finance(double resources, double trend) =>
            new(ColonyParameterNames.Economic, ParrentType: null, Weight: 2, "Финансы",
                  $"{resources.ToBeautifulString()} ({trend.ToBeautifulString(setPlus: true)}/н)",
                  Url: null);

        public static ColonyParameterResponse Trust(double resources, double trend) =>
            new(ColonyParameterNames.Mood_Total, ParrentType: null, Weight: 3, "Доверие",
                  $"{(resources < GameEventsConstants.TrustWithRevolt ? "🔥 " : "")}" +
                  $"{resources.ToBeautifulString()} ({trend.ToBeautifulString(setPlus: true)}/н)",
                  Url: null);

        public static ColonyParameterResponse TrustResourse(double value, bool isChange) =>
            new(ColonyParameterNames.Mood_Total, ParrentType: ColonyParameterNames.Mood_Total, Weight: 31, "Доверие",
                  $"{value.ToBeautifulString(isChange)}",
                  Url: null);
    }
}

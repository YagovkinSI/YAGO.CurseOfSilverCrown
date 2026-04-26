using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Host.Controllers.Common;

namespace YAGO.World.Host.Controllers.Colonies.ColonyParameters
{
    public record ColonyParameterResponse(
        string Type,
        string? ParrentType,
        int Weight,
        string Name,
        string Value,
        string? Url = null);

    public record ColonyParameterNameResponse : ColonyParameterResponse
    {
        public ColonyParameterNameResponse(string Value)
            : base(ColonyParameterNames.Colony_Name, ParrentType: null, Weight: 0, "Колония", Value, Url: null) 
        { }
    }

    public record ColonyParameterFinanceResponse : ColonyParameterResponse
    {
        public ColonyParameterFinanceResponse(double resources, double trend)
            : base(ColonyParameterNames.Economic, ParrentType: null, Weight: 20, "Финансы",
                  $"{resources.ToBeautifulString()} ({trend.ToBeautifulString(setPlus: true)}/н)", 
                  Url: null)
        { }
    }
}

using Newtonsoft.Json;
using System.Collections.Generic;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.ValueTypes.States;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal static class ColonyEntityMapper
    {
        public static Colony ToDomain(this ColonyEntity source)
        {
            var colonyParameters = JsonConvert.DeserializeObject<ColonyParameters>(source.StatesJson)
                ?? throw new YagoException("Не удалось десериализовать параметры колонии из БД.");

            var colonyStats = GetColonyStats(source, colonyParameters);
            var colonyName = new ColonyName(source.Name, colonyParameters.Named);

            return new Colony(
                source.Id,
                source.UserId,
                colonyName,
                colonyStats,
                colonyParameters.EventIds,
                source.Deactivated,
                source.DeactivateAtUtc);
        }

        public static ColonyEntity ToEntity(this Colony source)
        {
            var colonyName = source.Name;
            var colonyStats = source.Stats;
            var colonyParameters = GetColonyParameters(
                colonyName.Named,
                colonyStats,
                source.EventIds);
            var statesJson = JsonConvert.SerializeObject(colonyParameters);
            return new ColonyEntity(
                source.Id,
                source.UserId,
                colonyName.DatabaseName,
                solars: (colonyStats.States[StateKeys.Solars.Reserve] as State)!.Value,
                statesJson,
                source.Deactivated,
                source.DeactivateAtUtc);
        }

        private static ColonyStats GetColonyStats(ColonyEntity source, ColonyParameters colonyParameter)
        {
            var colonyIndustryList = new ColonyIndustryList(
                colonyParameter.AdministrativeIndustry.ToAdministrativeIndustry(),
                colonyParameter.MinningIndustry.ToMinningIndustry(),
                colonyParameter.ProductionIndustry.ToProductionIndustry(),
                colonyParameter.ServiceIndustry.ToServiceIndustry());
            var states = new Dictionary<string, IState>()
            {
                { StateKeys.Solars.Reserve, new MutableState(StateKeys.Solars.Reserve, source.Solars) },
                { StateKeys.ReformPoints.Income, new MutableState(StateKeys.ReformPoints.Income, colonyParameter.ActionPointsTrend) },
                { StateKeys.Mood.Reserve, new MutableState(StateKeys.Mood.Reserve, colonyParameter.MoodTotal, minValue: 0, maxValue: 100) },
                { StateKeys.Counters.Turns, new MutableState(StateKeys.Counters.Turns, colonyParameter.CurrentWeek) },
                { StateKeys.Flags.Events.FirstWedding, new MutableState(StateKeys.Flags.Events.FirstWedding, colonyParameter.FirstWedding ? 1 : 0) },
                { StateKeys.ReformPoints.Reserve, new MutableState(StateKeys.ReformPoints.Reserve, colonyParameter.ActionPoints) },
                { StateKeys.Modules.Total, new MutableState(StateKeys.Modules.Total, colonyParameter.Zones) },
                { StateKeys.Reforms.TaxLevel, new MutableState(StateKeys.Reforms.TaxLevel, colonyParameter.TaxLevel) },
                { StateKeys.Reforms.SocialGuaranteesLevel, new MutableState(StateKeys.Reforms.SocialGuaranteesLevel, colonyParameter.SocialGuaranteesLevel) },
            };
            var colonyStats = new ColonyStats(
                colonyIndustryList,
                states);
            return colonyStats;
        }

        private static ColonyParameters GetColonyParameters(
            bool named,
            ColonyStats colonyStats,
            IReadOnlyList<string> eventIds)
        {
            var colonyIndustries = colonyStats.Industries;

            return new ColonyParameters(
                named,
                (int)colonyStats.GetGameParameter(StateKeys.ReformPoints.Reserve),
                (int)colonyStats.GetGameParameter(StateKeys.ReformPoints.Income),
                (int)colonyStats.GetGameParameter(StateKeys.Reforms.TaxLevel),
                (int)colonyStats.GetGameParameter(StateKeys.Reforms.SocialGuaranteesLevel),
                colonyStats.GetGameParameter(StateKeys.Mood.Reserve),
                colonyStats.GetGameParameter(StateKeys.Flags.Events.FirstWedding) == 1,
                (int)colonyStats.GetGameParameter(StateKeys.Counters.Turns),
                (int)colonyStats.GetGameParameter(StateKeys.Modules.Total),
                colonyIndustries.Administrative.ToEntity(),
                colonyIndustries.Minning.ToEntity(),
                colonyIndustries.Production.ToEntity(),
                colonyIndustries.Service.ToEntity(),
                eventIds);
        }
    }
}

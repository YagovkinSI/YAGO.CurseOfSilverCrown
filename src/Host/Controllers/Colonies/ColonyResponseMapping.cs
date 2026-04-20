using System.Collections.Generic;
using System.Linq;
using YAGO.World.Application.Common.Pagination;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.ValueTypes;
using YAGO.World.Host.Controllers.Colonies.Models;
using YAGO.World.Host.Controllers.Common;

namespace YAGO.World.Host.Controllers.Colonies
{
    public static class ColonyResponseMapping
    {
        public static ApiResponse<MyColony> ToApiResponse(
            this Colony? source)
        {
            if (source == null)
                return ApiResponse<MyColony>.CreateSuccess(data: null);

            var result = source.ToMyColony();

            return ApiResponse<MyColony>.CreateSuccess(data: result);
        }

        public static MyColony ToMyColony(
            this Colony source)
        {
            var colonyPatameters = source.ToColonyPatameters();
            var autoRunCycle = source.IsAutoRunCycle();
            var newColonyAvailable = source.IsNewColonyAvailable();
            var solars = source.Stats.Resources.Solars;
            var zoneAvailable = source.Stats.ZonesAvailable;

            return new MyColony(
                source.Id,
                source.UserId,
                source.Name,
                colonyPatameters,
                autoRunCycle,
                newColonyAvailable,
                solars,
                zoneAvailable);
        }

        public static PaginatedResponse<ColonyDetails> ToPaginatedResponse(
            this PaginatedData<Colony> source)
        {
            var data = source.Data
                .Select(x => x.ToDetails())
                .ToArray();

            return new PaginatedResponse<ColonyDetails>(
                data,
                source.Total,
                source.Page,
                source.Limit);
        }

        public static ColonyDetails ToDetails(this Colony source)
        {
            var colonyPatameters = source.ToColonyPatameters();

            return new ColonyDetails(
                source.Id,
                source.UserId,
                source.Name,
                colonyPatameters);
        }

        public static IReadOnlyList<ColonyParameterResponse> ToColonyPatameters(
            this Colony source)
        {
            var colonyPatameters = new List<ColonyParameterResponse>();

            var colonyStats = source.Stats;
            var episodeCount = colonyStats.EpisodeCount;
            var colonySettings = colonyStats.Settings;

            if (episodeCount > 0)
            {
                colonyPatameters.Add(GetColonyName(source.Name));
                colonyPatameters.Add(GetReserves(colonyStats));
                colonyPatameters.Add(GetStation(
                    colonySettings.GetShipName(), colonySettings.ShipId, inOther: episodeCount > 1));
                colonyPatameters.Add(GetEpisodeCount(episodeCount));
            }
            if (episodeCount > 1)
            {
                colonyPatameters.Add(GetMood(colonyStats.MoodTotal));
                colonyPatameters.Add(GetAttractiveness(colonyStats));
                colonyPatameters.Add(GetPopulation(colonyStats.PopulationTotal));
                colonyPatameters.Add(GetZones(colonyStats));
                colonyPatameters.Add(GetLaws(colonySettings.CodeOfLaws));
            }

            return colonyPatameters
                .OrderBy(x => x.Weight)
                .ToList();
        }

        private static ColonyParameterResponse GetColonyName(string colonyName)
        {
            return new ColonyParameterResponse(
                ColonyParameterNames.Colony_Name,
                ParrentType: null,
                Weight: 0,
                "Колония",
                colonyName);
        }

        private static ColonyParameterResponse GetReserves(ColonyStats colonyStats)
        {
            return new ColonyParameterResponse(
                ColonyParameterNames.Economic_Reserves,
                ParrentType: null,
                Weight: 20,
                "Резервы",
                $"{colonyStats.Resources.Solars.ToBeautifulString()} ({colonyStats.BudgetBalance.ToBeautifulString()}/н)");
        }

        private static ColonyParameterResponse GetStation(string shipName, long shipId, bool inOther)
        {
            return new ColonyParameterResponse(
                ColonyParameterNames.Ship_Id,
                ParrentType: inOther ? ColonyParameterNames.Other : null,
                Weight: 200,
                "Станция",
                shipName,
                Url: shipId.ToString());
        }

        private static ColonyParameterResponse GetEpisodeCount(int episodeCount)
        {
            return new ColonyParameterResponse(
                ColonyParameterNames.EpisodeCount,
                ParrentType: ColonyParameterNames.Other,
                Weight: 900,
                "Ход",
                episodeCount.ToString());
        }

        private static ColonyParameterResponse GetMood(LimitedDouble moodTotal)
        {
            var value = moodTotal.Value.ToBeautifulString();
            if (moodTotal.Value < 50)
                value += " (риск бунта)";
            return new ColonyParameterResponse(
                ColonyParameterNames.Mood_Total,
                ParrentType: null,
                Weight: 30,
                "Настроение",
                value);
        }

        private static ColonyParameterResponse GetAttractiveness(ColonyStats colonyStats)
        {
            return new ColonyParameterResponse(
                ColonyParameterNames.Attractiveness_Total,
                ParrentType: null,
                Weight: 60,
                "Привлекательность",
                colonyStats.AttractivenessTotalCalc().ToBeautifulString());
        }

        private static ColonyParameterResponse GetPopulation(int populationTotal)
        {
            return new ColonyParameterResponse(
                ColonyParameterNames.Population_Total,
                ParrentType: null,
                Weight: 150,
                "Население",
                populationTotal.ToString());
        }

        private static ColonyParameterResponse GetZones(ColonyStats sourceStats)
        {
            return new ColonyParameterResponse(
                ColonyParameterNames.AreaCapacity_Occupied,
                ParrentType: null,
                Weight: 50,
                "Площадь",
                $"{sourceStats.ZonesOccupied}/{sourceStats.Resources.ZonesTotal}");
        }

        private static ColonyParameterResponse GetLaws(CodeOfLaws codeOfLaws)
        {
            var value = codeOfLaws switch
            {
                CodeOfLaws.Capitalist => "Корпоративные",
                CodeOfLaws.Centrist => "Стандартные",
                CodeOfLaws.Humanist => "Гуманные",
                _ => "Не определены",
            };
            return new ColonyParameterResponse(
                ColonyParameterNames.Laws_CodeOfLaws,
                ParrentType: ColonyParameterNames.Colony_Name,
                Weight: 300,
                "Законы",
                value);
        }
    }
}

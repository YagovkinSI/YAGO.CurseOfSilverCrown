using System;
using System.Linq;
using YAGO.World.Application.Colonies;
using YAGO.World.Application.Common.Pagination;
using YAGO.World.Application.Statistics.Queries;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.GameParameters;
using YAGO.World.Host.Controllers.Colonies.ColonyParameters;
using YAGO.World.Host.Controllers.Common.Extensions;
using YAGO.World.Host.Controllers.Common.Models;
using YAGO.World.Host.Controllers.Events;

namespace YAGO.World.Host.Controllers.Colonies
{
    public static class ColonyResponseMapping
    {
        public static ApiResponse<T> ToApiResponse<T>(
            this T? source)
            where T : class
        {
            return source == null ? ApiResponse<T>.CreateSuccess(data: null) : ApiResponse<T>.CreateSuccess(data: source);
        }

        public static ColonyPrivate ToResponse(this ColonyPrivateDto source)
        {
            var colony = source.Colony;
            var colonyEvents = source.ColonyEvents;
            var nextTurnStartAtUtc = colony.State.TurnReserve.GetNextTurnStartAtUtc(DateTime.UtcNow);
            var colonyName = colony.DisplayInfo;
            var colonyPatameters = ColonyParameterResponseMapping.ToColonyParameters(colony);
            var events = colonyEvents.Select(x => x.ToResponse()).ToList();
            var modulesUsed = colony.GetValue(GameParameterType.ModulesUsed);
            var actions = new ColonyActionsResponse(
                Reform: modulesUsed > 0,
                Build: modulesUsed > 0);

            return new ColonyPrivate(
                colony.Id,
                colony.UserId,
                nextTurnStartAtUtc,
                colonyName.DisplayName,
                colonyPatameters,
                events,
                actions);
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
            var colonyName = source.DisplayInfo;
            var colonyPatameters = ColonyParameterResponseMapping.ToColonyParameters(source);

            return new ColonyDetails(
                source.Id,
                source.UserId,
                colonyName.DisplayName,
                colonyPatameters);
        }

        public static StatisticType ToStatisticType(this string statisticType)
        {
            return statisticType switch
            {
                "SolarsDelta" => StatisticType.SolarsDelta,
                _ => throw new NotImplementedException(),
            };
        }

        public static StatisticsResponse ToResponse(this GameParameterComposition parameterComposition)
        {
            var parameters = parameterComposition.Parameters
                .Select(x => x.ToResponse())
                .ToList();
            return new StatisticsResponse(
                parameterComposition.DisplayInfo.Name,
                parameters);
        }

        public static ColonyParameterResponse ToResponse(this GameParameter gameParameter)
        {
            var displayInfo = gameParameter.ParameterType.ToDisplayInfo();
            var isInteger = gameParameter.ParameterType.IsInteger();
            return new ColonyParameterResponse(
                gameParameter.ParameterType.ToResponse(),
                StatMenus: [],
                Weight: 0,
                displayInfo.Name,
                gameParameter.Value.ToBeautifulString(isInteger),
                Url: null);
        }

        public static string ToResponse(this GameParameterType gameParameterType)
        {
            return gameParameterType switch
            {
                GameParameterType.SolarsCurrent => ColonyParameterNames.Economic_Reserves,
                GameParameterType.SolarsDelta => ColonyParameterNames.Economic_Budget_Balance,
                GameParameterType.SolarDeltaIndustriesPrivate => ColonyParameterNames.Other,
                GameParameterType.SolarDeltaIndustriesState => ColonyParameterNames.Other,
                GameParameterType.PublicDebtService => ColonyParameterNames.Other,
                GameParameterType.AdministrationSalary => ColonyParameterNames.Other,
                GameParameterType.PopulationTaxSolars => ColonyParameterNames.Other,
                GameParameterType.ActionPointsCurrent => ColonyParameterNames.ActionPoints_Resourses,
                GameParameterType.ActionPointsDelta => ColonyParameterNames.ActionPoints_Trend,
                GameParameterType.ModulesTotal => ColonyParameterNames.Area_Total,
                GameParameterType.ModulesUsed => ColonyParameterNames.AreaCapacity_Occupied,
                GameParameterType.MoodCurrent => ColonyParameterNames.Mood_Total,
                GameParameterType.MoodDelta => ColonyParameterNames.Other,
                GameParameterType.MiningSlotsFree => ColonyParameterNames.Other,
                GameParameterType.TurnsCurrent => ColonyParameterNames.CurrentWeek,
                GameParameterType.Population => ColonyParameterNames.Population_Total,
                GameParameterType.ReformsTaxLevel => ColonyParameterNames.Other,
                GameParameterType.ReformsSocialGuaranteesLevel => ColonyParameterNames.Other,
            };
        }
    }
}
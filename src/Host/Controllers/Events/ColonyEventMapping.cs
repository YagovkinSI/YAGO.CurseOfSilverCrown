using System.Collections.Generic;
using System.Linq;
using YAGO.World.Application.Events;
using YAGO.World.Domain.GameActions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Host.Controllers.Colonies.ColonyParameters;
using YAGO.World.Application.Common.Extensions;
using YAGO.World.Host.Controllers.Episodes;
using YAGO.World.Host.Controllers.Events.Models;

namespace YAGO.World.Host.Controllers.Events
{
    public static class ColonyEventMapping
    {
        public static ColonyEventPrivate ToResponse(this ColonyEventPrivateDto source)
        {
            return new ColonyEventPrivate(
                source.ColonyEvent.Id,
                source.GameEvent.Slides[0].Title,
                source.GameEvent.Type.ToResponse(),
                source.ToEpisodeResponse(),
                source.ColonyEvent.IsRead,
                source.ColonyEvent.CreatedAtUtc);
        }

        public static ColonyEventSummary ToResponse(this ColonyEventSummaryDto source)
        {
            return new ColonyEventSummary(
                source.ColonyEvent.Id,
                source.GameEvent.Slides[0].Title,
                source.GameEvent.Type.ToResponse(),
                source.ColonyEvent.IsRead,
                source.ColonyEvent.CreatedAtUtc);
        }

        public static EventResultSlideResponse? ToResponse(this GameActionResult source)
        {
            var colonyPatameters = GetColonyParameterResponse(source);
            return new EventResultSlideResponse(
                source.Show,
                source.DisplayInfo.Name,
                source.DisplayInfo.ImageName,
                source.DisplayInfo.Description,
                colonyPatameters);
        }

        private static IReadOnlyList<ColonyParameterResponse> GetColonyParameterResponse(GameActionResult source)
        {
            var result = new List<ColonyParameterResponse?>()
            {
                GetSolarsCurrent(source.SolarsCurrent),
                GetSolarsDelta(source.SolarsDelta),
                GetMoodCurrent(source.MoodCurrent),
                GetModulesUsed(source.ModulesUsed),
                GetPopulation(source.Population)
            };
            return result
                .Where(x => x != null)
                .Select(x => x!)
                .ToList();
        }

        private static ColonyParameterResponse? GetSolarsCurrent(GameActionResultValue<double> solarsCurrent)
        {
            if (solarsCurrent.Delta == default)
                return null;
            return new ColonyParameterResponse(
                ColonyParameterNames.Economic_Reserves,
                "Солары",
                solarsCurrent.GetChangeString());
        }

        private static ColonyParameterResponse? GetSolarsDelta(GameActionResultValue<double> solarsDelta)
        {
            if (solarsDelta.Delta == default)
                return null;
            return new ColonyParameterResponse(
                ColonyParameterNames.Economic_Budget_Balance,
                "Солары за ход",
                solarsDelta.GetChangeString());
        }

        private static ColonyParameterResponse? GetMoodCurrent(GameActionResultValue<double> moodCurrent)
        {
            if (moodCurrent.Delta == default)
                return null;
            return new ColonyParameterResponse(
                ColonyParameterNames.AreaCapacity_Occupied,
                "Доверие",
                moodCurrent.GetChangeString());
        }

        private static ColonyParameterResponse? GetModulesUsed(GameActionResultValue<int> modulesUsed)
        {
            if (modulesUsed.Delta == default)
                return null;
            return new ColonyParameterResponse(
                ColonyParameterNames.AreaCapacity_Occupied,
                "Занято модулей",
                modulesUsed.GetChangeString());
        }

        private static ColonyParameterResponse? GetPopulation(GameActionResultValue<int> population)
        {
            if (population.Delta == default)
                return null;
            return new ColonyParameterResponse(
                ColonyParameterNames.Population_Total,
                "Население",
                population.GetChangeString());
        }

        private static string GetChangeString(this GameActionResultValue<int> value)
        {
            return $"{(value.Delta > 0 ? "+" : "")}{value.Delta.ToBeautifulString()} " +
                $"({value.Before.ToBeautifulString()} -> {value.After.ToBeautifulString()})";
        }

        private static string GetChangeString(this GameActionResultValue<double> value)
        {
            return $"{(value.Delta > 0 ? "+" : "")}{value.Delta.ToBeautifulString()} " +
                $"({value.Before.ToBeautifulString()} -> {value.After.ToBeautifulString()})";
        }

        private static string ToResponse(this EventType eventType)
        {
            return eventType switch
            {
                EventType.Default => EventTypeConstants.Default,
                EventType.Autostart => EventTypeConstants.Autostart,
                EventType.Urgent => EventTypeConstants.Urgent,
                EventType.Quest => EventTypeConstants.Quest,
                _ => EventTypeConstants.Default,
            };
        }
    }
}

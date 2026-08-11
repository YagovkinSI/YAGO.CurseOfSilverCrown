using System.Collections.Generic;
using System.Linq;
using YAGO.World.Application.Colonies;
using YAGO.World.Application.Common.Pagination;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Services;
using YAGO.World.Host.Controllers.Colonies.ColonyParameters;
using YAGO.World.Host.Controllers.Colonies.Models;
using YAGO.World.Host.Controllers.Common;
using YAGO.World.Host.Controllers.Episodes;
using YAGO.World.Host.Controllers.Events.Models;

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

        public static MyColony ToMyColony(
            this Colony source,
            IReadOnlyList<ColonyEventDto> colonyEvents)
        {
            var colonyName = source.Name;
            var colonyPatameters = ColonyParameterResponseMapping.ToColonyParameters(source);
            var newColonyAvailable = source.IsNewColonyAvailable();
            var solars = source.State.GetValue(StateKey.SolarsCurrent);
            var zoneAvailable = source.State.GetValue(StateKey.ModulesFree);
            var events = colonyEvents.Select(x => x.ToMyQuest()).ToList();

            return new MyColony(
                source.Id,
                source.UserId,
                colonyName.DisplayName,
                colonyPatameters,
                events,
                newColonyAvailable,
                solars,
                zoneAvailable);
        }

        public static ColonyEventResponse ToMyQuest(this ColonyEventDto source)
        {
            var gameEvent = source.GameEvent;
            var episode = gameEvent.Episode;

            return new ColonyEventResponse(
                gameEvent.Id,
                episode.Slides[0].Title,
                gameEvent.EventType.ToResponse(),
                source.ToEpisodeResponse(),
                source.ColonyEvent.IsRead,
                source.ColonyEvent.CreatedAtUtc);
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
            var colonyName = source.Name;
            var colonyPatameters = ColonyParameterResponseMapping.ToColonyParameters(source);

            return new ColonyDetails(
                source.Id,
                source.UserId,
                colonyName.DisplayName,
                colonyPatameters);
        }

        public static EventResultSlideResponse? ToResponse(this EventResult source)
        {
            var colonyPatameters = source.MainParametersResult.Select(MapToColonyPatameters).ToList();

            return new EventResultSlideResponse(
                source.Title,
                source.ImageName,
                source.Text,
                colonyPatameters);
        }

        public static ColonyParameterResponse MapToColonyPatameters(this KeyValuePair<StateKey, double[]> colonyStatChange)
        {
            return colonyStatChange.Key switch
            {
                StateKey.ModulesUsed => new ColonyParameterResponse(
                    ColonyParameterNames.AreaCapacity_Occupied,
                    StatMenus: [], Weight: 0,
                    "Занято зон",
                    GetChangeString(colonyStatChange)),
                StateKey.SolarsCurrent => new ColonyParameterResponse(
                    ColonyParameterNames.Economic_Reserves,
                    StatMenus: [], Weight: 0,
                    "Солары",
                    GetChangeString(colonyStatChange)),
                StateKey.SolarsDelta => new ColonyParameterResponse(
                    ColonyParameterNames.AreaCapacity_Occupied,
                    StatMenus: [], Weight: 0,
                    "Солары за ход",
                    GetChangeString(colonyStatChange)),
                StateKey.MoodCurrent => new ColonyParameterResponse(
                    ColonyParameterNames.AreaCapacity_Occupied,
                    StatMenus: [], Weight: 0,
                    "Доверие",
                    GetChangeString(colonyStatChange)),
                StateKey.Population => new ColonyParameterResponse(
                    ColonyParameterNames.Population_Total,
                    StatMenus: [], Weight: 0,
                    "Население",
                    GetChangeString(colonyStatChange))
            };
        }

        public static string GetChangeString(this KeyValuePair<StateKey, double[]> colonyStatChange)
        {
            if (colonyStatChange.Value.Length > 1)
            {
                var before = colonyStatChange.Value[0];
                var after = colonyStatChange.Value[1];
                var change = after - before;
                return $"{(change > 0 ? "+" : "")}{change.ToBeautifulString()} " +
                    $"({before.ToBeautifulString()} -> {after.ToBeautifulString()})";
            }
            else
            {
                var change = colonyStatChange.Value[0];
                return $"{(change > 0 ? "+" : "")}{change.ToBeautifulString()}";
            }
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
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Application.Common.Pagination;
using YAGO.World.Domain.Aggregates;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Host.Controllers.Colonies.ColonyParameters;
using YAGO.World.Host.Controllers.Colonies.Models;
using YAGO.World.Host.Controllers.Colonies.MyQuests;
using YAGO.World.Host.Controllers.Common;
using YAGO.World.Host.Controllers.Episodes;

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
            IReadOnlyList<ColonyEvent> colonyEvents)
        {
            var colonyName = source.Name;
            var colonyPatameters = ColonyParameterResponseMapping.ToColonyParameters(source);
            var newColonyAvailable = source.IsNewColonyAvailable();
            var solars = source.State.States[StateKey.SolarsCurrent];
            var zoneAvailable = source.State.GetZonesAvailable();
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

        public static MyQuest ToMyQuest(this ColonyEvent source)
        {
            var gameEvent = source.GameEvent;
            var colonyEpisode = source.GetPrologueColonyEpisode();
            var (questType, progress) = gameEvent.GetQuestTypeAndProgress(source.ColonyStats);

            return new MyQuest(
                gameEvent.Id,
                gameEvent.Episode.Slides[0].Title,
                progress,
                (QuestTypeResponse)questType,
                colonyEpisode.ToResponse());
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

        private static ColonyParameterResponse MapToColonyPatameters(KeyValuePair<StateKey, double[]> colonyStatChange)
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
                StateKey.MoodReserve => new ColonyParameterResponse(
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

        private static string GetChangeString(KeyValuePair<StateKey, double[]> colonyStatChange)
        {
            var before = colonyStatChange.Value[0];
            var after = colonyStatChange.Value[1];
            var change = after - before;
            return $"{before.ToBeautifulString()} -> {after.ToBeautifulString()} " +
                $"({(change > 0 ? "+" : "")}{change.ToBeautifulString()})";
        }
    }
}

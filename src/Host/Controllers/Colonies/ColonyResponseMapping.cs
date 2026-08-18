using System;
using System.Collections.Generic;
using System.Linq;
using YAGO.World.Application.Colonies;
using YAGO.World.Application.Common.Pagination;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.GameActions;
using YAGO.World.Host.Controllers.Colonies.ColonyParameters;
using YAGO.World.Host.Controllers.Common;
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

        public static ColonyPrivate ToMyColony(
            this Colony source,
            IReadOnlyList<ColonyEventDto> colonyEvents)
        {
            var nextTurnStartAtUtc = source.TurnReserve.GetNextTurnStartAtUtc(DateTime.UtcNow);
            var colonyName = source.Name;
            var colonyPatameters = ColonyParameterResponseMapping.ToColonyParameters(source);
            var events = colonyEvents.Select(x => x.ToMyQuest()).ToList();
            var modulesUsed = source.State.GetValue(GameParameterType.ModulesUsed);
            var actions = new ColonyActionsResponse(
                Reform: modulesUsed > 0,
                Build: modulesUsed > 0);

            return new ColonyPrivate(
                source.Id,
                source.UserId,
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
            var colonyName = source.Name;
            var colonyPatameters = ColonyParameterResponseMapping.ToColonyParameters(source);

            return new ColonyDetails(
                source.Id,
                source.UserId,
                colonyName.DisplayName,
                colonyPatameters);
        }
    }
}
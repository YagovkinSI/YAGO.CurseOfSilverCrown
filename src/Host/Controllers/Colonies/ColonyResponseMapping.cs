using System;
using System.Linq;
using YAGO.World.Application.Colonies;
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
            var events = colonyEvents.Select(x => x.ToResponse()).ToList();
            var modulesUsed = colony.State.Slots[Domain.Colonies.Slots.ColonySlotType.Modules].GetUsed(colony.State);
            var actions = new ColonyActionsResponse(
                Reform: modulesUsed > 0,
                Build: modulesUsed > 0,
                Statistics: modulesUsed > 0);

            return new ColonyPrivate(
                colony.Id,
                colony.UserId,
                nextTurnStartAtUtc,
                colonyName.DisplayName,
                events,
                actions);
        }
    }
}
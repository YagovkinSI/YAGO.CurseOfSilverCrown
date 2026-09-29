using System;
using System.Linq;
using YAGO.World.Application.Colonies;
using YAGO.World.Domain.Colonies;
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
            var events = colonyEvents.Select(x => x.ToResponse()).ToList();
            var actions = colony.State.Menus.ToResponse();
            var unreadWikiArticles = colony.State.UnlockedWikiArticles.Values
                .Count(x => !x.Value);

            return new ColonyPrivate(
                colony.Id,
                colony.UserId,
                nextTurnStartAtUtc,
                colony.DisplayName,
                events,
                actions,
                unreadWikiArticles);
        }

        private static ColonyActionsResponse ToResponse(this ColonyMenus menus)
        {
            return new ColonyActionsResponse(
                Reform: menus.IsAvailable(ColonyMenuType.Reform),
                Build: menus.IsAvailable(ColonyMenuType.Build),
                Statistics: menus.IsAvailable(ColonyMenuType.Statistics),
                Council: menus.IsAvailable(ColonyMenuType.Council),
                Wiki: menus.IsAvailable(ColonyMenuType.Wiki));
        }
    }
}

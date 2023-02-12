using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Enums;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Game.Map.Routes;
using YSI.CurseOfSilverCrown.Core.ViewModels;

namespace YSI.CurseOfSilverCrown.Core.Helpers
{
    public static class WarBaseHelper
    {
        public static async Task<IEnumerable<GameMapRoute>> GetAvailableTargets(
            ApplicationDbContext context,
            int domainId,
            Unit unit,
            enArmyCommandType commandType)
        {
            var domain = await context.Domains.FindAsync(domainId);
            var kingdomDomainIds = KingdomHelper.GetAllDomainsIdInKingdoms(context.Domains, domain);
            var allTargerDomains = commandType switch
            {
                enArmyCommandType.ForDelete => throw new NotImplementedException(),
                enArmyCommandType.War => GetKingdomNeiborDomains(context, kingdomDomainIds),
                enArmyCommandType.CollectTax => throw new NotImplementedException(),
                enArmyCommandType.WarSupportDefense => kingdomDomainIds,
                enArmyCommandType.WarSupportAttack => GetKingdomNeiborDomains(context, kingdomDomainIds),
                _ => throw new NotImplementedException(),
            };
            var targetRoutes = GetKingdomNeiborRoutes(context, allTargerDomains, unit);
            targetRoutes = targetRoutes
                .OrderBy(t => t.TargetDomain.Name)
                .OrderBy(r => r.Distance);
            return targetRoutes;
        }

        private static List<int> GetKingdomNeiborDomains(ApplicationDbContext context, List<int> kingdomDomainIds)
        {
            var kingdomNeiborDomains = new List<int>();
            foreach (var domainId in kingdomDomainIds)
            {
                var neibors = RouteHelper.GetNeighbors(context, domainId);
                var newNeibors = neibors
                    .Where(d => !kingdomDomainIds.Contains(d.Id))
                    .Where(d => !kingdomNeiborDomains.Contains(d.Id))
                    .Select(d => d.Id);
                kingdomNeiborDomains.AddRange(newNeibors);
            }
            return kingdomNeiborDomains;
        }

        private static IEnumerable<GameMapRoute> GetKingdomNeiborRoutes(ApplicationDbContext context,
            List<int> kingdomNeiborDomains, Unit unit)
        {
            var kingdomNeiborRoutes = new List<GameMapRoute>();
            foreach (var targetDomainId in kingdomNeiborDomains)
            {
                var routeFindParameters = new RouteFindParameters(unit, enMovementReason.Atack, targetDomainId);
                var route = RouteHelper.FindRoute(context, routeFindParameters);
                if (route != null)
                {
                    var targetDomain = context.Domains.Find(targetDomainId);
                    var gameMapRoute = new GameMapRoute(targetDomain, route.Count - 1);
                    kingdomNeiborRoutes.Add(gameMapRoute);
                }
            }
            return kingdomNeiborRoutes;
        }
    }
}

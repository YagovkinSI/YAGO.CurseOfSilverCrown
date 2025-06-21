using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YAGO.World.Infrastructure.Database.Models.Units;
using YAGO.World.Infrastructure.Helpers.Map.Routes;
using YAGO.World.Infrastructure.APIModels;
using YAGO.World.Infrastructure.Database;

namespace YAGO.World.Infrastructure.Helpers
{
    public static class WarBaseHelper
    {
        public static async Task<IEnumerable<GameMapRoute>> GetAvailableTargets(
            ApplicationDbContext context,
            int domainId,
            Unit unit,
            UnitCommandType commandType)
        {
            var domain = await context.Domains.FindAsync(domainId);
            var kingdomDomainIds = KingdomHelper.GetAllDomainsIdInKingdoms(context.Domains, domain);
            var allTargerDomains = commandType switch
            {
                UnitCommandType.ForDelete => throw new NotImplementedException(),
                UnitCommandType.War => GetKingdomNeiborDomains(context, kingdomDomainIds),
                UnitCommandType.Disbandment => throw new NotImplementedException(),
                UnitCommandType.WarSupportDefense => kingdomDomainIds,
                UnitCommandType.WarSupportAttack => GetKingdomNeiborDomains(context, kingdomDomainIds),
                _ => throw new NotImplementedException(),
            };
            var targetRoutes = GetTargetRoutes(context, allTargerDomains, unit);
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

        private static IEnumerable<GameMapRoute> GetTargetRoutes(ApplicationDbContext context,
            List<int> targetDomainIds, Unit unit)
        {
            var kingdomNeiborRoutes = new List<GameMapRoute>();
            foreach (var targetDomainId in targetDomainIds)
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

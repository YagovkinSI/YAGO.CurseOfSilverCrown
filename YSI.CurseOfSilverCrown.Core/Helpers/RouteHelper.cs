using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.ViewModels;

namespace YSI.CurseOfSilverCrown.Core.Helpers
{
    public static class RouteHelper
    {
        public static List<Domain> GetNeighbors(this ApplicationDbContext context, int domainId)
        {
            return context.Routes
                .Where(r => r.FromDomainId == domainId)
                .Select(r => r.ToDomain)
                .ToList();
        }

        public static bool IsNeighbors(ApplicationDbContext context, int domainId1, int domainId2)
        {
            var neighbors = GetNeighbors(context, domainId1);
            return neighbors.Any(n => n.Id == domainId2);
        }

        public static List<GameMapRoute> GetAvailableRoutes(this ApplicationDbContext context,
            int startSomanId, int maxSteps = int.MaxValue)
        {
            var startDomain = context.Domains.Find(startSomanId);
            var usedDomains = new List<GameMapRoute>();
            var fromDomains = new List<GameMapRoute> {
                new GameMapRoute(startDomain, 1)
            };

            var step = 0;
            do
            {
                if (step == maxSteps)
                {
                    usedDomains.AddRange(fromDomains);
                    break;
                }
                var newFromDomains = new List<Domain>();
                foreach (var fromDomain in fromDomains)
                {
                    var neighbors = GetNeighbors(context, fromDomain.TargetDomain.Id);
                    usedDomains.Add(fromDomain);
                    var neighborLords = neighbors
                        .Where(o => !usedDomains.Any(u => u.TargetDomain.Id == o.Id) && !newFromDomains.Any(u => u.Id == o.Id));
                    foreach (var neighborLord in neighborLords)
                        newFromDomains.Add(neighborLord);
                }
                fromDomains = newFromDomains
                    .Where(o => !usedDomains.Any(u => u.TargetDomain.Id == o.Id))
                    .Select(d => new GameMapRoute(d, step + 1))
                    .ToList();
                step++;
            }
            while (fromDomains.Any());

            return usedDomains;
        }

        public static int GetNextPosition(ApplicationDbContext context, int domainId, int domainIdFrom, int domainIdTo,
            bool needIntoTarget)
        {
            var domain = context.Domains.Find(domainId);
            var domainFrom = context.Domains.Find(domainIdFrom);
            var fromRoutes = new List<List<Domain>> { new List<Domain> { domainFrom } };
            var usedDomains = new List<Domain>();

            do
            {
                var newFromRoutes = new List<List<Domain>>();
                foreach (var route in fromRoutes)
                {
                    var neighbors = GetNeighbors(context, route.Last().Id);
                    usedDomains.Add(route.Last());
                    var neighborLords = neighbors
                        .Where(o => !usedDomains.Any(u => u.Id == o.Id))
                        .OrderBy(o => o.MoveOrder);
                    if (!needIntoTarget && neighborLords.Any(n => n.Id == domainIdTo))
                    {
                        return route.Count == 1
                            ? domainIdTo
                            : route[1].Id;
                    }

                    foreach (var neighborLord in neighborLords)
                    {
                        var hasPermissionOfPassage = KingdomHelper.IsSameKingdoms(context.Domains, domain, neighborLord) ||
                            DomainRelationsHelper.HasPermissionOfPassage(context, domain.Id, neighborLord.Id);
                        if (hasPermissionOfPassage)
                        {
                            if (needIntoTarget && neighborLord.Id == domainIdTo)
                            {
                                return route.Count == 1
                                    ? neighborLord.Id
                                    : route[1].Id;
                            }

                            var newRoute = route.ToList();
                            newRoute.Add(neighborLord);
                            newFromRoutes.Add(newRoute);
                        }
                        else
                        {
                            usedDomains.Add(neighborLord);
                        }
                    }
                }
                fromRoutes = newFromRoutes;
            }
            while (fromRoutes.Any());

            return domainFrom.Id;
        }
    }
}

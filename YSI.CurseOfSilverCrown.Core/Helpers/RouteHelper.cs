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
            bool needIntoTarget, out int fullSteps)
        {
            fullSteps = int.MaxValue;
            var domainFrom = context.Domains.Find(domainIdFrom);
            if (needIntoTarget && !CanEnterDomain(context, domainId, domainIdTo))
                return domainFrom.Id;

            var route = FindRoute(context, domainId, domainIdFrom, domainIdTo);
            if (route == null)
                return domainIdFrom;

            fullSteps = route.Count - 1;
            return route[1].Id;
        }

        private static List<Domain> FindRoute(ApplicationDbContext context, int domainId, int domainIdFrom, int domainIdTo)
        {
            var domainFrom = context.Domains.Find(domainIdFrom);

            var usedDomains = new List<Domain>();
            var fromRoutes = new List<List<Domain>> { new List<Domain> { domainFrom } };
            do
            {
                var newFromRoutes = new List<List<Domain>>();
                foreach (var route in fromRoutes)
                {
                    var neighborDomains = GetNeighbors(context, route.Last().Id)
                        .Where(o => !usedDomains.Any(u => u.Id == o.Id))
                        .OrderBy(o => o.MoveOrder);
                    if (IsFoundRoute(route, domainIdTo, neighborDomains, out var finalRoute))
                        return finalRoute;

                    foreach (var neighborDomain in neighborDomains)
                    {
                        if (CanEnterDomain(context, domainId, neighborDomain.Id))
                        {
                            var newRoute = route.ToList();
                            newRoute.Add(neighborDomain);
                            newFromRoutes.Add(newRoute);
                        }
                        else
                        {
                            usedDomains.Add(neighborDomain);
                        }
                    }
                    usedDomains.Add(route.Last());
                }
                fromRoutes = newFromRoutes;
            }
            while (fromRoutes.Any());
            return null;
        }

        private static bool CanEnterDomain(ApplicationDbContext context, int domainId, int domainIdTo)
        {
            var domain = context.Domains.Find(domainId);
            var domainTo = context.Domains.Find(domainIdTo);
            return KingdomHelper.IsSameKingdoms(context.Domains, domain, domainTo) ||
                DomainRelationsHelper.HasPermissionOfPassage(context, domain.Id, domainTo.Id);
        }

        private static bool IsFoundRoute(List<Domain> route, int domainToId, IEnumerable<Domain> checkDomainList,
            out List<Domain> finalRoute)
        {
            finalRoute = null;
            var domain = checkDomainList.FirstOrDefault(d => d.Id == domainToId);
            if (domain == null)
                return false;

            finalRoute = route.ToList();
            finalRoute.Add(domain);
            return true;
        }
    }
}

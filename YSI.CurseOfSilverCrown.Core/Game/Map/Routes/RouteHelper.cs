using System;
using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Core.MainModels;
using YSI.CurseOfSilverCrown.Core.ViewModels;

namespace YSI.CurseOfSilverCrown.Core.Game.Map.Routes
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
            var neighbors = context.GetNeighbors(domainId1);
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
                    var neighbors = context.GetNeighbors(fromDomain.TargetDomain.Id);
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

        //TODO: Big method
        public static List<Domain> FindRoute(ApplicationDbContext context, RouteFindParameters routeFindParameters)
        {
            if (routeFindParameters.NeedIntoTarget &&
                !CanEnterDomain(context, routeFindParameters, routeFindParameters.ToDomainId))
            {
                return null;
            }

            var domainFrom = context.Domains.Find(routeFindParameters.FromDomainId);
            if (routeFindParameters.FromDomainId == routeFindParameters.ToDomainId)
                return new List<Domain> { domainFrom };

            var usedDomains = new List<Domain>();
            var fromRoutes = new List<List<Domain>> { new List<Domain> { domainFrom } };
            do
            {
                var newFromRoutes = new List<List<Domain>>();
                foreach (var route in fromRoutes)
                {
                    var neighborDomains = context.GetNeighbors(route.Last().Id)
                        .Where(o => !usedDomains.Any(u => u.Id == o.Id))
                        .OrderBy(o => o.MoveOrder);
                    if (IsFoundRoute(route, routeFindParameters.ToDomainId, neighborDomains, out var finalRoute))
                        return finalRoute;

                    foreach (var neighborDomain in neighborDomains)
                    {
                        if (CanEnterDomain(context, routeFindParameters, neighborDomain.Id))
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

        private static bool CanEnterDomain(ApplicationDbContext context, RouteFindParameters routeFindParameters, int neighborDomainId)
        {
            var domain = context.Domains.Find(routeFindParameters.UnitDomainId);
            var neighborDomain = context.Domains.Find(neighborDomainId);
            if (context.Domains.IsSameKingdoms(domain, neighborDomain))
                return true;

            switch (routeFindParameters.MovementReason)
            {
                case enMovementReason.Atack:
                    return false;
                case enMovementReason.SupportAttack:
                    return IsSupporting(context, routeFindParameters.SupportingDomainId.Value, neighborDomain);
                case enMovementReason.Defense:
                    return DomainRelationsHelper.HasPermissionOfPassage(context, domain.Id, neighborDomain.Id);
                case enMovementReason.Moving:
                    return false;
                case enMovementReason.Retreat:
                    var unit = context.Units.Find(routeFindParameters.UnitId);
                    if (unit.Type == Database.Enums.enArmyCommandType.WarSupportAttack)
                        return IsSupporting(context, unit.Target2DomainId.Value, neighborDomain);
                    return DomainRelationsHelper.HasPermissionOfPassage(context, domain.Id, neighborDomain.Id);
                default:
                    throw new NotImplementedException();
            }
        }

        private static bool IsSupporting(ApplicationDbContext context, int supportingDomainId, Domain neighborDomain)
        {
            var supportingDomain = context.Domains.Find(supportingDomainId);
            return context.Domains.IsSameKingdoms(supportingDomain, neighborDomain);
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

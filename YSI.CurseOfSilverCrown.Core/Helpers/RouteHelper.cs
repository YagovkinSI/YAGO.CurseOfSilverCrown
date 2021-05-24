using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models;
using YSI.CurseOfSilverCrown.Core.Database.Enums;

namespace YSI.CurseOfSilverCrown.Core.Helpers
{
    public static class RouteHelper
    {
        public static List<Domain> GetNeighbors(this ApplicationDbContext context, int domainId)
        {
            return context.Routes
                .Include(r => r.ToDomain)
                .Where(r => r.FromDomainId == domainId)
                .Select(r => r.ToDomain)
                .ToList();
        }

        public static List<Domain> GetAvailableRoutes(this ApplicationDbContext context, Domain organization, int startSomanId, int maxSteps = int.MaxValue)
        {
            var startDomain = context.Domains.Find(startSomanId);
            var usedDomains = new List<Domain>();
            var fromDomains = new List<Domain> { startDomain };

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
                    var neighbors = GetNeighbors(context, fromDomain.Id);
                    usedDomains.Add(fromDomain);
                    var neighborLords = neighbors
                        .Where(o => !usedDomains.Any(u => u.Id == o.Id) && !newFromDomains.Any(u => u.Id == o.Id));
                    foreach (var neighborLord in neighborLords)
                    {
                        var IsSameKingdoms = KingdomHelper.IsSameKingdoms(context.Domains, organization, neighborLord);
                        if (IsSameKingdoms)
                            newFromDomains.Add(neighborLord);
                        else
                            usedDomains.Add(neighborLord);
                    }

                }
                fromDomains = newFromDomains
                    .Where(o => !usedDomains.Any(u => u.Id == o.Id))
                    .ToList();
                step++;
            }
            while (fromDomains.Any());

            return usedDomains;
        }

        public static int GetNextPosition(ApplicationDbContext context, int domainId, int domainIdFrom, int domainIdTo)
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
                        .Where(o => !usedDomains.Any(u => u.Id == o.Id));
                    if (neighborLords.Any(n => n.Id == domainIdTo))
                        return route.Count == 1
                            ? domainIdTo
                            : route[1].Id;

                    foreach (var neighborLord in neighborLords)
                    {
                        var IsSameKingdoms = KingdomHelper.IsSameKingdoms(context.Domains, domain, neighborLord);
                        if (IsSameKingdoms)
                        {
                            var newRoute = route.ToList();
                            newRoute.Add(neighborLord);
                            newFromRoutes.Add(newRoute);
                        }
                        else
                            usedDomains.Add(neighborLord);
                    }
                }
                fromRoutes = newFromRoutes;
            }
            while (fromRoutes.Any());

            return domainFrom.Id;
        }
    }
}

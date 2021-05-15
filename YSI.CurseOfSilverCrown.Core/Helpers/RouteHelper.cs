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
        public async static Task<List<Domain>> GetNeighbors(this ApplicationDbContext context, int domainId)
        {
            return await context.Routes
                .Include(r => r.ToDomain)
                .Where(r => r.FromDomainId == domainId)
                .Select(r => r.ToDomain)
                .ToListAsync();
        }

        public async static Task<List<Domain>> GetAvailableRoutes(this ApplicationDbContext context, Domain organization)
        {
            var usedDomains = new List<Domain>();
            var fromDomains = new List<Domain> { organization };

            do
            {
                var newFromDomains = new List<Domain>();
                foreach (var fromDomain in fromDomains)
                {
                    var neighbors = await GetNeighbors(context, fromDomain.Id);
                    usedDomains.Add(fromDomain);
                    var neighborLords = neighbors
                        .Where(o => !usedDomains.Any(u => u.Id == o.Id) && !newFromDomains.Any(u => u.Id == o.Id));
                    foreach (var neighborLord in neighborLords)
                    {
                        var IsSameKingdoms = await KingdomHelper.IsSameKingdoms(context.Domains, organization, neighborLord);
                        if (IsSameKingdoms)
                            newFromDomains.Add(neighborLord);
                        else
                            usedDomains.Add(neighborLord);
                    }

                }
                fromDomains = newFromDomains
                    .Where(o => !usedDomains.Any(u => u.Id == o.Id))
                    .ToList();
            }
            while (fromDomains.Any());

            return usedDomains;
        }
    }
}

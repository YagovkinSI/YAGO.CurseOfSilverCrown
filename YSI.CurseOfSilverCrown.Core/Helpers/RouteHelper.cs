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
        public async static Task<List<Province>> GetNeighbors(this ApplicationDbContext context, int provinceId)
        {
            return await context.Routes
                .Include(r => r.ToProvince)
                .Include("ToProvince.Organizations")
                .Where(r => r.FromProvinceId == provinceId)
                .Select(r => r.ToProvince)
                .ToListAsync();
        }

        public async static Task<List<Organization>> GetAvailableRoutes(this ApplicationDbContext context, Organization organization)
        {
            var usedProvinces = new List<Organization>();
            var fromProvinces = new List<Organization> { organization };

            do
            {
                var newFromProvinces = new List<Organization>();
                foreach (var fromProvince in fromProvinces)
                {
                    var neighbors = await GetNeighbors(context, fromProvince.ProvinceId);
                    usedProvinces.Add(fromProvince);
                    var neighborLords = neighbors
                        .SelectMany(p => p.Organizations)
                        .Where(o => o.OrganizationType == enOrganizationType.Lord)
                        .Where(o => !usedProvinces.Any(u => u.Id == o.Id) && !newFromProvinces.Any(u => u.Id == o.Id));
                    foreach (var neighborLord in neighborLords)
                    {
                        var IsSameKingdoms = await KingdomHelper.IsSameKingdoms(context.Organizations, organization, neighborLord);
                        if (IsSameKingdoms)
                            newFromProvinces.Add(neighborLord);
                        else
                            usedProvinces.Add(neighborLord);
                    }

                }
                fromProvinces = newFromProvinces
                    .Where(o => !usedProvinces.Any(u => u.Id == o.Id))
                    .ToList();
            }
            while (fromProvinces.Any());

            return usedProvinces;
        }
    }
}

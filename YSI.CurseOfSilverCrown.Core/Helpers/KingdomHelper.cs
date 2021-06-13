using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models;

namespace YSI.CurseOfSilverCrown.Core.Helpers
{
    public static class KingdomHelper
    {
        public static Domain GetKingdomCapital(List<Domain> allOrganizations, Domain organization)
        {
            if (organization.SuzerainId == null)
                return organization;

            var suzerain = allOrganizations
                .Single(o => o.Id == organization.SuzerainId);
            return GetKingdomCapital(allOrganizations, suzerain);
        }

        public static Domain GetKingdomCapital(this DbSet<Domain> organizationsDbSet, Domain organization)
        {
            if (organization.SuzerainId == null)
                return organization;

            var suzerain = organizationsDbSet
                .Single(o => o.Id == organization.SuzerainId);
            return GetKingdomCapital(organizationsDbSet, suzerain);
        }

        public static bool IsSameKingdoms(this DbSet<Domain> organizationsDbSet, Domain organization1, Domain organization2)
        {
            var kingdomCapital1 = GetKingdomCapital(organizationsDbSet, organization1);
            var kingdomCapital2 = GetKingdomCapital(organizationsDbSet, organization2);
            return kingdomCapital1.Id == kingdomCapital2.Id;
        }

        public static List<int> GetAllDomainsIdInKingdoms(this DbSet<Domain> organizationsDbSet, Domain organization)
        {
            var kingdomCapital = GetKingdomCapital(organizationsDbSet, organization);

            return GetAllLevelVassalIds(organizationsDbSet, kingdomCapital.Id);
        }

        private static List<int> GetAllLevelVassalIds(this DbSet<Domain> organizationsDbSet, int suzerainId, List<int> currentList = null)
        {
            if (currentList == null)
                currentList = new List<int>();

            currentList.Add(suzerainId);

            var vassals = organizationsDbSet
                .Where(o => o.SuzerainId == suzerainId)
                .ToList();

            foreach (var vassal in vassals)
            {
                var ids = GetAllLevelVassalIds(organizationsDbSet, vassal.Id, currentList);
                currentList.AddRange(ids);
            }

            return currentList
                .Distinct()
                .ToList();
        }

        public static Color GetColor(ApplicationDbContext context, Domain domain)
        {
            var allDomains = context.Domains
                .Include(p => p.Person)
                .Include("Person.User")
                .Include(p => p.Suzerain)
                .Include(p => p.Vassals)
                .Include(p => p.UnitsHere)
                .ToList();
            var count = allDomains.Count;
            var colorParts = (int)Math.Ceiling(Math.Pow(count, 1 / 3.0));
            var colorStep = 255 / (colorParts - 1);
            var colorCount = (int)Math.Pow(colorParts, 3);
            var sqrt = (int)Math.Floor(Math.Sqrt(colorCount));

            var capital = GetKingdomCapital(context.Domains, domain);
            var king = context.Persons
                .Single(p => p.Id == capital.PersonId);

            var colorId = -1;
            if (allDomains.Any(d => d.PersonId == king.Id && d.Id == king.Id))
                colorId = king.Id;
            if (colorId == -1)            
                colorId = allDomains
                    .Where(d => d.PersonId == king.Id)
                    .Min(d => d.Id);
            

            var colorNum = (king.Id % sqrt * (colorCount / sqrt)) + (king.Id / sqrt);

            var colorR = colorNum % colorParts * colorStep;
            var colorG = (colorNum / colorParts) % colorParts * colorStep;
            var colorB = (colorNum / colorParts / colorParts) % colorParts * colorStep;

            return Color.FromArgb(colorR, colorG, colorB);
        }
    }
}

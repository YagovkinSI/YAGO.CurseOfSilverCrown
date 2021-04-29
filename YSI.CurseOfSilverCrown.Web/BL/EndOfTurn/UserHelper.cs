using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.Data;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;

namespace YSI.CurseOfSilverCrown.Web.BL.EndOfTurn
{
    public static class KingdomHelper
    {
        public async static Task<Organization> GetKingdomCapital(this DbSet<Organization> organizationsDbSet, Organization organization)
        {
            if (organization.SuzerainId == null)
                return organization;

            var suzerain = await organizationsDbSet
                .SingleAsync(o => o.Id == organization.SuzerainId);
            return await GetKingdomCapital(organizationsDbSet, suzerain);
        }

        public async static Task<bool> IsSameKingdoms(this DbSet<Organization> organizationsDbSet, Organization organization1, Organization organization2)
        {
            var kingdomCapital1 = await GetKingdomCapital(organizationsDbSet, organization1);
            var kingdomCapital2 = await GetKingdomCapital(organizationsDbSet, organization2);
            return kingdomCapital1.Id == kingdomCapital2.Id;
        }

        public async static Task<List<string>> GetAllProvincesIdInKingdoms(this DbSet<Organization> organizationsDbSet, Organization organization)
        {
            var kingdomCapital = await GetKingdomCapital(organizationsDbSet, organization);

            return GetAllLevelVassalIds(organizationsDbSet, kingdomCapital.Id);
        }

        private static List<string> GetAllLevelVassalIds(this DbSet<Organization> organizationsDbSet, string suzerainId, List<string> currentList = null)
        {
            if (currentList == null)
                currentList = new List<string>();

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
    }
}

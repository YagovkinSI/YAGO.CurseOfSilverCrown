
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models;

namespace YSI.CurseOfSilverCrown.Core.Helpers
{
    public static class DomainRelationsHelper
    {
        public static bool HasPermissionOfPassage(ApplicationDbContext context, int domainFromId, int domainToId)
        {
            var suzerainFromList = new List<int>();
            var suzerainFrom = context.Domains
                .Include(d => d.Suzerain)
                .Single(d => d.Id == domainFromId);
            while (suzerainFrom != null)
            {
                suzerainFromList.Add(suzerainFrom.Id);
                if (suzerainFrom.Suzerain == null)
                    break;
                suzerainFrom = context.Domains
                    .Include(d => d.Suzerain)
                    .Single(d => d.Id == suzerainFrom.Suzerain.Id);
            }

            var suzerainTo = context.Domains
                .Include(d => d.Suzerain)
                .Single(d => d.Id == domainToId);
            while (suzerainTo != null)
            {
                var needIncludeVassals = false;
                DomainRelation domainRelation = null;
                foreach (var targetId in suzerainFromList)
                {
                    domainRelation = context.DomainRelations
                        .SingleOrDefault(r => r.SourceDomainId == suzerainTo.Id &&
                                              r.TargetDomainId == targetId &&
                                              (!needIncludeVassals || r.IsIncludeVassals));
                    if (domainRelation != null)
                        break;
                    needIncludeVassals = true;
                }                
                if (domainRelation != null)
                    return domainRelation.PermissionOfPassage;
                if (suzerainTo.Suzerain == null)
                    return false;
                suzerainTo = context.Domains
                    .Include(d => d.Suzerain)
                    .Single(d => d.Id == suzerainTo.Suzerain.Id);
            }
            return false;
        }
    }
}

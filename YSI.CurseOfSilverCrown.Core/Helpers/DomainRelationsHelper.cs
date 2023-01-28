using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.Database.EF;
using YSI.CurseOfSilverCrown.Core.Database.Models.GameWorld;

namespace YSI.CurseOfSilverCrown.Core.Helpers
{
    public static class DomainRelationsHelper
    {
        public static bool HasPermissionOfPassage(ApplicationDbContext context, int domainFromId, int domainToId)
        {
            var suzerainFromList = new List<int>();
            var suzerainFrom = context.Domains.Find(domainFromId);
            while (suzerainFrom != null)
            {
                suzerainFromList.Add(suzerainFrom.Id);
                if (suzerainFrom.SuzerainId == null)
                    break;
                suzerainFrom = context.Domains.Find(suzerainFrom.SuzerainId);
            }

            var suzerainTo = context.Domains.Find(domainToId);
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
                    return false; //domainRelation.PermissionOfPassage;
                if (suzerainTo.SuzerainId == null)
                    return false;
                suzerainTo = context.Domains.Find(suzerainTo.SuzerainId);
            }
            return false;
        }

        public static IEnumerable<Domain> GetRelationDefenseDomains(ApplicationDbContext context, int domainId)
        {
            var allSuzerainIds = new List<int>();
            var vassalId = (int?)domainId;
            while (true)
            {
                var vassal = context.Domains.Find(vassalId);
                if (vassal.SuzerainId == null)
                    break;
                allSuzerainIds.Add(vassal.SuzerainId.Value);
                vassalId = vassal.SuzerainId;
            }

            var relationDefenders = context.DomainRelations
                .Where(r => r.TargetDomainId == domainId ||
                    (r.IsIncludeVassals && allSuzerainIds.Contains(r.TargetDomainId)))
                .Select(r => r.SourceDomain)
                .ToList();
            return relationDefenders;
        }
    }
}

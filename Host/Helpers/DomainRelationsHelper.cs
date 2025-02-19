using System.Collections.Generic;
using System.Linq;
using YAGO.World.Host.Database.Domains;
using YAGO.World.Host.Database.Relations;
using YAGO.World.Host.Infrastructure.Database;

namespace YAGO.World.Host.Helpers
{
    public static class DomainRelationsHelper
    {
        //TODO: Big method
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
                Relation domainRelation = null;
                foreach (var sourceDomain in suzerainFromList)
                {
                    domainRelation = context.Relations
                        .SingleOrDefault(r => r.SourceDomainId == sourceDomain &&
                                              r.TargetDomainId == suzerainTo.Id &&
                                              (!needIncludeVassals || r.IsIncludeVassals));
                    if (domainRelation != null)
                        break;
                    needIncludeVassals = true;
                }
                if (domainRelation != null)
                    return true; //domainRelation.PermissionOfPassage;
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

            var relationDefenders = context.Relations
                .Where(r => r.TargetDomainId == domainId ||
                    (r.IsIncludeVassals && allSuzerainIds.Contains(r.TargetDomainId)))
                .Select(r => r.SourceDomain)
                .ToList();
            return relationDefenders;
        }
    }
}

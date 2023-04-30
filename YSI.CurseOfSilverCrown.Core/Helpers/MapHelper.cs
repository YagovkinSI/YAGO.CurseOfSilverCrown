using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.APIModels;
using YSI.CurseOfSilverCrown.Core.Database;

namespace YSI.CurseOfSilverCrown.Core.Helpers
{
    public static class MapHelper
    {
        public static List<DomainPublic> GetMap(ApplicationDbContext context)
        {
            var allDomains = context.Domains.ToList();

            var map = new List<DomainPublic>();
            foreach (var domain in allDomains)
            {
                var colorKingdom = KingdomHelper.GetColor(context, allDomains, domain);
                var domainPublic = new DomainPublic()
                {
                    Id = domain.Id,
                    Name = domain.Name,
                    ColorKingdom = $"rgba({colorKingdom.R}, {colorKingdom.G}, {colorKingdom.B}, 0.7)"
                };
                map.Add(domainPublic);
            }
            return map;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using YSI.CurseOfSilverCrown.Core.APIModels;
using YSI.CurseOfSilverCrown.Core.Database;

namespace YSI.CurseOfSilverCrown.Core.Helpers
{
    public static class MapHelper
    {
        public static List<MapElementApi> GetMap(ApplicationDbContext context)
        {
            var allDomains = context.Domains.ToList();

            var mapElements = new List<MapElementApi>();
            foreach (var domain in allDomains)
            {
                var colorKingdom = KingdomHelper.GetColor(context, allDomains, domain);
                var mapElement = new MapElementApi()
                {
                    Id = domain.Id,
                    Name = domain.Name,
                    ColorKingdom = $"rgba({colorKingdom.R}, {colorKingdom.G}, {colorKingdom.B}, 0.7)"
                };
                mapElements.Add(mapElement);
            }
            return mapElements;
        }
    }
}

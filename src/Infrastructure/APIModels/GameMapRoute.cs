using YAGO.World.Infrastructure.Database.Models.Domains;

namespace YAGO.World.Infrastructure.APIModels
{
    public class GameMapRoute
    {
        public Organization TargetDomain { get; set; }
        public int Distance { get; set; }

        public string RouteName => $"{TargetDomain.Name} ({Distance})";

        public GameMapRoute(Organization targetDomain, int disatanse)
        {
            TargetDomain = targetDomain;
            Distance = disatanse;
        }

    }
}

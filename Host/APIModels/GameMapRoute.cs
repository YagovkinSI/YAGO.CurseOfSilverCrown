using YAGO.World.Host.Database.Domains;

namespace YAGO.World.Host.APIModels
{
    public class GameMapRoute
    {
        public Domain TargetDomain { get; set; }
        public int Distance { get; set; }

        public string RouteName => $"{TargetDomain.Name} ({Distance})";

        public GameMapRoute(Domain targetDomain, int disatanse)
        {
            TargetDomain = targetDomain;
            Distance = disatanse;
        }

    }
}

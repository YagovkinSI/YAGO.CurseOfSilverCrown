using YSI.CurseOfSilverCrown.Core.Database.Domains;

namespace YSI.CurseOfSilverCrown.Core.APIModels
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

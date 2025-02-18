using YSI.CurseOfSilverCrown.Web.Database.Domains;

namespace YSI.CurseOfSilverCrown.Web.APIModels
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

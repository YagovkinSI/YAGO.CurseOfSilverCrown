using YAGO.World.Domain.Common;

namespace YAGO.World.Domain.Wiki
{
    public class WikiArticle
    {
        public string Code { get; }
        public string Section { get; }
        public int Order { get; }
        public DisplayInfo DisplayInfo { get; }

        public WikiArticle(
            string code,
            string section,
            int order,
            DisplayInfo displayInfo)
        {
            Code = code;
            Section = section;
            Order = order;
            DisplayInfo = displayInfo;
        }
    }
}

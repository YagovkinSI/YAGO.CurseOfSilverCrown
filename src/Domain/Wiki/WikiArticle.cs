using YAGO.World.Domain.Common;

namespace YAGO.World.Domain.Wiki
{
    public class WikiArticle
    {
        public string Code { get; }
        public DisplayInfo DisplayInfo { get; }

        public WikiArticle(
            string code,
            DisplayInfo displayInfo)
        {
            Code = code;
            DisplayInfo = displayInfo;
        }
    }
}

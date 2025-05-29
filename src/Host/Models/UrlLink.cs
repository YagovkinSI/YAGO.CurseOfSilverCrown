using YAGO.World.Infrastructure.APIModels.AspActions;

namespace YAGO.World.Host.Models
{
    public class UrlLink : ILink
    {
        public string Url { get; set; }
        public string DisplayText { get; set; }
        public bool OpenOnNewBlank { get; set; }

        public UrlLink(string url, string displayText, bool openOnNewBlank = false)
        {
            Url = url;
            DisplayText = displayText;
            OpenOnNewBlank = openOnNewBlank;
        }
    }
}